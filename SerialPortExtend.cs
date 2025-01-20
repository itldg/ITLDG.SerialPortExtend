using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static ITLDG.SerialPortExtend.NativeStructs;

namespace ITLDG.SerialPortExtend
{
    public static class SerialPortExtensions
    {
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        internal const uint ERROR_INVALID_HANDLE = 0x00000006;
        internal const uint ERROR_INSUFFICIENT_BUFFER = 122;

        internal const uint GENERIC_READ = 0x80000000;
        internal const uint GENERIC_WRITE = 0x40000000;
        internal const uint FILE_SHARE_READ = 0x00000001;
        internal const uint FILE_SHARE_WRITE = 0x00000002;

        internal const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const uint OPEN_EXISTING = 0x00000003;

        internal const uint NO_ERROR = 0x00000000;
        internal const uint STATUS_SUCCESS = 0x00000000;

        internal const uint DIGCF_DEFAULT = 0x00000001;
        internal const uint DIGCF_PRESENT = 0x00000002;
        internal const uint DIGCF_ALLCLASSES = 0x00000004;
        internal const uint DIGCF_PROFILE = 0x00000008;
        internal const uint DIGCF_DEVICEINTERFACE = 0x00000010;

        internal const uint ERROR_IO_PENDING = 997;
        internal const uint INFINITE = 0xFFFFFFFF;
        internal const uint WAIT_ABANDONED = 0x00000080;
        internal const uint WAIT_OBJECT_0 = 0x00000000;
        internal const uint WAIT_TIMEOUT = 0x00000102;

        internal const uint IOCTL_SERIAL_GET_PROPERTIES = 0x1b0074;
        internal const uint IOCTL_SERIAL_SET_QUEUE_SIZE = 0x1b0008;
        internal const uint IOCTL_SERIAL_GET_TIMEOUTS = 0x1b0020;
        internal const uint IOCTL_SERIAL_SET_TIMEOUTS = 0x1b001c;
        internal const uint IOCTL_SERIAL_PURGE = 0x1b004c;
        internal const uint IOCTL_SERIAL_GET_BAUD_RATE = 0x1b0050;
        internal const uint IOCTL_SERIAL_GET_LINE_CONTROL = 0x1b0054;
        internal const uint IOCTL_SERIAL_GET_CHARS = 0x1b0058;
        internal const uint IOCTL_SERIAL_GET_HANDFLOW = 0x1b0060;
        internal const uint IOCTL_SERIAL_SET_BAUD_RATE = 0x1b0004;
        internal const uint IOCTL_SERIAL_SET_RTS = 0x1b0030;
        internal const uint IOCTL_SERIAL_SET_DTR = 0x1b0024;
        internal const uint IOCTL_SERIAL_SET_LINE_CONTROL = 0x1b000c;
        internal const uint IOCTL_SERIAL_SET_CHARS = 0x1b005c;
        internal const uint IOCTL_SERIAL_SET_HANDFLOW = 0x1b0064;

        private const string COM_PATTERN = @"COM[0-9]+";
        private const string PORT_INFO_SELECT_SQL = "SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'";
        /// <summary>
        /// 获取串口名称
        /// </summary>
        /// <returns>返回串口名称词典</returns>
        public static List<SerialPortInfo> GetPortNames()
        {
            string[] portNames = new string[0];
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                portNames = SerialPort.GetPortNames();
                using (var searcher = new ManagementObjectSearcher(PORT_INFO_SELECT_SQL))
                {
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());
                    var portList = portNames.Select(n => ports.FirstOrDefault(s => s.EndsWith(n + ")"))).ToList();
                    foreach (string detail in portList)
                    {
                        if(detail == null)
                        {
                            continue;
                        }
                        if (Regex.Match(detail, COM_PATTERN) is Match match && match.Success && match.Groups != null &&
                            match.Value is string portName && !string.IsNullOrEmpty(portName) &&
                            !dict.ContainsKey(portName))
                        {
                            dict.Add(portName, detail);
                        }
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    foreach (var item in portNames)
                    {
                        if (int.TryParse(item.ToUpper().Replace("COM", ""), out _) && !dict.ContainsKey(item))
                        {
                            dict.Add(item, item);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            List<SerialPortInfo> list = new List<SerialPortInfo>();
            foreach (var item in dict)
            {
                list.Add(new SerialPortInfo() { COM = item.Key, Name = item.Value });
            }
            return list;
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(IntPtr hDevice, uint ioControlCode,
        [MarshalAs(UnmanagedType.LPArray)][In] byte[] inBuffer, int ninBufferSize,
        [MarshalAs(UnmanagedType.LPArray)][Out] byte[] outBuffer, int noutBufferSize,
        out uint bytesReturned, [In] IntPtr overlapped);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(IntPtr hDevice, uint ioControlCode, IntPtr inBuffer, int ninBufferSize,
            IntPtr outBuffer, int noutBufferSize, out uint bytesReturned, [In] IntPtr overlapped);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(IntPtr hDevice, uint ioControlCode, byte[] inBuffer, int ninBufferSize,
            IntPtr outBuffer, int noutBufferSize, out uint bytesReturned, [In] IntPtr overlapped);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(IntPtr hDevice, uint ioControlCode, IntPtr inBuffer, int ninBufferSize,
            byte[] outBuffer, int noutBufferSize, out uint bytesReturned, [In] IntPtr overlapped);

        /// <summary>
        /// 获取串口句柄
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <returns>串口句柄</returns>
        static IntPtr GetHandle(this SerialPort port)
        {
            object baseStream = port.BaseStream;
            Type baseStreamType = baseStream.GetType();

            // Get the Win32 file handle for the port
            SafeFileHandle portFileHandle = (SafeFileHandle)baseStreamType.GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(baseStream);
            return portFileHandle.DangerousGetHandle();
        }
        /// <summary>
        /// 检查句柄是否有效
        /// </summary>
        /// <param name="pHandle">要检查的句柄</param>
        /// <exception cref="Exception">句柄无效,串口可能未打开</exception>
        static void CheckHandle(IntPtr pHandle)
        {
            if (pHandle == IntPtr.Zero || pHandle == INVALID_HANDLE_VALUE)
            {
                throw new Exception("获取串口句柄异常,串口是否已打开?");
            }

        }
        /// <summary>
        /// 获取串行控制器功能的信息
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <returns>属性</returns>
        public static SERIAL_COMMPROP GetProperties(this SerialPort port)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            SERIAL_COMMPROP sSerialCompProp = new SERIAL_COMMPROP();
            IntPtr psSerialCompProp = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialCompProp));
            Marshal.StructureToPtr(sSerialCompProp, psSerialCompProp, false);
            DeviceIoControl(pHandle, IOCTL_SERIAL_GET_PROPERTIES, IntPtr.Zero, 0, psSerialCompProp,
                Marshal.SizeOf(sSerialCompProp), out _, IntPtr.Zero);
            Marshal.PtrToStructure(psSerialCompProp, sSerialCompProp);
            Marshal.FreeHGlobal(psSerialCompProp);
            return sSerialCompProp;
        }


        /// <summary>
        /// 请求设置内部接收缓冲区的大小。 如果请求的大小大于当前接收缓冲区大小，则会创建新的接收缓冲区。 否则，接收缓冲区不会更改。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <param name="sSerialQueueSize">缓冲大小</param>
        /// <returns>设置是否成功</returns>
        public static bool SetQueueSize(this SerialPort port, SERIAL_QUEUE_SIZE sSerialQueueSize)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            IntPtr psSerialQueueSize = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialQueueSize));
            Marshal.StructureToPtr(sSerialQueueSize, psSerialQueueSize, false);
            bool bResult = DeviceIoControl(pHandle, IOCTL_SERIAL_SET_QUEUE_SIZE, psSerialQueueSize,
                Marshal.SizeOf(sSerialQueueSize), IntPtr.Zero, 0, out _, IntPtr.Zero);
            Marshal.FreeHGlobal(psSerialQueueSize);
            return bResult;
        }

        /// <summary>
        /// 请求返回串行控制器驱动程序用于读取和写入请求的超时值。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <returns>超时信息</returns>
        public static SERIAL_TIMEOUTS GetTimeouts(this SerialPort port)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            SERIAL_TIMEOUTS sSerialTimeOuts = new SERIAL_TIMEOUTS();
            IntPtr psSerialTimeOuts = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialTimeOuts));
            Marshal.StructureToPtr(sSerialTimeOuts, psSerialTimeOuts, false);
            DeviceIoControl(pHandle, IOCTL_SERIAL_GET_TIMEOUTS, IntPtr.Zero, 0, psSerialTimeOuts,
                Marshal.SizeOf(sSerialTimeOuts), out _, IntPtr.Zero);
            Marshal.PtrToStructure(psSerialTimeOuts, sSerialTimeOuts);
            Marshal.FreeHGlobal(psSerialTimeOuts);
            return sSerialTimeOuts;
        }

        /// <summary>
        /// 请求设置串行控制器驱动程序用于读取和写入请求的超时值。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <param name="sSerialTimeOuts">超时信息</param>
        /// <returns>设置是否成功</returns>
        public static bool SetTimeouts(this SerialPort port, SERIAL_TIMEOUTS sSerialTimeOuts)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);


            IntPtr psSerialTimeOuts = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialTimeOuts));
            Marshal.StructureToPtr(sSerialTimeOuts, psSerialTimeOuts, false);
            bool bResult = DeviceIoControl(pHandle, IOCTL_SERIAL_SET_TIMEOUTS, psSerialTimeOuts,
                Marshal.SizeOf(sSerialTimeOuts), IntPtr.Zero, 0, out _, IntPtr.Zero);
            Marshal.FreeHGlobal(psSerialTimeOuts);
            return bResult;
        }

        /// <summary>
        /// 请求取消指定的请求，并从指定的缓冲区中删除数据。 清除请求可用于取消所有读取请求和写入请求，以及从接收缓冲区和传输缓冲区中删除所有数据。清除请求的完成并不表示清除请求取消的请求已完成。 在客户端释放或重用相应的 IRP 之前，客户端必须验证清除的请求是否已完成。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <param name="abyInputBuffer"></param>
        /// <returns>是否成功</returns>
        public static bool Purge(this SerialPort port, byte[] abyInputBuffer)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            return DeviceIoControl(pHandle, IOCTL_SERIAL_PURGE, abyInputBuffer, abyInputBuffer.Length,
                IntPtr.Zero, 0, out _, IntPtr.Zero);
        }
        ///// <summary>
        ///// 请求返回串行端口当前配置为发送和接收数据的波特率。
        ///// </summary>
        ///// <param name="port">已打开的串口</param>
        ///// <returns>波特率</returns>
        //public static SERIAL_BAUD_RATE GetBaudRate(this SerialPort port)
        //{
        //    IntPtr pHandle = port.GetHandle();
        //    CheckHandle(pHandle);

        //    SERIAL_BAUD_RATE sSerialBaudRate = new SERIAL_BAUD_RATE();
        //    IntPtr psSerialBaudRate = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialBaudRate));
        //    Marshal.StructureToPtr(sSerialBaudRate, psSerialBaudRate, false);
        //    DeviceIoControl(pHandle, IOCTL_SERIAL_GET_BAUD_RATE, IntPtr.Zero, 0, psSerialBaudRate,
        //        Marshal.SizeOf(sSerialBaudRate), out _, IntPtr.Zero);
        //    Marshal.PtrToStructure(psSerialBaudRate, sSerialBaudRate);
        //    Marshal.FreeHGlobal(psSerialBaudRate);
        //    return sSerialBaudRate;
        //}
        ///// <summary>
        ///// 请求在串行控制器设备上设置波特率。 串行控制器驱动程序验证指定的波特率。
        ///// </summary>
        ///// <param name="port">已打开的串口</param>
        ///// <param name="sSerialBaudRate">波特率</param>
        ///// <returns>设置是否成功</returns>
        //public static bool SetBaudRate(this SerialPort port, SERIAL_BAUD_RATE sSerialBaudRate)
        //{
        //    IntPtr pHandle = port.GetHandle();
        //    CheckHandle(pHandle);

        //    IntPtr psSerialBaudRate = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialBaudRate));
        //    Marshal.StructureToPtr(sSerialBaudRate, psSerialBaudRate, false);
        //    bool bResult = DeviceIoControl(pHandle, IOCTL_SERIAL_SET_BAUD_RATE, psSerialBaudRate,
        //        Marshal.SizeOf(sSerialBaudRate), IntPtr.Zero, 0, out _, IntPtr.Zero);
        //    Marshal.FreeHGlobal(psSerialBaudRate);
        //    return bResult;
        //}

        /// <summary>
        /// 请求返回有关串行设备的线路控制集的信息。 行控制参数包括停止位数、数据位数和奇偶校验。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <returns>串行行的控件设置</returns>
        public static SERIAL_LINE_CONTROL GetLineControl(this SerialPort port)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            SERIAL_LINE_CONTROL sSerialLineControl = new SERIAL_LINE_CONTROL();
            IntPtr psSerialLineControl = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialLineControl));
            Marshal.StructureToPtr(sSerialLineControl, psSerialLineControl, false);
            DeviceIoControl(pHandle, IOCTL_SERIAL_GET_LINE_CONTROL, IntPtr.Zero, 0, psSerialLineControl,
                Marshal.SizeOf(sSerialLineControl), out _, IntPtr.Zero);
            Marshal.PtrToStructure(psSerialLineControl, sSerialLineControl);
            Marshal.FreeHGlobal(psSerialLineControl);
            return sSerialLineControl;
        }
        /// <summary>
        /// 请求设置线路控制寄存器 （LCR）。线路控制寄存器控制数据大小、停止位数和奇偶校验。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <param name="sSerialLineControl">串行行的控件设置</param>
        /// <returns>设置是否成功</returns>
        public static bool SetLineControl(this SerialPort port, SERIAL_LINE_CONTROL sSerialLineControl)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            IntPtr psSerialLineControl = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialLineControl));
            Marshal.StructureToPtr(sSerialLineControl, psSerialLineControl, false);
            bool bResult = DeviceIoControl(pHandle, IOCTL_SERIAL_SET_LINE_CONTROL,
                psSerialLineControl, Marshal.SizeOf(sSerialLineControl), IntPtr.Zero, 0, out _, IntPtr.Zero);
            Marshal.FreeHGlobal(psSerialLineControl);
            return bResult;
        }


        /// <summary>
        /// 请求检索串行控制器驱动程序用于握手流控制的特殊字符。 特殊字符由 SERIAL_CHARS 结构描述。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <returns>获取结果</returns>
        public static SERIAL_CHARS GetChars(this SerialPort port)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            SERIAL_CHARS sSerialChars = new SERIAL_CHARS();
            IntPtr psSerialChars = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialChars));
            Marshal.StructureToPtr(sSerialChars, psSerialChars, false);
            DeviceIoControl(pHandle, IOCTL_SERIAL_GET_CHARS, IntPtr.Zero, 0, psSerialChars,
                Marshal.SizeOf(sSerialChars), out _, IntPtr.Zero);
            Marshal.PtrToStructure(psSerialChars, sSerialChars);
            Marshal.FreeHGlobal(psSerialChars);
            return sSerialChars;
        }
        /// <summary>
        /// 请求设置串行控制器驱动程序用于握手流控制的特殊字符。 此驱动程序验证指定的特殊字符。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <param name="sSerialChars">要设置的值</param>
        /// <returns>设置是否成功</returns>
        public static bool SetChars(this SerialPort port, SERIAL_CHARS sSerialChars)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            IntPtr psSerialChars = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialChars));
            Marshal.StructureToPtr(sSerialChars, psSerialChars, false);
            bool bResult = DeviceIoControl(pHandle, IOCTL_SERIAL_SET_CHARS, psSerialChars,
                Marshal.SizeOf(sSerialChars), IntPtr.Zero, 0, out _, IntPtr.Zero);
            Marshal.FreeHGlobal(psSerialChars);
            return bResult;
        }

        /// <summary>
        /// 请求返回有关为串行设备设置的握手流控制设置的信息。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <returns>获取结果</returns>
        public static SERIAL_HANDFLOW GetHandFlow(this SerialPort port)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            SERIAL_HANDFLOW sSerialHandFlow = new SERIAL_HANDFLOW();
            IntPtr psSerialHandFlow = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialHandFlow));
            Marshal.StructureToPtr(sSerialHandFlow, psSerialHandFlow, false);
            DeviceIoControl(pHandle, IOCTL_SERIAL_GET_HANDFLOW, IntPtr.Zero, 0, psSerialHandFlow,
                Marshal.SizeOf(sSerialHandFlow), out _, IntPtr.Zero);
            Marshal.PtrToStructure(psSerialHandFlow, sSerialHandFlow);
            Marshal.FreeHGlobal(psSerialHandFlow);
            return sSerialHandFlow;
        }

        /// <summary>
        /// 请求设置握手流控制的配置。 串行控制器驱动程序验证指定的握手流控制信息。
        /// </summary>
        /// <param name="port">已打开的串口</param>
        /// <param name="sSerialHandFlow">要设置的值</param>
        /// <returns>设置是否成功</returns>
        public static bool SetHandFlow(this SerialPort port, SERIAL_HANDFLOW sSerialHandFlow)
        {
            IntPtr pHandle = port.GetHandle();
            CheckHandle(pHandle);

            IntPtr psSerialHandFlow = Marshal.AllocHGlobal(Marshal.SizeOf(sSerialHandFlow));
            Marshal.StructureToPtr(sSerialHandFlow, psSerialHandFlow, false);
            bool bResult = DeviceIoControl(pHandle, IOCTL_SERIAL_SET_HANDFLOW, psSerialHandFlow,
                Marshal.SizeOf(sSerialHandFlow), IntPtr.Zero, 0, out _, IntPtr.Zero);
            Marshal.FreeHGlobal(psSerialHandFlow);
            return bResult;
        }
    }
}

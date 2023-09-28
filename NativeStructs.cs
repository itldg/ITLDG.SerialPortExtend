using System;
using System.Runtime.InteropServices;

namespace ITLDG.SerialPortExtend
{
    public class NativeStructs
    {
        /// <summary>
        /// 串行端口的属性
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_COMMPROP
        {
            /// <summary>
            /// 以此结构开头且包含所请求属性数据的数据包的大小（以字节为单位）。 此大小包括 SERIAL_COMMPROP 结构和遵循此结构的任何其他 ProvChar 数组元素。
            /// </summary>
            public UInt16 PacketLength;
            /// <summary>
            /// 此结构的版本。 当前版本号为 2。
            /// </summary>
            public UInt16 PacketVersion;
            /// <summary>
            /// 一个位掩码，指示此通信提供程序实现哪些服务。 此成员始终设置为由串行通信提供程序（包括调制解调器提供程序）SERIAL_SP_SERIALCOMM。 ProvSubType 成员指示 (实现的特定串行通信类型，例如调制解调器) 。
            /// </summary>
            public uint ServiceMask;
            /// <summary>
            /// 未使用。
            /// </summary>
            public uint Reserved1;
            /// <summary>
            /// 最大传输队列大小。 串行控制器驱动程序的内部输出缓冲区的最大大小（以字节为单位）。 值为零表示串行提供程序未施加任何最大值。
            /// </summary>
            public uint MaxTxQueue;
            /// <summary>
            /// 最大接收队列大小。 串行控制器驱动程序的内部输入缓冲区的最大大小（以字节为单位）。 值为零表示串行提供程序未施加任何最大值。
            /// </summary>
            public uint MaxRxQueue;
            /// <summary>
            /// 可接受的最大波特率。 基于扩展的串行控制器驱动程序将此成员设置为以比特/秒 (bps) 表示的波特率。 例如，如果串行控制器支持的最大波特率为 115，200 bps，则驱动程序会将 <b>MaxBaud</b> 设置为 115200。
            ///<para>但是，Serial.sys 和许多较旧的串行控制器驱动程序将 <b>MaxBaud</b> 设置为 Ntddser.h 头文件中定义的 SERIAL_BAUD_XXX 标志位之一。 为此成员定义了以下值。</para>
            ///<list type="table">
            ///<item>SERIAL_BAUD_075	75 bps</item>
            ///<item>SERIAL_BAUD_110	110 bps</item>
            ///<item>SERIAL_BAUD_134_5	134.5 bps</item>
            ///<item>SERIAL_BAUD_150	150 bps</item>
            ///<item>SERIAL_BAUD_300	300 bps</item>
            ///<item>SERIAL_BAUD_600	600 bps</item>
            ///<item>SERIAL_BAUD_1200	1，200 bps</item>
            ///<item>SERIAL_BAUD_1800	1，800 bps</item>
            ///<item>SERIAL_BAUD_2400	2，400 bps</item>
            ///<item>SERIAL_BAUD_4800	4，800 bps</item>
            ///<item>SERIAL_BAUD_7200	7，200 bps</item>
            ///<item>SERIAL_BAUD_9600	9，600 bps</item>
            ///<item>SERIAL_BAUD_14400	14，400 bps</item>
            ///<item>SERIAL_BAUD_19200	19，200 bps</item>
            ///<item>SERIAL_BAUD_38400	38，400 bps</item>
            ///<item>SERIAL_BAUD_56K	56，000 bps</item>
            ///<item>SERIAL_BAUD_57600	57，600 bps</item>
            ///<item>SERIAL_BAUD_115200	115，200 bps</item>
            ///<item>SERIAL_BAUD_128K	128，000 bps</item>
            ///<item>SERIAL_BAUD_USER	提供可编程波特率。</item>
            /// </list>
            /// </summary>
            public uint MaxBaud;
            /// <summary>
            /// 特定的通信提供程序类型。 当 ServiceMask 成员设置为 SERIAL_SP_SERIALCOMM 时， <b>ProvSubType</b> 设置为以下值之一。
            /// <list type="table">
            /// <item>SERIAL_SP_UNSPECIFIED	未指定</item>
            /// <item>SERIAL_SP_RS232 RS-232 串行端口</item>
            /// <item>SERIAL_SP_PARALLEL 并行端口</item>
            /// <item>SERIAL_SP_RS422 RS-422 串行端口</item>
            /// <item>SERIAL_SP_RS423 RS-423 串行端口</item>
            /// <item>SERIAL_SP_RS449 RS-449 串行端口</item>
            /// <item>SERIAL_SP_MODEM 调制解调器设备</item>
            /// <item>SERIAL_SP_FAX FAX 设备</item>
            /// <item>SERIAL_SP_SCANNER 扫描仪设备</item>
            /// <item>SERIAL_SP_BRIDGE 未指定的网络网桥</item>
            /// <item>SERIAL_SP_LAT LAT 协议</item>
            /// <item>SERIAL_SP_TELNET TCP/IP Telnet® 协议</item>
            /// <item>SERIAL_SP_X25 X.25 标准</item>
            /// </list>
            /// </summary>
            public uint ProvSubType;
            /// <summary>
            /// 提供程序提供的功能。 当 <b>ServiceMask</b> 成员设置为 SERIAL_SP_SERIALCOMM 时， <b>ProvSubType</b> 设置为以下值之一。
            /// <list type="table">
            /// <item>SERIAL_PCF_DTRDSR	支持 DTR (数据终端就绪) 和 DSR (数据集就绪) 。</item>
            /// <item>SERIAL_PCF_RTSCTS	支持发送) 的 RTS (请求和 CTS (明确发送) 。</item>
            /// <item>SERIAL_PCF_CD	支持 CD (运营商检测) 。</item>
            /// <item>SERIAL_PCF_PARITY_CHECK	支持奇偶校验检查。</item>
            /// <item>SERIAL_PCF_XONXOFF	支持) 上的 XON (传输和 XOFF (传输) 流控制。</item>
            /// <item>SERIAL_PCF_SETXCHAR	可设置 XON 和 XOFF 字符。</item>
            /// <item>SERIAL_PCF_TOTALTIMEOUTS	支持总已用时间超时。</item>
            /// <item>SERIAL_PCF_INTTIMEOUTS	支持间隔超时。</item>
            /// <item>SERIAL_PCF_SPECIALCHARS	支持特殊字符。</item>
            /// <item>SERIAL_PCF_16BITMODE	支持特殊的 16 位模式。</item>
            /// </list>
            /// </summary>
            public uint ProvCapabilities;
            /// <summary>
            /// 一个位掩码，指示可以更改的通信参数。 此成员设置为零或设置为以下一个或多个标志位的按位 OR。
            /// <list type="table">
            /// <item>SERIAL_SP_PARITY	奇偶校验类型 (偶数或奇数)</item>
            /// <item>SERIAL_SP_BAUD	波特率</item>
            /// <item>SERIAL_SP_DATABITS	数据位</item>
            /// <item>SERIAL_SP_STOPBITS	停止位</item>
            /// <item>SERIAL_SP_HANDSHAKING	握手 (流控制)</item>
            /// <item>SERIAL_SP_PARITY_CHECK	奇偶校验检查</item>
            /// <item>SERIAL_SP_CARRIER_DETECT	运营商检测</item>
            /// </list>
            /// </summary>
            public uint SettableParams;
            /// <summary>
            /// 指示可以使用的波特率的位掩码。 有关描述为此成员定义的 SERIAL_BAUD_XXX 标志位的表，请参阅 <b>MaxBaud</b> 成员的说明。 <b>SettableBaud</b> 设置为零或设置为按位 OR 或其中一个或多个标志位。
            ///<para>串行控制器驱动程序在 <b>SettableBaud</b> 位掩码值中设置SERIAL_BAUD_USER标志位，以指示它们支持的波特率高于其他 SERIAL_BAUD_XXX 标志位表示的波特率。 例如，支持 57600、115200、230400 和 460800 bps 波特率的驱动程序将 <b>SettableBaud</b> = (SERIAL_BAUD_57600 |SERIAL_BAUD_115200 |SERIAL_BAUD_USER) 。</para>
            /// </summary>
            public uint SettableBaud;
            /// <summary>
            /// 可以设置的数据位数。 此成员设置为零或设置为以下一个或多个标志位的按位 OR。
            /// <list type="table">
            /// <item>SERIAL_DATABITS_5	5 个数据位</item>
            /// <item>SERIAL_DATABITS_6	6 个数据位</item>
            /// <item>SERIAL_DATABITS_7	7 个数据位</item>
            /// <item>SERIAL_DATABITS_8	8 数据位</item>
            /// <item>SERIAL_DATABITS_16	16 个数据位</item>
            /// <item>SERIAL_DATABITS_16X	通过串行硬件线的特殊宽路径</item>
            /// </list>
            /// </summary>
            public ushort SettableData;
            /// <summary>
            /// 可选择的停止位和奇偶校验设置。 此成员设置为零或设置为以下一个或多个标志位的按位 OR。
            /// <list type="table">
            /// <item>SERIAL_STOPBITS_10	一个停止位。</item>
            /// <item>SERIAL_STOPBITS_15	一个半停止位。</item>
            /// <item>SERIAL_STOPBITS_20	两个停止位。</item>
            /// <item>SERIAL_PARITY_NONE	不使用奇偶校验位。</item>
            /// <item>SERIAL_PARITY_ODD	奇数奇偶校验。 如果字符值中的 1 数为偶数，则奇偶校验位为 1。 否则，奇偶校验位为 0。</item>
            /// <item>SERIAL_PARITY_EVEN	甚至奇偶校验。 如果字符值中的 1 个数为奇数，则奇偶校验位为 1。 否则，奇偶校验位为 0。</item>
            /// <item>SERIAL_PARITY_MARK	奇偶校验位始终设置为 1。</item>
            /// <item>SERIAL_PARITY_SPACE	奇偶校验位始终设置为 0。</item>
            /// </list>
            /// </summary>
            public ushort SettableStopParity;
            /// <summary>
            /// 传输队列大小。 此成员指定串行控制器驱动程序的内部输出缓冲区的大小（以字节为单位）。 值为零表示缓冲区大小不可用。
            /// <para>对于 SerCx2 和 SerCx，关联的串行控制器驱动程序通常将此成员设置为零。 Serial.sys 将此成员设置为指示输出缓冲区大小的非零值。</para>
            /// </summary>
            public uint CurrentTxQueue;
            /// <summary>
            /// 接收队列大小。 此成员指定串行控制器驱动程序的内部输入缓冲区的大小（以字节为单位）。 值为零表示缓冲区大小不可用。
            /// <para>对于 SerCx2 和 SerCx，此成员由关联的串行控制器驱动程序设置。 对于 SerCx2，驱动程序通常将此成员设置为零。 对于 SerCx，驱动程序通常将此成员设置为 SerCx 用于缓冲接收的数据的环形缓冲区的大小。 此驱动程序可以调用 SerCxGetRingBufferUtilization 方法从 SerCx 获取环形缓冲区大小。</para>
            /// <para>Serial.sys 将此成员设置为指示输入缓冲区大小的非零值。</para>
            /// </summary>
            public uint CurrentRxQueue;
            /// <summary>
            /// 提供程序特定的数据。 除非有关串行端口所需数据格式的提供程序特定数据可用，否则应用程序应忽略此成员。
            /// </summary>
            public uint ProvSpec1;
            /// <summary>
            /// 提供程序特定的数据。 除非有关串行端口所需数据格式的提供程序特定数据可用，否则应用程序应忽略此成员。
            /// </summary>
            public uint ProvSpec2;
            /// <summary>
            /// 提供程序特定的数据。 除非有关串行端口所需数据格式的提供程序特定数据可用，否则应用程序应忽略此成员。 此成员是一个或多个元素的宽字符数组中的第一个元素。 紧跟在此成员之后的任何其他元素。 <b>PacketLength</b> 成员指定<b>SERIAL_COMMPROP</b>结构以及此结构后面的任何其他 <b>ProvChar</b> 数组元素的大小。
            /// </summary>
            public IntPtr ProvChar;
        }
        /// <summary>
        /// 用于调整串行控制器驱动程序用于串行接收操作的输入缓冲区的大小。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_QUEUE_SIZE
        {
            /// <summary>
            /// 输入缓冲区请求的字节数。
            /// </summary>
            public uint InSize;
            /// <summary>
            /// 未使用。 设置为零。
            /// </summary>
            public uint OutSize;
        }

        /// <summary>
        /// 串行端口读取和写入操作的超时参数。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_TIMEOUTS
        {
            /// <summary>
            /// 读取操作中允许的两个连续字节之间的最大时间量（以毫秒为单位）。 超过此最大超时的读取操作。此最大值不适用于读取第一个字节之前的时间间隔。 值为零表示不使用间隔超时。 
            /// </summary>
            public uint ReadIntervalTimeout;
            /// <summary>
            /// 读取操作中每个字节允许的最大时间量（以毫秒为单位）。 超过此最大超时的读取操作。
            /// </summary>
            public uint ReadTotalTimeoutMultiplier;
            /// <summary>
            /// 每个读取操作允许的最大额外时间量（以毫秒为单位）。 超过此最大超时的读取操作。
            /// </summary>
            public uint ReadTotalTimeoutConstant;
            /// <summary>
            /// 写入操作中每个字节允许的最大总时间（以毫秒为单位）。 超过此最大超时的写入操作。
            /// </summary>
            public uint WriteTotalTimeoutMultiplier;
            /// <summary>
            /// 每个写入操作允许的最大额外时间量（以毫秒为单位）。 超过此最大超时的写入操作。
            /// </summary>
            public uint WriteTotalTimeoutConstant;
        }
        /// <summary>
        /// 串行端口当前配置为发送和接收数据的波特率。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_BAUD_RATE
        {
            /// <summary>
            /// 波特率。 此参数指定串行端口当前配置为传输或接收的每秒位数。 例如， BaudRate 值为 115200 表示端口配置为每秒传输 115，200 位。
            /// </summary>
            public uint BaudRate;
        }
        /// <summary>
        /// 串行行的控件设置。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_LINE_CONTROL
        {
            /// <summary>
            /// 在传输或接收的每个字符末尾使用的停止位数。 此成员设置为以下值之一。
            /// <list type="table">
            /// <item>STOP_BIT_1	一个停止位。</item>
            /// <item>STOP_BITS_1_5	一个半停止位。</item>
            /// <item>STOP_BITS_2	两个停止位。 如果 WordLength = 5，则无效。</item>
            /// </list>
            /// </summary>
            public byte StopBits;
            /// <summary>
            /// 用于传输或接收的每个字符的奇偶校验检查的类型。 此成员设置为以下值之一。
            /// <list type="table">
            /// <item>NO_PARITY	不使用奇偶校验位。</item>
            /// <item>ODD_PARITY	使用奇数奇偶校验。 如果字符值中的 1 个数为奇数，则奇偶校验位为 0。 否则，奇偶校验位为 1。</item>
            /// <item>EVEN_PARITY	甚至使用奇偶校验。 如果字符值中的 1 数为偶数，则奇偶校验位为 0。 否则，奇偶校验位为 1。</item>
            /// <item>MARK_PARITY	奇偶校验位始终设置为 1。</item>
            /// <item>SPACE_PARITY	奇偶校验位始终设置为 0。</item>
            /// </list>
            /// </summary>
            public byte Parity;
            /// <summary>
            /// 每个字符的数据位数。 此成员指示传输或接收的每个字符值中的数据位数，不包括奇偶校验位或停止位。 通常支持 5 到 8 范围内的 <b>WordLength</b> 值。
            /// </summary>
            public byte WordLength;
        }

        /// <summary>
        /// 串行控制器驱动程序用于握手流控制的特殊字符。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_CHARS
        {
            /// <summary>
            /// EOF (文件) 字符的结尾。 接收此字符将标记输入流的末尾。
            /// </summary>
            public byte EofChar;
            /// <summary>
            /// 奇偶校验错误替换字符。 收到出现奇偶校验错误的字节将替换为此字符。
            /// </summary>
            public byte ErrorChar;
            /// <summary>
            /// 断字符。 收到此字符表示输入流中出现中断 (临时暂停) 。
            /// </summary>
            public byte BreakChar;
            /// <summary>
            /// 事件字符。 如果当前等待掩码中设置了SERIAL_EV_RXFLAG标志位，则接收此字符表示串行通信事件。 等待掩码由 IOCTL_SERIAL_SET_WAIT_MASK 请求设置。 IOCTL_SERIAL_WAIT_ON_MASK请求启动等待掩码中的事件的等待。
            /// </summary>
            public byte EventChar;
            /// <summary>
            /// 用于传输和接收操作的 XON（传输打开）字符。
            /// </summary>
            public byte XonChar;
            /// <summary>
            /// 用于传输和接收操作的 XOFF（传输关闭）字符。
            /// </summary>
            public byte XoffChar;
        }
        /// <summary>
        /// 串行端口的握手和流控制设置。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class SERIAL_HANDFLOW
        {
            /// <summary>
            /// 一个位掩码，指定串行端口用于流控制的控制线。 此成员设置为零或设置为按位 OR 或以下一个或多个标志。
            /// <list type="table">
            /// <item>SERIAL_DTR_CONTROL	启用 DTR (数据终端就绪) 。</item>
            /// <item>SERIAL_DTR_HANDSHAKE	DTR 用于输入流控制。</item>
            /// <item>SERIAL_CTS_HANDSHAKE	CTS (清除以发送) 用于输出流控制。</item>
            /// <item>SERIAL_DSR_HANDSHAKE	DSR (数据集就绪) 用于输出流控制。</item>
            /// <item>SERIAL_DCD_HANDSHAKE	DCD (数据载体检测) 用于输出流控制。</item>
            /// <item>SERIAL_DSR_SENSITIVITY	当 DSR 行处于非活动状态时，忽略到达串行端口的字符。</item>
            /// <item>SERIAL_ERROR_ABORT	如果发生错误，请中止传输或接收操作。</item>
            /// </list>
            /// SerCx2 支持 SERIAL_CTS_HANDSHAKE 标志，并且可能支持或不支持为此成员定义的其他六个标志，具体取决于串行控制器驱动程序和串行控制器硬件的功能。 SerCx 仅支持上表中的前四个标志。 Serial.sys 支持上表中的所有标志。
            /// </summary>
            public uint ControlHandShake;
            /// <summary>
            /// 指定流控制行为的位掩码。 此成员设置为零或设置为按位 OR 或以下一个或多个标志。
            /// <list type="table">
            /// <item>SERIAL_AUTO_TRANSMIT	使用 XON/XOFF 对传输的数据进行流控制。</item>
            /// <item>SERIAL_AUTO_RECEIVE	使用 XON/XOFF 对接收的数据进行流控制。</item>
            /// <item>SERIAL_ERROR_CHAR	发生错误时，将错误字符插入到接收的数据中。 有关详细信息，请参阅 SERIAL_CHARS 中 ErrorChar 成员的说明。</item>
            /// <item>SERIAL_NULL_STRIPPING	自动从接收的数据中去除 null 字符。</item>
            /// <item>SERIAL_BREAK_CHAR	发生中断时，将分隔符插入到接收的数据中。 有关详细信息，请参阅 SERIAL_CHARS 中 BreakChar 成员的说明。</item>
            /// <item>SERIAL_RTS_CONTROL	已启用发送 的 RTS 请求。</item>
            /// <item>SERIAL_RTS_HANDSHAKE	RTS 用于输出流控制。</item>
            /// <item>SERIAL_XOFF_CONTINUE	将 XOFF 发送到线路另一端的设备后继续传输。</item>
            /// </list>
            /// SerCx2 支持SERIAL_RTS_CONTROL和SERIAL_RTS_HANDSHAKE标志，但通常不支持为此成员定义的其他六个标志，具体取决于串行控制器驱动程序和串行控制器硬件的功能。 SerCx 仅支持SERIAL_RTS_CONTROL和SERIAL_RTS_HANDSHAKE标志。 Serial.sys 支持上表中的所有标志。
            /// </summary>
            public uint FlowReplace;
            /// <summary>
            /// XON 限制。 当内部接收缓冲区中的字符数低于 XON 限制时，串行控制器驱动程序使用流控制信号来告知发送方继续发送字符。
            /// </summary>
            public int XonLimit;
            /// <summary>
            /// XOFF 限制。 当内部接收缓冲区中的字符数达到 XOFF 限制时，串行控制器驱动程序使用流控制信号来告知发送方停止发送字符。
            /// </summary>
            public int XoffLimit;
        }
    }
}

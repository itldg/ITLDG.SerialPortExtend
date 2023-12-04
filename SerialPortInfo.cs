using System;
using System.Collections.Generic;
using System.Text;

namespace ITLDG.SerialPortExtend
{

    public class SerialPortInfo
    {
        private const string VIRTUAL_TAG = "virtual";
        private const string USB_TAG = "usb";
        private static string[] BLE_STRING = {
            "蓝牙",
            "ble",
            "bluetooth low energy" ,
            "bluetooth smart",
            "bluetooth le"
        };
        /// <summary>
        /// 端口号
        /// </summary>
        public string COM { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public SerialPortType Type
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return SerialPortType.Unknown;

                string name = Name.ToLower().Trim();
                if (name.Contains(VIRTUAL_TAG))
                    return SerialPortType.Virtual;
                if (name.Contains(USB_TAG))
                    return SerialPortType.USBSerial;
                foreach (var item in BLE_STRING)
                {
                    if (name.Contains(item))
                    {
                        return SerialPortType.Ble;
                    }
                }
                return SerialPortType.Unknown;
            }
        }

    }
    /// <summary>
    /// 串口类型
    /// </summary>
    public enum SerialPortType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 虚拟串口
        /// </summary>
        Virtual,
        /// <summary>
        /// USB设备
        /// </summary>
        USBSerial,
        /// <summary>
        /// 蓝牙串口
        /// </summary>
        Ble
    }
}

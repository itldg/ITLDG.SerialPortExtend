# ITLDG.SerialPortExtend

SerialPort 的增强扩展

## 增加方法

| 方法           | 说明                                                                                 |
| -------------- | ------------------------------------------------------------------------------------ |
| GetProperties  | 获取串口属性                                                                         |
| SetQueueSize   | 设置内部接收缓冲区的大小                                                             |
| GetTimeouts    | 返回串行控制器驱动程序用于读取和写入请求的超时值                                     |
| SetTimeouts    | 设置串行控制器驱动程序用于读取和写入请求的超时值                                     |
| Purge          | 请求取消指定的请求，并从指定的缓冲区中删除数据                                       |
| GetLineControl | 请求返回有关串行设备的线路控制集的信息。线路控制参数包括停止位数、数据位数和奇偶校验 |
| SetLineControl | 请求设置线路控制寄存器 （LCR）。线路控制寄存器控制数据大小、停止位数和奇偶校验       |
| GetChars       | 请求检索串行控制器驱动程序用于握手流控制的特殊字符                                   |
| SetChars       | 请求设置串行控制器驱动程序用于握手流控制的特殊字符                                   |
| GetHandFlow    | 请求返回有关为串行设备设置的握手流控制设置的信息                                     |
| SetHandFlow    | 请求设置握手流控制设置                                                               |

## 静态方法

| 方法                                | 说明                               |
| ----------------------------------- | ---------------------------------- |
| SerialPortExtensions.GetPortNames() | 获取串口列表(端口号,名称,串口类型) |

## 使用示例

```csharp
using ITLDG.SerialPortExtend;


SerialPort sp = new SerialPort();
sp.PortName = "COM1";
sp.Open();

sp.SetChars(new NativeStructs.SERIAL_CHARS() { EofChar = 0x01, ErrorChar = 0x02, BreakChar = 0x03, EventChar = 0x04, XonChar = 0x05, XoffChar = 0x06, });
sp.SetHandFlow(new NativeStructs.SERIAL_HANDFLOW() { ControlHandShake = 0x01, FlowReplace = 0x40, XonLimit = 0x00, XoffLimit = 0x300 });
sp.SetLineControl(new NativeStructs.SERIAL_LINE_CONTROL() { StopBits = 0x01, Parity = 0x01, WordLength = 0x08 });
sp.SetQueueSize(new NativeStructs.SERIAL_QUEUE_SIZE() { InSize = 0x1000, OutSize = 0x1000 });
sp.SetTimeouts(new NativeStructs.SERIAL_TIMEOUTS() { ReadIntervalTimeout = 0x100, ReadTotalTimeoutConstant = 0x100, ReadTotalTimeoutMultiplier = 0x100, WriteTotalTimeoutConstant = 0x100, WriteTotalTimeoutMultiplier = 0x100 });
var properties = sp.GetProperties();
var chars = sp.GetChars();
var handFlow = sp.GetHandFlow();
var lineControl = sp.GetLineControl();
var timeOuts = sp.GetTimeouts();
sp.Close();
```

## 更多方法

更多的方法和参数等待大家 `Pr`

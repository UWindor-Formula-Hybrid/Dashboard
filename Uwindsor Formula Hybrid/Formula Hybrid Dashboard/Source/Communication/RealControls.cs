using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    /// <summary>
    /// This class contains the real controls for the car. This class uses the Input Interface(<see cref="InputInterface"/>)<br></br>
    /// to commnunicate with the various sensors on the car, it communicates over serial when gathering data, and outputs a single pwm current.<br/>
    /// This class uses the serial chart from It seems that it uses CANBUS, which means that this class will contain an implementation of the CANBUS protocal.
    /// </summary>
    class RealControls : InputInterface
    {

        //SerialDevices for reading data
        private static SerialDevice CANSerial;
        private static SerialDevice USBSerial;

        //Internal Versions Of Public Variables
        private static decimal Internal_EngineAirIntakeTemp = 21;
        private static decimal Internal_EngineCoolantTemp = 21;
        private static decimal Internal_EngineRPM = 0;
        private static decimal Internal_FuelLevel = 50;
        private static decimal Internal_MotorBatteryLevel = 50;
        private static decimal Internal_UtilBatteryVoltage = 0;
        private static decimal Internal_Throttle_Engine = 0;
        private static decimal Internal_Throttle_Motor = 0;
        private static decimal Internal_VehicleSpeed = 0;



        public decimal EngineAirIntakeTemp
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_EngineAirIntakeTemp;
                }
            }
        }

        public decimal EngineCoolantTemp
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_EngineCoolantTemp;
                }
            }
        }

        public decimal EngineRPM
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_EngineRPM;
                }
            }
        }

        public decimal FuelLevel
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_FuelLevel;
                }
            }
        }

        public decimal MotorBatteryLevel
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_MotorBatteryLevel;
                }
            }
        }

        public decimal Throttle_Engine
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_Throttle_Engine;
                }
            }
        }

        public decimal Throttle_Motor
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_Throttle_Motor;
                }
            }
        }

        public decimal UtilBatteryVoltage
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_UtilBatteryVoltage;
                }
            }
        }

        public decimal VehicleSpeed
        {
            get
            {
                lock ("ComLock")
                {
                    return Internal_VehicleSpeed;
                }
            }
        }

        private async void USBSerialListen(SerialDevice serialPort)
        {
            var reader = new StreamReader(serialPort.InputStream.AsStreamForRead());
            while (true)
            {
                String line = "";
                try
                {
                    line = reader.ReadLine();
                    string[] split = Regex.Split(line, @"(?<!\\),");
                    
                    if (split.First() == "DB" && split.Count() == 10)
                    {
                        lock ("ComLock")
                        {
                            Internal_EngineRPM = Decimal.Parse(split[1]);
                        }
                    }
                }
                catch
                {
                    // :(
                }

                
            }
        }
        public async void Initialize()
        {
            CANSerial = await SerialDevice.FromIdAsync(@"\\?\FTDIBUS#VID_0403+PID_6001+AI028SHYA#0000#{86e0d1e0-8089-11d0-9ce4-08003e301f73}");
            USBSerial = await SerialDevice.FromIdAsync(@"\\?\USB#VID_2341&PID_0010#7543134323435160E0C2#{86e0d1e0-8089-11d0-9ce4-08003e301f73}");

            //Configure CANSerial
            CANSerial.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            CANSerial.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            CANSerial.BaudRate = 57600;
            CANSerial.Parity = SerialParity.None;
            CANSerial.StopBits = SerialStopBitCount.One;
            CANSerial.DataBits = 8;

            //Configure USBSerial
            USBSerial.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            USBSerial.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            USBSerial.BaudRate = 115200;
            USBSerial.Parity = SerialParity.None;
            USBSerial.StopBits = SerialStopBitCount.One;
            USBSerial.DataBits = 8;

            //Announce that we are ready to receive on USB
            var Writer = new DataWriter(USBSerial.OutputStream);
            Writer.WriteString("OK");
            Task<UInt32> StoreTask = Writer.StoreAsync().AsTask();
            Writer.DetachStream();
            Writer = null;

            
        }
    }
}

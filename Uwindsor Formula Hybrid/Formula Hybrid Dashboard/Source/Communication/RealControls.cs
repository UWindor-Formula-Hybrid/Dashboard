using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    /// <summary>
    /// This class contains the real controls for the car. This class uses the Input Interface(<see cref="InputInterface"/>)<br></br>
    /// to commnunicate with the various sensors on the car, it communicates over serial when gathering data, and outputs a single pwm current.<br/>
    /// This class uses the serial chart from It seems that it uses CANBUS, which means that this class will contain an implementation of the CANBUS protocal.
    /// </summary>
    class RealControls : InputInterface
    {
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
        
        public void Initialize()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uwindsor_Formula_Hybrid.Source.Communication;

namespace Formula_Hybrid_Dashboard.Source.Communication
{
    class TestingControls : InputInterface
    {
        public static float BatteryPercent = 0;
        public static float CurSpeed = 0;
        public static int CurRPM = 0;
        public static float fuel = 0;
        public static float throttle = 0;

        public float BatteryLeft
        {
            get
            {
                lock ("CommunicationLock")
                {
                    return BatteryPercent;
                }
            }
            set
            {
                lock ("CommunicationLock")
                {
                    BatteryPercent = value;
                }
            }
        }

        public float CurrentSpeed
        {
            get
            {
                lock ("CommunicationLock")
                {
                    return CurSpeed;
                }
            }
            set
            {
                lock ("CommunicationLock")
                {
                    CurSpeed = value;
                }
            }
        }

        public int EngineRPM
        {
            get
            {
                lock ("CommunicationLock")
                {
                    return CurRPM;
                }
            }
            set
            {
                lock ("CommunicationLock")
                {
                    CurRPM = value;
                }
            }
        }

        public float FuelLeft
        {
            get
            {
                lock ("CommunicationLock")
                {
                    return fuel;
                }
            }

            set
            {
                lock ("CommunicationLock")
                {
                    fuel = value;
                }
            }
        }

        public float ThrottlePos
        {
            get
            {
                lock ("CommunicationLock")
                {
                    return throttle;
                }
            }

            set
            {
                lock ("CommunicationLock")
                {
                    throttle = value;
                }
            }
        }

        public void Initialize()
        {

        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}

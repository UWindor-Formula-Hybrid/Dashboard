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
    
        //This is used to signal a application wide shutdown. 
        public bool isShuttingDown = false;

        public float ThrottlePos
        {
            get
            {
                
                throw new NotImplementedException();
            }
        }

        public float CurrentSpeed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float FuelLeft
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float BatteryLeft
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int EngineRPM
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        float InputInterface.ThrottlePos
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        float InputInterface.CurrentSpeed
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        float InputInterface.FuelLeft
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        float InputInterface.BatteryLeft
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This thread is asynchronously run leaving it inside of its own thread.
        /// There is no clean shutdown as of yet.
        /// </summary>
        private async void ControlLoop()
        {
            while (!isShuttingDown)
            {

            }
        }
        /// <summary>
        /// Marks the loop for shutdown and finishing anything needed to be shutdown before returning.
        /// </summary>
        public void Shutdown()
        {
            isShuttingDown = true;
        }

        /// <summary>
        /// This method initializes the class, starting the thread and returning.
        /// </summary>
        public async void Initialize()
        {
            var x = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            DeviceInformation deviceraw = x.First();
            SerialDevice device = await SerialDevice.FromIdAsync(deviceraw.Id);

        }
    }
}

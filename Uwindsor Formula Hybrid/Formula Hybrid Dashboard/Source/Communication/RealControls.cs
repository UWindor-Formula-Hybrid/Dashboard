using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    /// <summary>
    /// This class contains the real controls for the car. This class uses the Input Interface(<see cref="InputInterface"/>)<br></br>
    /// to commnunicate with the various sensors on the car, it communicates over serial when gathering data, and outputs a single pwm current.<br/>
    /// This class uses the serial chart from It seems that it uses CANBUS, which means that this class will contain an implementation of the CANBUS protocal.
    /// </summary>
    class RealControls : InputInterface
    {
        /// <summary>
        /// The Labels to the CanbusData, Only really useful in debugging, configuration, and testing
        /// </summary>
        private string[] CanbusDataLabels = {"RPM","Throttle","Ignition"}; 
        /// <summary>
        /// This is where the data from the ecu is stored as it is received, <br/>
        /// it is like this because the communication must be done on a loop in its own thread as to maintain proper timing.
        /// </summary>
        private int[] CanbusData;
        //This is used to signal a application wide shutdown. 
        public bool isShuttingDown = false;
        /// <summary>
        /// This method initializes the class, starting the thread and returning.
        /// </summary>
        RealControls()
        {

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
        public void InitSerialCom(int bitRate)
        {
            throw new NotImplementedException();
        }

        public void SetMotorOuput(float value)
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public int getSpeedSensor()
        {
            throw new NotImplementedException();
        }
    }
}

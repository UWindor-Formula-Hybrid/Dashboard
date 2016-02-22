using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    /// <summary>This is the inteface used by any method of Communication. 
    /// The method that is used will not be known to any class but the one in App.xaml.cs
    /// <seealso cref="App.xaml.cs"></summary>
    interface InputInterface
    {
        /// <summary>This method sets the motor output value, this will then get output to the motor.</summary>
        /// <param name="value">The value sent to the motor, an integer from 0 - 255</param>
        void SetMotorOuput(float value);

        /// <summary>This method initializes serial communication with the ECU.</summary>
        /// <param name="bitRate"> The bitrate used when talking to the ECU, an integer value.</param>
        void InitSerialCom(int bitRate);

        /// <summary>
        /// This variable is used to signal an application wide shutdown. <br/>
        /// It allows the threads to shudown gracefully.
        /// </summary>
        bool isShuttingDown;
        
        

    }
}

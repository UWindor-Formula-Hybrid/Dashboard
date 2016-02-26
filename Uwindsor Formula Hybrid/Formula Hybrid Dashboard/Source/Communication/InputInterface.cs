using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    /// <summary>This is the inteface used by any method of Communication. 
    /// The method that is used will not be known to any class but the one in App.xaml.cs
    /// <seealso cref="App.xaml.cs"></summary>
    interface InputInterface
    {
        //Pretty much all communication is locked and is just a variable, updated by the interface
        float ThrottlePos { get; set; } // a Value from 0 - 100
        float CurrentSpeed { get; set; }  // in KPH
        float FuelLeft { get; set; }  // a Value from 0 - 100
        float BatteryLeft { get; set; }  // a Value from 0 - 100
        int EngineRPM { get; }

        /// <summary>
        /// Begin all theading and stores the mutex for use by any private methods
        /// </summary>
        /// <param name="mutex">Used for communication between threads and allows any thread to use communication without worrying about being threadsafe</param>
        void Initialize();


        /// <summary>
        /// This method is used to signal an application wide shutdown. <br/>
        /// It allows the thread to shudown gracefully.
        /// </summary>
        void Shutdown();
        
        

    }
}

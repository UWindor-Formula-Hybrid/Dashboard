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
        Decimal EngineRPM            { get; } // The number of RPM in the Engine, as provided by the ecu and relayed by the arduino
        Decimal VehicleSpeed         { get; } // The current speed in KPH as calculated by the arduino.
        Decimal Throttle_Engine      { get; } // The current engine throttle position, a value from 0 to 100.
        Decimal Throttle_Motor       { get; } // the current motor throttle position, a value from 0 to 100.
        Decimal FuelLevel            { get; } // The current fuel tank fill level, as a percentage of full.
        Decimal MotorBatteryLevel    { get; } // The Current Motor battery charge level, as a percentage of full.
        Decimal UtilBatteryVoltage   { get; } // The Current electronics battery charge level, as a voltage.
        Decimal EngineCoolantTemp    { get; } // The Current engine coolant temperature, in degrees celcius.
        Decimal EngineAirIntakeTemp  { get; } // The Current intake air temperature, in degrees celcius.
        
        void Initialize();  // used to start anything related to communication.


        
        
        

    }
}

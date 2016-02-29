using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiSharp.LibGpio
using PiSharp.LibGpio.Entities

namespace Formula_Hybrid_Dashboard.Source.Communication
{
    class Config
    {
        public void Main(string[] args)
        {
            // Use test mode
            LibGpio.Gpio.TestMode = true;

            // Setup pin 11,13 for input (using Physical Board Pin numbers)
            LibGpio.Gpio.SetupChannel(PhysicalPinNumber.Eleven, Direction.Input);
            LibGpio.Gpio.SetupChannel(PhysicalPinNumber.Thirteen, Direction.Input);

            // Loop around and set the value of GPIO 0 to the inverse of GPIO 7
            // ie. If the button is pressed, the LED will go out
            var oldInput = true;
            while (true)
            {
                var newInput = LibGpio.Gpio.ReadValue(RaspberryPinNumber.Seven);
                if (newInput != oldInput)
                {
                    LibGpio.Gpio.OutputValue(BroadcomPinNumber.Seventeen, !newInput);
                    oldInput = newInput;
                }
            }
        }
    }
}

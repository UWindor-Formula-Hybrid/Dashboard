using Formula_Hybrid_Dashboard.Source.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    class Communication
    {        
        public static InputInterface DefaultControls = getControls();
        public static bool isTesting = false;

        /// <summary>
        /// function to initialize the controls object.
        /// </summary>
        /// <returns>an initialized control object</returns>
        static InputInterface getControls()
        {
            var x = new RealControls(); // make our controls
            x.Initialize(); // initialize our controls
            return x;
        }
    }
}

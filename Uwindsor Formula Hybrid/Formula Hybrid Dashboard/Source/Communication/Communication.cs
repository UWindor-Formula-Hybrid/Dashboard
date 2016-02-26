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

#if DEBUG
        private static InputInterface input = new TestingControls();
        public static bool isTesting = true;
#else
        private static InputInterface input = new RealControls();
        public static bool isTesting = false;
#endif


        /// <summary>
        /// This returns the currently used interface to the class asking.
        /// </summary>
        /// <returns>The Default Communication Interface</returns>
        static InputInterface getInputInterface()
        {
            return input;
        }
        
    }
}

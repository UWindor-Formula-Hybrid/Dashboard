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
        public static InputInterface DefaultControls = new TestingControls();
        public static bool isTesting = true;
#else
        public static InputInterface DefaultControls = new RealControls();
        public static bool isTesting = false;
#endif
    }
}

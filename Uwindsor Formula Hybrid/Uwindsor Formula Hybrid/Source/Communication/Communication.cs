using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwindsor_Formula_Hybrid.Source.Communication
{
    class Communication
    {
        private InputInterface input;
        Communication()
        {
#if DEBUG
            input = new Testing.TestingInputs();
#else
       //     input = new RealControls();
#endif

        }
        async void CommunicationWorker()
        {
            
        }
        
    }
}

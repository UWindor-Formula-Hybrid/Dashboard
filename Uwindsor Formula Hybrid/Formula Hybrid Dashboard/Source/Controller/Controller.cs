using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Formula_Hybrid_Dashboard.Source.Controller
{
    class Controller
    {
        private static int throttle = 0;
        /// <summary>
        /// this is thread safe
        /// </summary>
        /// <returns>returns the motor position</returns>
        int getThrottle()
        {
            lock ("ControllerLock")
            {
                return throttle;
            }
        }
        /// <summary>
        /// Only Runs Once
        /// </summary>
        void InitController()
        {
            DispatcherTimer dispatcherTimer;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Controller_Loop;
            dispatcherTimer.Interval = new TimeSpan(2);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// This runs every 2ns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Controller_Loop(object sender, object e)
        {

        }
    }
}

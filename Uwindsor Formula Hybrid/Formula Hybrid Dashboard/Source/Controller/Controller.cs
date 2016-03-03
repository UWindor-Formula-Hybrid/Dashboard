using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Formula_Hybrid_Dashboard.Source.Controller
{
    class Controller
    {
        ///Configurable Variables
        private static int LoopTicks = 20000; //2ms or 500 times a second
        private static int ThrottleHistoryLimit = 10000000 / LoopTicks; // one second worth of history
        private static int MotorSpeedCutoff = 60; // Do not use the motor past 60km/h
        private static int PercentChangeForAccel = 5; // how much does the throttle have to be pressed for the motor to kick in, value in percent
        private static int RPMHistoryLimit = 5000000 / LoopTicks; // half a second worth of history
        private static int RPMDiffThreshold = 100; // if it changes less than this in half a second we will stop accelerating
        private static int AccelerationLimit = 50; // how much can we accelerate in half a second(how many percent can the throttle increase)
        private static int AccelerationStopDelay = (5000000 / LoopTicks) / 2; // wait a quarter of a second after beggining to accelerate to look at whether or not you should stop

        /// <summary>
        /// A difference of 5% total in the throttle will cause the momtor to kick in to aid in acceleration
        /// </summary>
        private static Uwindsor_Formula_Hybrid.Source.Communication.InputInterface comm = Uwindsor_Formula_Hybrid.Source.Communication.Communication.DefaultControls;
        private static int throttle = 0; // a value between 0 and 255
        private static List<double> ThrottleHist = new List<double> { comm.ThrottlePos };
        private static List<int> RPMHist = new List<int> { comm.EngineRPM };
        private static double ThrottleAverage = ThrottleHist.Average();
        private static double RPMAverage = RPMHist.Average();
        private static float InternalSpeed = comm.CurrentSpeed;
        private static bool isAccel = false; // Whether or not we are using the motor right now.
        private static int WaitCount = 0; // the counting for the stop check



       

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
            dispatcherTimer.Interval = new TimeSpan(LoopTicks);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// This runs every so often
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Controller_Loop(object sender, object e)
        {
            //Update variables
            ThrottleHist.Add(comm.ThrottlePos);
            RPMHist.Add(comm.EngineRPM);
            InternalSpeed = comm.CurrentSpeed;

            //Keep List Lengths Within Thresholds
            if(ThrottleHist.Count > ThrottleHistoryLimit)
            {
                ThrottleHist.RemoveAt(0);
            }
            if(RPMHist.Count > RPMHistoryLimit)
            {
                RPMHist.RemoveAt(0);
            }
            //Update List Averages 
            ThrottleAverage = ThrottleHist.Average();
            RPMAverage = RPMHist.Average();

            //check speed cutoff
            if (InternalSpeed >= MotorSpeedCutoff)
            {
                lock ("ControllerLock")
                {
                    throttle = 0; // when we are going to fast, do not engage the motor
                }
            }
            else
            {
                //We arn't going too fast
                if (isAccel)// are we accelerating right now?
                {
                    //we are accelerating
                    if(RPMHist.Last() - RPMAverage > RPMDiffThreshold && WaitCount >= AccelerationStopDelay) //Should we be?
                    {
                        //Yes, we should be
                        lock ("ControllerLock")
                        {
                            
                        }
                    }
                    else if(WaitCount < AccelerationStopDelay) // are we still counting?
                    {
                        WaitCount += 1; // lets count

                    }
                    else 
                    {
                        //We should not be accelerating
                    }
                }
            }

        }
    }
}

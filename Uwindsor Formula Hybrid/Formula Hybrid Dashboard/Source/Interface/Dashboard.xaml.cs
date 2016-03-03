using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Uwindsor_Formula_Hybrid.Source.Communication;
using Formula_Hybrid_Dashboard.Source.Controller;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Uwindsor_Formula_Hybrid.Source.Interface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Dashboard : Page
    {
        
        public Dashboard()
        {
            this.InitializeComponent();
        }
        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        private int getRPM()
        {
            return Communication.Communication.DefaultControls.EngineRPM;
        }

        private float getSpeed()
        {
            return Communication.Communication.DefaultControls.CurrentSpeed;
        }

        private double getRPMStroke()
        {
            return ((getRPM() + 0.0) / 10000.0) * 13.5;
        }

        private double getSpeedStroke()
        {
            return ((getSpeed() + 0.0) / 160.0) * 22.9;
        }

        private float getFuelLeft()
        {
            return Communication.Communication.DefaultControls.FuelLeft;
        }

        private double getFuelLeftStroke()
        {
            return ((getFuelLeft() + 0.0) / 100.0) * 25.75;
        }

        private float getBatteryLeft()
        {
            return Communication.Communication.DefaultControls.BatteryLeft;
        }

        private double getBatteryLeftStroke()
        {
            return ((getBatteryLeft() + 0.0) / 100.0) * 28.25;
        }
        
        private float getEngThrottlePos()
        {
            return Communication.Communication.DefaultControls.ThrottlePos;
        }
        /// <summary>
        /// This is where the code runs that will update the UI, it runs repeatably
        /// </summary>
        public void UI_TimerTick(object sender, object e)
        {
            RPMlabel.Text = "" + getRPM() + " RPM";
            SpeedLabel.Text = "" + getSpeed() + " km/h";
            RPM.StrokeDashArray = new DoubleCollection() { getRPMStroke() , 100};// RPM divded by RPM limit times max stroke length
            Speed.StrokeDashArray = new DoubleCollection() { getSpeedStroke(), 100 };
            FuelE.StrokeDashArray = new DoubleCollection() { getFuelLeftStroke(), 100 };
            BattE.StrokeDashArray = new DoubleCollection() { getBatteryLeftStroke(), 100 };
            ((ProgressBar)FindChildControl<ProgressBar>(InfoCenter, "Ethrottle")).Value = getEngThrottlePos();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //This opens the testing window if we are suposed to do that for testing
#if DEBUG
            CoreApplicationView newCoreView = CoreApplication.CreateNewView();
            ApplicationView newAppView = null;
            int mainViewId = ApplicationView.GetApplicationViewIdForWindow(
              CoreApplication.MainView.CoreWindow);

            await newCoreView.Dispatcher.RunAsync(
              CoreDispatcherPriority.Normal,
              () =>
              {
                  newAppView = ApplicationView.GetForCurrentView();
                  Window.Current.Content = new Formula_Hybrid_Dashboard.Source.Communication.TestingWindow();
                  Window.Current.Activate();
              });

            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
              newAppView.Id,
              ViewSizePreference.UseHalf,
              mainViewId,
              ViewSizePreference.UseHalf);
#endif
            DispatcherTimer dispatcherTimer;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += UI_TimerTick;
            dispatcherTimer.Interval = new TimeSpan(200000); //2ms or 500 times a second
            dispatcherTimer.Start();
        }
    }
}

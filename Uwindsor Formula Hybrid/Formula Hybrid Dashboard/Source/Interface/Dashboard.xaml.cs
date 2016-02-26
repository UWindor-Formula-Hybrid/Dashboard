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

        /// <summary>
        /// This is where the code runs that will update the UI, it runs repeatably
        /// </summary>
        public void UI_TimerTick(object sender, object e)
        {
            RPMlabel.Text = "" + Communication.Communication.DefaultControls.EngineRPM + " RPM";
            SpeedLabel.Text = "" + Communication.Communication.DefaultControls.CurrentSpeed + " Km/h";
            RPM.StrokeDashArray = new DoubleCollection() { ((Communication.Communication.DefaultControls.EngineRPM + 0.0) / 10000.0) * 13.5 , 100};// RPM divded by RPM limit times max stroke length
            Speed.StrokeDashArray = new DoubleCollection() { ((Communication.Communication.DefaultControls.CurrentSpeed + 0.0) / 160.0) * 22.9, 100 };
            FuelE.StrokeDashArray = new DoubleCollection() { ((Communication.Communication.DefaultControls.FuelLeft + 0.0) / 100.0) * 25.75, 100 };
            BattE.StrokeDashArray = new DoubleCollection() { ((Communication.Communication.DefaultControls.BatteryLeft + 0.0) / 100.0) * 28.25, 100 };
            ((ProgressBar)FindChildControl<ProgressBar>(InfoCenter, "Ethrottle")).Value = Communication.Communication.DefaultControls.ThrottlePos;
            ((ProgressBar)FindChildControl<ProgressBar>(InfoCenter, "Ethrottle")).Value = Controller.
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
            dispatcherTimer.Interval = new TimeSpan(20);
            dispatcherTimer.Start();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dashboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        //testing purposes
        Double[] maxval = { 23, 13.5, 25.75, 28.25 };
        Double[] RoundMax = { 100, 8000, 100, 100 };
        Double[] RoundValues = { 0, 0, 0, 0 };
        Double[] dx =  { 0, 0, 0, 0 };

        private async void MicrocontrollerBackground(MainPage parent)
        {
            
            while (true)
            {
                //Get Values
                parent.getValues();
                parent.setContent();
                parent.setBars();
                
                //Set Bars
                
                parent.setContent();
                await Task.Delay(TimeSpan.FromMilliseconds(50));
               // Debug.WriteLine("" + dx[0]);
            }
            

        }
        private void getValues()
        {
            for (int i = 0; i < 4; i++)
            {
                RoundValues[i] += dx[i];
                if (RoundValues[i] >= (RoundMax[i] / 2.0))
                {
                    dx[i] = dx[i] - 1;
                }
                else
                {
                    dx[i] = dx[i] + 1;
                }
                RoundValues[i] = RoundValues[i] + dx[i];
            }
           // Debug.WriteLine("" + RoundValues[0] + " " + RoundMax[0] / 2.0);
            
        }



        private void setContent()
        {

        }

        private void setBars()
        {
            Speed.StrokeDashArray = new DoubleCollection { (RoundValues[0] / RoundMax[0]) * maxval[0], 1000 };
            RPM.StrokeDashArray = new DoubleCollection { (RoundValues[1] / RoundMax[1]) * maxval[1], 1000 };
            FuelE.StrokeDashArray = new DoubleCollection { (RoundValues[2] / RoundMax[2]) * maxval[2], 1000 };
            BattE.StrokeDashArray = new DoubleCollection { (RoundValues[3] / RoundMax[3]) * maxval[3], 1000 };
            SpeedLabel.Text = "" + RoundValues[0] + " Km/h";
            RPMlabel.Text = "" + RoundValues[1] + " RPM";
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

           
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
           Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MicrocontrollerBackground(this));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uwindsor_Formula_Hybrid.Source.Communication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Formula_Hybrid_Dashboard.Source.Communication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestingWindow : Page
    {
        TestingControls controls = new TestingControls();
        public TestingWindow()
        {
            this.InitializeComponent();
        }

        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            controls.CurrentSpeed = (float)e.NewValue;
        }

        private void slider1_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            controls.EngineRPM = (int)e.NewValue;
        }

        private void slider2_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            controls.ThrottlePos = (float)e.NewValue;
        }

        private void slider3_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            controls.BatteryLeft = (float)e.NewValue;
        }

        private void slider4_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            controls.FuelLeft = (float)e.NewValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DocApp.Presentation.Views.Templates
{
    public class ButtonClickArgs:EventArgs
    {
        public Models.HospitalInDoctorDetails model { get; set; }
    }

    public sealed partial class HospitalsInDoctorsTemplate : UserControl
    {
        public delegate void ButtonClickedEventHandler(object source, ButtonClickArgs args);
        public ButtonClickedEventHandler ButtonClicked;

        Models.HospitalInDoctorDetails hospital { get { return this.DataContext as Models.HospitalInDoctorDetails; } }
        public HospitalsInDoctorsTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        public void onButtonClicked()
        {
            if (ButtonClicked != null)
                ButtonClicked(this, new ButtonClickArgs {model=hospital });
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            //BookCal.Visibility = Visibility.Visible;
            //BookCombo.Visibility = Visibility.Visible;
           
            onButtonClicked();
        }
    }
}

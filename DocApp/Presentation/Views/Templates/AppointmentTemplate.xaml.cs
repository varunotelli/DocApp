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
    public sealed partial class AppointmentTemplate : UserControl
    {
        Models.AppointmentDetails app { get { return this.DataContext as Models.AppointmentDetails; } }
        public delegate void ButtonClickedEventHandler(object source, ButtonClickArgs args);
        public event ButtonClickedEventHandler CancelButtonClicked;
        public event ButtonClickedEventHandler RescheduleButtonClicked;
        public AppointmentTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        void onCancelButtonClicked()
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(this, new ButtonClickArgs() { id_val=app.id});
        }

        void onResButtonClicked()
        {
            if (RescheduleButtonClicked != null)
                RescheduleButtonClicked(this, new ButtonClickArgs() { id_val = app.id });
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            onCancelButtonClicked();
        }

        private void ResBtn_Click(object sender, RoutedEventArgs e)
        {
            onResButtonClicked();
        }
    }
}

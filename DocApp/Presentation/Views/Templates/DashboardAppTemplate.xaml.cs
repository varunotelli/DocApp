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
    public sealed partial class DashboardAppTemplate : UserControl
    {
        public delegate void ButtonClickedEventHandler(object source, ButtonClickArgs args);
        public event ButtonClickedEventHandler CancelButtonClicked;
        public event ButtonClickedEventHandler RescheduleButtonClicked;
        Models.AppointmentDetails details { get { return this.DataContext as Models.AppointmentDetails; } }
        public DashboardAppTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        void onCancelButtonClicked()
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(this, new ButtonClickArgs() { id_val = details.id });
        }

        void onResButtonClicked()
        {
            if (RescheduleButtonClicked != null)
                RescheduleButtonClicked(this, new ButtonClickArgs() { id_val = details.id });
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            onCancelButtonClicked();
        }

        private void ResBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("image=" + details.img);
            onResButtonClicked();
        }

        
        public void onPointerEntered(object sender, EventArgs args)
        {
            ResBtn.Opacity = 1;
            CancelBtn.Opacity = 1;
        }

        public void onPointerExited(object sender, EventArgs args)
        {
            ResBtn.Opacity = 0;
            CancelBtn.Opacity = 0;
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {

            //ResBtn.Opacity = 0;
            //CancelBtn.Opacity = 0;
        }
    }
}

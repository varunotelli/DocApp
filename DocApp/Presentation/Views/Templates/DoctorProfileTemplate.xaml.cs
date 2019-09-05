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
    public sealed partial class DoctorProfileTemplate : UserControl
    {
        public Models.Doctor doctor { get {return  this.DataContext as Models.Doctor; } }
        public delegate void BackButtonEventHandler(object source, EventArgs e);
        public event BackButtonEventHandler BackButtonClicked;
        public DoctorProfileTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        public void onBackButtonClicked()
        {
            if (BackButtonClicked != null)
                BackButtonClicked(this, EventArgs.Empty);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            onBackButtonClicked();
        }
    }
}

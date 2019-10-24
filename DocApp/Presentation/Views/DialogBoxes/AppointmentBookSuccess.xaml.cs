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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views.DialogBoxes
{
    public sealed partial class AppointmentBookSuccess : ContentDialog
    {
        public delegate void ButtonClickedEventHandler(object source, EventArgs args);
        public event ButtonClickedEventHandler ButtonClicked;
        public AppointmentBookSuccess()
        {
            this.InitializeComponent();
        }

        public void onButtonClicked()
        {
            if (ButtonClicked != null)
                ButtonClicked(this, EventArgs.Empty);
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
            onButtonClicked();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}

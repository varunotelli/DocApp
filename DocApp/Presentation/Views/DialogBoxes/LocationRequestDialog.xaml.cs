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
    public sealed partial class LocationRequestDialog : ContentDialog
    {
        public delegate void ButtonClickedEvent(object source, EventArgs eventArgs);
        public event ButtonClickedEvent YesClicked;
        public event ButtonClickedEvent NoClicked;
        public bool flag = false;
        public LocationRequestDialog()
        {
            this.InitializeComponent();
        }

        public void onYesClicked()
        {
            if (YesClicked != null)
                YesClicked(this, EventArgs.Empty);
            flag = true;
        }

        public void onNoClicked()
        {
            if (NoClicked != null)
                NoClicked(this, EventArgs.Empty);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            onYesClicked();
            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            onNoClicked();
            this.Hide();
        }
    }
}

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
    public sealed partial class LastBookDialog : ContentDialog
    {
        public string txt { get; set; }
        public delegate void ButtonClickedEventHandler(object source, EventArgs args);
        public event ButtonClickedEventHandler YesButtonClicked;
        public event ButtonClickedEventHandler NoButtonClicked;
        public LastBookDialog()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        public void onYesButtonClicked()
        {
            if (YesButtonClicked != null)
                YesButtonClicked(this, EventArgs.Empty);
        }

        public void onNoButtonClicked()
        {
            if (NoButtonClicked != null)
                NoButtonClicked(this, EventArgs.Empty);
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            onYesButtonClicked();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            onNoButtonClicked();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

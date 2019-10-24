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
    public sealed partial class BookedDocTemplate : UserControl
    {
        public Models.Doctor doctor { get { return this.DataContext as Models.Doctor; } }
        public delegate void ButtonClickedEventHandler(object source, ButtonClickArgs args);
        public event ButtonClickedEventHandler ButtonClicked;
        public BookedDocTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        public void onButtonClicked()
        {
            if (ButtonClicked != null)
                ButtonClicked(this, new ButtonClickArgs() { id_val=doctor.ID });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            onButtonClicked();
        }
    }
}

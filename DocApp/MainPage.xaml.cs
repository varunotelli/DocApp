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
using DocApp.Presentation.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DocApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        double current_scroll=0.0f;
        public MainPage()
        {
            this.InitializeComponent();
            
            TitleFrame.Navigate(typeof(HospitalView));
        }

        public void OnBackButtonClicked(object source, EventArgs eventArgs)
        {
            System.Diagnostics.Debug.WriteLine("FIRED!!!!!");
            MainScroll.ChangeView(0.0f, current_scroll, 1.0f);
            current_scroll = MainScroll.VerticalOffset;
        }

        public void OnListViewItemSelected(object source, EventArgs eventArgs)
        {
            current_scroll = MainScroll.VerticalOffset;
            System.Diagnostics.Debug.WriteLine("FIRED!!!!!");
            MainScroll.ChangeView(0.0f, 300.0f, 1.0f);
        }
        
    }
}

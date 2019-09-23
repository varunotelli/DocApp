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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    


    public sealed partial class MainPageBuffer : Page
    {
        string addr;
        MainPage mainPage;
        public MainPageBuffer()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e1)
        {
            var temp = e1.Parameter as navargs;
            addr = temp.name;
            mainPage = temp.mp;
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            

        }

        public void onAutoSuggestChanged(object sender, navargs2 args)
        {
            addr = args.location;
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var griditem = sender as GridView;
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;
            AutoSuggestBox autoSuggestBox = grid.FindName("MyAutoSuggest") as AutoSuggestBox;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            if (griditem.SelectedIndex == 0)
            {
                my_frame.Navigate(typeof(DoctorSearchResultView), new navargs { name = addr, index = 0, mp = mainPage });

            }
            else if (griditem.SelectedIndex == 1)
                my_frame.Navigate(typeof(HospitalSearchResultView), new navargs { name = addr, index = 0, mp = mainPage }); ;

            
        }
    }
}

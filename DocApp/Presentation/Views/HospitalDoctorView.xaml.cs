using DocApp.Models;
using DocApp.Presentation.ViewModels;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HospitalDoctorView : Page
    {
        string name = "";
        HospitalDoctorViewModel viewModel = new HospitalDoctorViewModel();
        public HospitalDoctorView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            navargs args = e1.Parameter as navargs;
            //System.Diagnostics.Debug.WriteLine("Sent val=" + (string)e1.Parameter);
            name = args.name;
            if(!args.location)
            {
                DocNearYouBlock.Visibility = Visibility.Collapsed;
                HospNearYouBlock.Visibility = Visibility.Collapsed;
                await viewModel.GetHospitals(name);
                await viewModel.GetDoctors(name);
            }
            else
            {
                DocNearYouBlock.Visibility = Visibility.Visible;
                HospNearYouBlock.Visibility = Visibility.Visible;
                await viewModel.GetDoctorByLocation(name);
                await viewModel.GetHospitalByLocation(name);
            }
            
            //System.Diagnostics.Debug.WriteLine("Doc size=" + (string)e1.Parameter);
            return;



        }

        private void DoctorGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(DoctorDetailView), e.ClickedItem as Doctor);
        }

        private void HospitalGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(HospitalDetailView), e.ClickedItem as Hospital);
        }
    }
}

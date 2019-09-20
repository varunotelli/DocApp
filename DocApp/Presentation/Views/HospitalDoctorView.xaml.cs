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
    /// 
    /*
     * @todo  Create new use case to get do
     */
    public sealed partial class HospitalDoctorView : Page
    {
        string address;
        int dept;
        MainPage mp;
        string name;
        HospitalDoctorViewModel viewModel = new HospitalDoctorViewModel();
        public HospitalDoctorView()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            navargs1 args = e1.Parameter as navargs1;
            name = args.name;
            if(args.dept_id==-1)
            {
                await viewModel.GetDoctorByLocation(args.location);
                await viewModel.GetHospitalByLocation(args.location);
            }
            else
            {
               
                await viewModel.GetDoctorsByName(args.name,args.location);
                await viewModel.GetHospitalByName(args.name,args.location);
            }
            mp = args.mp;
            mp.AutoSuggestChanged += this.onAutoSuggestChanged;
            return;



        }

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            address = n.location;
            if (dept != -1)
            {
                await viewModel.GetDoctorsByDept(address, dept);
                await viewModel.GetHospitalByDept(address, dept);
            }
            else
            {
                await viewModel.GetDoctorByLocation(address);
                await viewModel.GetHospitalByLocation(address);
            }
            
        }


        private void DoctorGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(DoctorDetailView), (e.ClickedItem as Doctor).ID);
        }

        private void HospitalGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(HospitalDetailView), (e.ClickedItem as Hospital).ID);
        }
    }
}

using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.Templates;
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
    /*
     * @todo  Create new use case to get do
     */

    public class DocNavEventArgs:EventArgs
    {
        public INavEvents view { get; set; }
        public int val { get; set; }
        public int vis { get; set; }
    }
    public sealed partial class HospitalDoctorView : Page,INavEvents
    {
        string address;
        int dept;
        MainPage mp;
        string name;
        SelectedDocDetailView view;
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
            address = args.location;
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


        private async void DoctorGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;
            DetailSplitView.IsPaneOpen = true;
            SecondStack.Visibility = Visibility.Collapsed;
            FirstStack.SetValue(Grid.ColumnSpanProperty, 2);
            await viewModel.AddDocSearchResult(new Doc_Search() { doc_id = (e.ClickedItem as Doctor).ID, user_id = 1 });
            HospDocFrame.Navigate(typeof(SelectedDocDetailView), 
                new DocNavEventArgs() {val= (e.ClickedItem as Doctor).ID, view=this }
            , new SuppressNavigationTransitionInfo());
        }

        private void HospitalGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            FirstStack.Visibility = Visibility.Collapsed;
            SecondStack.SetValue(Grid.ColumnProperty, 0);
            SecondStack.SetValue(Grid.ColumnSpanProperty, 2);
            DetailSplitView.IsPaneOpen = true;
            HospDocFrame.Navigate(typeof(SelectedHospView),
                new DocNavEventArgs() { val = (e.ClickedItem as Hospital).ID, view = this }
            , new SuppressNavigationTransitionInfo());
        }

        private void DocSeeAll_Click(object sender, RoutedEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame myFrame = grid.FindName("myFrame") as Frame;
            myFrame.Navigate(typeof(DoctorSearchResultView), new navargs
            {
                name = address,
                mp = this.mp,
                index = 0,
                doc = true

            });
        }

        
        private void HospSeeAll_Click(object sender, RoutedEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame myFrame = grid.FindName("myFrame") as Frame;
            myFrame.Navigate(typeof(DoctorSearchResultView), new navargs
            {
                name = address,
                mp = this.mp,
                index = 0,
                doc = false

            });

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            SecondStack.SetValue(Grid.ColumnProperty, 1);
            SecondStack.SetValue(Grid.ColumnSpanProperty, 1);
            FirstStack.SetValue(Grid.ColumnSpanProperty, 1);
            FirstStack.Visibility = Visibility.Visible;
            SecondStack.Visibility = Visibility.Visible;
            DetailSplitView.IsPaneOpen = false;
        }

        private void HospDocFrame_Loaded(object sender, RoutedEventArgs e)
        {
            //view.UpdateEvent += this.onDoctorUpdateSuccess;
        }

        public void onDoctorUpdateSuccess(object sender, UpdateDocEventArgs args)
        {
            view = args.page;
            var item = viewModel.doctors.FirstOrDefault(i => i.ID == args.doctor.ID);
            var index = viewModel.doctors.IndexOf(item);
            if (item != null)
                item = args.doctor;
            viewModel.doctors[index] = item;
            Bindings.Update();
        }

        public void onHospitalUpdateSuccess(object sender, UpdateHospEventArgs args)
        {
            //view = args.page;
            var item = viewModel.hospitals.FirstOrDefault(i => i.ID == args.hospital.ID);
            var index = viewModel.hospitals.IndexOf(item);
            if (item != null)
                item = args.hospital;
            viewModel.hospitals[index] = item;
            Bindings.Update();
        }
    }
}

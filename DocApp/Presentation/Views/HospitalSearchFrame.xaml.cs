using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.Controls;
using DocApp.Presentation.Views.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class HospNavEventArgs : EventArgs
    {
        public IHospEvents view { get; set; }
        public int val { get; set; }
        
    }
    public sealed partial class HospitalSearchFrame : Page,IHospEvents,IFilter,INotifyPropertyChanged
    {
        public DoctorSearchViewModel viewModel;
        public DoctorSearchResultView view;
        
        int index;
        int id;
        MainPage mainPage;
        int docorderby;
        int deptindex = 1;
        string d;
        public string dept
        {
            get
            {
                return d;
            }
            set
            {
                d = value;
                RaisePropertyChanged("dept");
            }
        }
        string add;
        public string address
        {
            get
            {
                return add;
            }
            set
            {
                add = value;
                RaisePropertyChanged("address");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));

            }
        }
        int lexp = -1, uexp = 200, rating = -1;

        public HospitalSearchFrame()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

            viewModel = new DoctorSearchViewModel();
            var args = e1.Parameter as DocSendEventArgs;
            view = args.view;
            address = args.add;
            mainPage = args.mp;
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            mainPage.LocationButtonClicked += this.onLocationButtonClicked;
            view.OrderComboChanged += this.onComboChanged;
            FilterButton.DeptListChanged += this.onDeptListChanged;

            FilterButton.RatingChanged += this.onRatingChanged;
            FilterButton.RatingCleared += this.onRatingCleared;
            viewModel.HospsSuccess += this.onHospsSuccess;
            
            await viewModel.GetHospitalByDept(address, 1, rating);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {

            mySplitView.IsPaneOpen = false;
        }

        public void onHospitalUpdateSuccess(object sender, UpdateHospEventArgs args)
        {
            //view = args.page;
            var item = viewModel.hosps.FirstOrDefault(i => i.ID == args.hospital.ID);
            var index = viewModel.hosps.IndexOf(item);
            if (item != null)
                item = args.hospital;
            viewModel.hosps[index] = item;

            item = viewModel.hospsmain.FirstOrDefault(i => i.ID == args.hospital.ID);
            index = viewModel.hospsmain.IndexOf(item);
            if (item != null)
                item = args.hospital;
            viewModel.hospsmain[index] = item;
            Bindings.Update();
        }

        public async void onDeptListChanged(object source, FilterEventArgs args)
        {
            deptindex = args.deptindex;
            dept = args.deptname;
            await viewModel.GetHospitalByDept(address, args.deptindex,  args.rat);
        }

        public async void onExpListChanged(object source, FilterEventArgs args)
        {
            await viewModel.GetHospitalByDept(address, args.deptindex, args.rat);
        }

        public async void onExpCleared(object source, FilterEventArgs args)
        {
            await viewModel.GetHospitalByDept(address, args.deptindex, args.rat);
        }

        public async void onRatingChanged(object source, FilterEventArgs args)
        {
            await viewModel.GetHospitalByDept(address, args.deptindex, args.rat);
        }

        public async void onRatingCleared(object source, FilterEventArgs args)
        {
            await viewModel.GetHospitalByDept(address, args.deptindex, args.rat);
        }

        
        private async void MyHospListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            index = myHospListView.SelectedIndex;
            var doc = e.ClickedItem as Hospital;
            id = doc.ID;
            await viewModel.GetHospital(id);
            mySplitView.IsPaneOpen = false;
            mySplitView.IsPaneOpen = true;
            myFrame.Navigate(typeof(SelectedHospView),
                new HospNavEventArgs() { val = (e.ClickedItem as Hospital).ID, view = this }
            , new SuppressNavigationTransitionInfo());
        }

        public void onComboChanged(object source, OrderEventArgs args)
        {
            var temp = new List<Hospital>(viewModel.hospsmain);
            viewModel.hospsmain.Clear();
            docorderby = args.index;
            if (args.index == 0)
            {
                foreach (var i in temp.OrderBy(d => d.Name))
                    viewModel.hospsmain.Add(i);
            }

            else if (args.index == 1)
            {
                foreach (var i in temp.OrderByDescending(d => d.Rating))
                    viewModel.hospsmain.Add(i);
            }
            else if (args.index == 2)
            {
                foreach (var i in temp.OrderByDescending(d => d.Number_Of_Rating))
                    viewModel.hospsmain.Add(i);
            }

        }

        public void onHospsSuccess(object source, EventArgs args)
        {
            viewModel.hospsmain.Clear();
            if (docorderby == 0)
                foreach (var i in viewModel.hosps.Where(d => d.Rating >= rating).OrderBy(d => d.Name))
                    viewModel.hospsmain.Add(i);
            else if (docorderby == 1)
                foreach (var i in viewModel.hosps.Where(d =>d.Rating >= rating).OrderByDescending(d => d.Rating))
                    viewModel.hospsmain.Add(i);
            else if (docorderby == 2)
                foreach (var i in viewModel.hosps.Where(d => d.Rating >= rating).OrderByDescending(d => d.Number_Of_Rating))
                    viewModel.hospsmain.Add(i);
            else
                foreach (var i in viewModel.hosps.Where(d => d.Rating >= rating))
                    viewModel.hospsmain.Add(i);
            //doctemp = viewModel.docsmain;
            mySplitView.IsPaneOpen = false;

        }

        public async void onLocationButtonClicked(object source, navargs2 n)
        {
            address = n.location;
            await viewModel.GetHospitalByDept(address, deptindex);
        }

        private void FilterButton_Loaded(object sender, RoutedEventArgs e)
        {
            dept = "CARDIOLOGY";
        }

        private void MySplitView_PaneClosed(SplitView sender, object args)
        {
            mySplitView.Focus(FocusState.Programmatic);
        }

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            address = n.location;
            await viewModel.GetHospitalByDept(address, deptindex);
        }

        public void onComboChanged(object source, ComboBoxSelectEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

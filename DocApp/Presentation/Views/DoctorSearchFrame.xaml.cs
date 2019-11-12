using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.Controls;
using DocApp.Presentation.Views.ViewInterfaces;
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
    public sealed partial class DoctorSearchFrame : Page, INavEvents,IFrames
    {
        public DoctorSearchViewModel viewModel;
        DoctorSearchResultView view;
        int index, id,docorderby;
        int lexp = -1, uexp = 200, rating = -1;
        string address;
        MainPage mainPage;
        public DoctorSearchFrame()
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
            view.OrderComboChanged += this.onComboChanged;
            view.DeptListChanged += this.onDeptListChanged;
            view.ExpChanged += this.onExpListChanged;
            view.ExpCleared += this.onExpCleared;
            view.RatingChanged += this.onRatingChanged;
            view.RatingCleared += this.onRatingCleared;
            viewModel.DoctorsSuccess += this.onDoctorsSuccess;
            await viewModel.GetDoctorsByDept(address, 1, lexp, uexp, rating);
        }
        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            index = myListView.SelectedIndex;
            var doc = e.ClickedItem as Doctor;
            id = doc.ID;
            await viewModel.GetDoctor(id);
            mySplitView.IsPaneOpen = false;
            mySplitView.IsPaneOpen = true;
            MyFrame.Navigate(typeof(SelectedDocDetailView),
                new DocNavEventArgs() { val = (e.ClickedItem as Doctor).ID, view = this }
            , new SuppressNavigationTransitionInfo());

        }

        public void onDoctorUpdateSuccess(object sender, UpdateDocEventArgs args)
        {
            var item = viewModel.docsmain.FirstOrDefault(i => i.ID == args.doctor.ID);
            var index = viewModel.docsmain.IndexOf(item);
            if (item != null)
                item = args.doctor;
            viewModel.docsmain[index] = item;
            item = viewModel.docs.FirstOrDefault(i => i.ID == args.doctor.ID);
            index = viewModel.docs.IndexOf(item);
            if (item != null)
                item = args.doctor;
            viewModel.docs[index] = item;
            Bindings.Update();
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {

            mySplitView.IsPaneOpen = false;
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = myListView.SelectedIndex;

        }

        public async void onDeptListChanged(object source, FilterEventArgs args)
        {
            await viewModel.GetDoctorsByDept(address, args.deptindex,args.lower , args.upper, args.rat);
        }

        public async void onExpListChanged(object source, FilterEventArgs args)
        {
            await viewModel.GetDoctorsByDept(address, args.deptindex, args.lower, args.upper, args.rat);
        }

        public async void onExpCleared(object source, FilterEventArgs args)
        {
            await viewModel.GetDoctorsByDept(address, args.deptindex, args.lower, args.upper, args.rat);
        }

        public async void onRatingChanged(object source, FilterEventArgs args)
        {
            await viewModel.GetDoctorsByDept(address, args.deptindex, args.lower, args.upper, args.rat);
        }

        public async void onRatingCleared(object source, FilterEventArgs args)
        {
            await viewModel.GetDoctorsByDept(address, args.deptindex, args.lower, args.upper, args.rat);
        }

        public void onComboChanged(object source, OrderEventArgs args)
        {
            var temp = new List<Doctor>(viewModel.docsmain);
            viewModel.docsmain.Clear();
            docorderby = args.index;
            if (args.index == 0)
            {
                foreach (var i in temp.OrderBy(d => d.Name))
                    viewModel.docsmain.Add(i);
            }

            else if (args.index == 1)
            {
                foreach (var i in temp.OrderByDescending(d => d.Rating))
                    viewModel.docsmain.Add(i);
            }
            else if (args.index == 2)
            {
                foreach (var i in temp.OrderByDescending(d => d.Number_of_Rating))
                    viewModel.docsmain.Add(i);
            }

        }

        public void onDoctorsSuccess(object source, EventArgs args)
        {
            viewModel.docsmain.Clear();
            if (docorderby == 0)
                foreach (var i in viewModel.docs.Where(d => d.Experience >= lexp &&
                d.Experience <= uexp && d.Rating >= rating).OrderBy(d => d.Name))
                    viewModel.docsmain.Add(i);
            else if (docorderby == 1)
                foreach (var i in viewModel.docs.Where(d => d.Experience >= lexp &&
                d.Experience <= uexp && d.Rating >= rating).OrderByDescending(d => d.Rating))
                    viewModel.docsmain.Add(i);
            else if (docorderby == 2)
                foreach (var i in viewModel.docs.Where(d => d.Experience >= lexp &&
                d.Experience <= uexp && d.Rating >= rating).OrderByDescending(d => d.Number_of_Rating))
                    viewModel.docsmain.Add(i);
            else
                foreach (var i in viewModel.docs.Where(d => d.Experience >= lexp &&
                d.Experience <= uexp && d.Rating >= rating))
                    viewModel.docsmain.Add(i);
            //doctemp = viewModel.docsmain;
            mySplitView.IsPaneOpen = false;
            
        }
    }
}

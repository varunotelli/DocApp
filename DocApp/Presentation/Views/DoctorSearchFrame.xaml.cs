using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.Controls;
using DocApp.Presentation.Views.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class DoctorSearchFrame : Page, INavEvents,IFrames,IFilter,INotifyPropertyChanged
    {
        public DoctorSearchViewModel viewModel;
        DoctorSearchResultView view;
        int index, id,docorderby;
        public int deptindex = 1;
        int stackval = 0;
        int lexp = -1, uexp = 200, rating = -1;
        //string address;
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
        MainPage mainPage;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));

            }
        }

        public DoctorSearchFrame()
        {
            this.InitializeComponent();
            //BackBtn.myFrame = this.Frame;
            mySplitView.Focus(FocusState.Programmatic);

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
            OrderCombo.ComboSelectionChanged += this.onComboChanged;
            //view.OrderComboChanged += this.onComboChanged;
            FilterButton.DeptListChanged += this.onDeptListChanged;
            FilterButton.ExpChanged += this.onExpListChanged;
            FilterButton.ExpCleared += this.onExpCleared;
            FilterButton.RatingChanged += this.onRatingChanged;
            FilterButton.RatingCleared += this.onRatingCleared;
            viewModel.DeptRead += this.onDeptRead;
            viewModel.DoctorsSuccess += this.onDoctorsSuccess;
            mySplitView.Focus(FocusState.Programmatic);
            //myListView.SelectedIndex = 0;
            await viewModel.GetDepartments();
            
        }

        public void onListClicked(object source, EventArgs args)
        {
            
            BackButton.Visibility = Visibility.Visible;
        }

        public async void onDeptRead(object source, EventArgs args)
        {
            //FilterButton.deptnames = new ObservableCollection<string>();
            foreach (var x in viewModel.deptnames)
                FilterButton.deptnames.Add(x);
            await viewModel.GetDoctorsByDept(address, 1, lexp, uexp, rating);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            mySplitView.IsPaneOpen = true;
            MyFrame.BackStack.Clear();

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
            MyFrame.BackStack.Clear();
            mySplitView.IsPaneOpen = false;
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            index = myListView.SelectedIndex;
            ShowingText.Opacity = 0;
            ShowingText.Opacity = 0;
            var temp = sender as ListView;
            index = myListView.SelectedIndex;
            if(myListView.SelectedItem!=null)
            {
                var doc = myListView.SelectedItem as Doctor;
                id = doc.ID;
                //await viewModel.GetDoctor(id);
                //mySplitView.IsPaneOpen = false;
                if(!mySplitView.IsPaneOpen)
                    mySplitView.IsPaneOpen = true;
                MyFrame.Navigate(typeof(SelectedDocDetailView),
                    new DocNavEventArgs() { val = (doc).ID, view = this }
                , new SuppressNavigationTransitionInfo());
                MyFrame.BackStack.Clear();
            }
            

        }

        public async void onDeptListChanged(object source, FilterEventArgs args)
        {
            deptindex = args.deptindex;
            dept = args.deptname;
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

        private void FilterButton_Loaded(object sender, RoutedEventArgs e)
        {
            //FilterButton.deptnames = new ObservableCollection<string>();
            //foreach (var x in viewModel.deptnames)
            //    FilterButton.deptnames.Add(x);
            dept = "CARDIOLOGY";
            
        }

        private void MySplitView_PaneOpened(SplitView sender, object args)
        {
            ShowingText.Opacity = 0;
            //if (MyFrame.BackStackDepth <= 1)
            //    BackButton.Visibility = Visibility.Collapsed;
            //else BackButton.Visibility = Visibility.Visible;
            //MyFrame.BackStack.Clear();
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void MySplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            ShowingText.Opacity = 1;
            mySplitView.Focus(FocusState.Programmatic);
        }

        private void MySplitView_PaneClosed(SplitView sender, object args)
        {
            ShowingText.Opacity = 1;
            BackButton.Visibility = Visibility.Visible;
            MyFrame.BackStack.Clear();
        }

        private void MySplitView_PaneOpening(SplitView sender, object args)
        {
            ShowingText.Opacity = 0;

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.BackStackDepth == 1)
                BackButton.Visibility = Visibility.Collapsed;
                if (MyFrame.CanGoBack)
                    MyFrame.GoBack();
                
           
        }

        

        private void BackButton_Loaded(object sender, RoutedEventArgs e)
        {
            //BackButton.Visibility = Visibility.Collapsed;
        }

        public void onComboChanged(object source, ComboBoxSelectEventArgs args)
        {
            var temp = new List<Doctor>(viewModel.docsmain);
            viewModel.docsmain.Clear();
            docorderby = args.val;
            if (args.val == 0)
            {
                foreach (var i in temp.OrderBy(d => d.Name))
                    viewModel.docsmain.Add(i);
            }

            else if (args.val == 1)
            {
                foreach (var i in temp.OrderByDescending(d => d.Rating))
                    viewModel.docsmain.Add(i);
            }
            else if (args.val == 2)
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

        public async void onLocationButtonClicked(object source, navargs2 n)
        {
            address = n.location;
            await viewModel.GetDoctorsByDept(address, deptindex);
        }

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            address = n.location;
            await viewModel.GetDoctorsByDept(address, deptindex);
        }
    }
}

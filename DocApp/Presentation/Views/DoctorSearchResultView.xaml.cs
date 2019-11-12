using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.DialogBoxes;
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
using System.Text.RegularExpressions;
using Windows.UI;
using System.Collections.ObjectModel;
using DocApp.Presentation.Views.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /*
     * @todo Add filters
     * @todo Display next available slots if no vacancy
     * @todo Back Button for Appointments
     * @todo Custom Calendar
     */

     public class DocSendEventArgs: EventArgs
     {
        public string add { get; set; }
        public MainPage mp { get; set; }
        public DoctorSearchResultView view { get; set; }

     }

    public class OrderEventArgs:EventArgs
    {
        public int index { get; set; }
    }

    public class FilterEventArgs:EventArgs
    {
        public int deptindex { get; set; }
        public int lower { get; set; }
        public int upper { get; set; }
        public int rat { get; set; }
    }

    public sealed partial class DoctorSearchResultView : Page
    {
        public delegate void OrderEventHandler(object source, OrderEventArgs args);
        public event OrderEventHandler OrderComboChanged;

        public delegate void FilterEventHandler(object source, FilterEventArgs args);
        public event FilterEventHandler DeptListChanged;
        public event FilterEventHandler RatingChanged;
        public event FilterEventHandler ExpChanged;
        public event FilterEventHandler RatingCleared;
        public event FilterEventHandler ExpCleared;
        ObservableCollection<Doctor> doctemp = new ObservableCollection<Doctor>();

        public DoctorSearchViewModel viewModel;
        string address = "";
        int id;
        int h_id;
        int hosp_id;
        int index;
        bool en;
        int docorderby = -1;
        int lexp = -1, uexp=200, rating = -1;
        string time,app_date;
        MainPage mainPage;
        public DoctorSearchResultView()
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
           
        }

        public void onOrderComboChanged(int x)
        {
            if (OrderComboChanged != null)
                OrderComboChanged(this, new OrderEventArgs() { index = x });
        }

        public void onDeptListChanged(int l, int u, int r)
        {
            if (DeptListChanged != null)
                DeptListChanged(this, new FilterEventArgs() {lower=l,upper=u,rat=r,deptindex=DeptListbox.SelectedIndex });
        }


        public void onExpChanged(int l, int u, int r)
        {
            if (ExpChanged != null)
                ExpChanged(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onExpCleared(int l, int u, int r)
        {
            if (ExpCleared != null)
                ExpCleared(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onRatingChanged(int l, int u, int r)
        {
            if (RatingChanged != null)
                RatingChanged(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onRatingCleared(int l, int u ,int r)
        {
            if(RatingCleared!=null)
                RatingCleared(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onHospButtonClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            //Book_Pop.Visibility = Visibility.Visible;
            //Book_Pop.IsOpen = true;
            

        }

        public async void onAppCheckSuccess(object source, EventArgs args)
        {
            if (viewModel.ct > 0)
            {
                AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
                {
                    Title = "Appointment Booking Failed",
                    Content = String.Format("You already have an appointment on {0} at {1}", app_date, time),
                    CloseButtonText = "OK"

                };
                await bookFail.ShowAsync();
                //TimeSlotBox.SelectedIndex = -1;
            }

        }

        

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            
            viewModel = new DoctorSearchViewModel();
            var args = e1.Parameter as navargs;
            address = args.name;
            mainPage = args.mp;
            DeptListbox.SelectedIndex = args.index;
            if (args.doc)
                myFrame.Navigate(typeof(DoctorSearchFrame), new DocSendEventArgs() { view=this, add=address, mp=mainPage}, 
                    new SuppressNavigationTransitionInfo());
            else;
                //MainTabs.SelectedIndex = 1;
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            mainPage.LocationButtonClicked += this.onLocationButtonClicked;
            //viewModel.DoctorsSuccess+=this.onDoctorsSuccess;
            //viewModel.InsertFail += this.onInsertFail;
            //viewModel.InsertSuccess += this.onInsertSuccess;
            //viewModel.AppointmentRead += this.onAppointmentRead;
            //viewModel.DoctorRatingUpdateSuccess += this.onDoctorRatingUpdateSucess;
            //viewModel.TestimonialAddedSuccess += this.onTestAddedSuccess;
            //viewModel.HospitalRatingUpdateSuccess += this.onHospitalUpdateSuccess;
            //viewModel.AppointmentCheckSuccess += this.onAppCheckSuccess;
            OrderCombo.ComboSelectionChanged += this.onComboChanged;
            await viewModel.GetDepartments();
            //await viewModel.GetDoctorsByDept("Chennai", 0);

            //Bindings.Update();

        }


        public async void onLocationButtonClicked(object source, navargs2 n)
        {
            

        }

        public void onComboChanged(object source, ComboBoxSelectEventArgs args)
        {
            onOrderComboChanged(args.val);
        }

        public void onHospitalUpdateSuccess(object source, EventArgs args)
        {
            
            var item = viewModel.hosps.FirstOrDefault(i => i.ID==viewModel.hospital.ID);
            var index = viewModel.hosps.IndexOf(item);
            if (item != null)
                item = viewModel.hospital;
            viewModel.hosps[index] = item;
            item = viewModel.hospsmain.FirstOrDefault(i => i.ID == viewModel.hospital.ID);
            index = viewModel.hospsmain.IndexOf(item);
            if (item != null)
                item = viewModel.hospital;
            viewModel.hospsmain[index] = item;
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
            //mySplitView.IsPaneOpen = false;
            
        }


        public void onDoctorReadSuccess(object source, EventArgs args)
        {
            //mySplitView.IsPaneOpen = true;
        }

        
        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            //this.mySplitView.IsPaneOpen = false;
            address = n.location;
            
            if (DeptListbox.SelectedItem != null)
            {
                //if (MainTabs.SelectedIndex == 0)
                {
                    await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
                    //this.mySplitView.IsPaneOpen = false;
                }
                
            }
            
        }
        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {
            
            if (sender.Value > 0)
            {
                
                await viewModel.UpdateDoctor(id, (double)sender.Value);
               
                //myListView.SelectedIndex = index;
                //myRating.Caption = myRating.Value.ToString();

            }

        }

        private  void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //mySplitView.IsPaneOpen = false;
            
            var listbox = sender as ListBox;
            var select = listbox.SelectedIndex;

            onDeptListChanged(lexp, uexp, rating);
                
            
        }

       
        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;
            
            Frame my_frame = grid.FindName("myFrame") as Frame;
            //my_frame.Navigate(typeof(HospitalDetailView), (e.ClickedItem as HospitalInDoctorDetails).Hosp_ID, 
            //    new SuppressNavigationTransitionInfo());
        }

        
        
        public async void onInsertSuccess(object source,EventArgs args)
        {
            await viewModel.GetAppointment(app_date, time);
           
        }

        public async void onAppointmentRead(object source, EventArgs args)
        {
            AppointmentBookSuccess bookSuccess = new AppointmentBookSuccess()
            {
                Title = "Appointment Booking Confirmed",
                Content = String.Format("Appointment booked with Dr. {0} in {1},{2} on {3} at {4}", viewModel.app.doc_name,
               viewModel.app.hosp_name, viewModel.app.location, viewModel.app.app_date, viewModel.app.Timeslot),
                CloseButtonText = "OK"
            };
            await bookSuccess.ShowAsync();
        }

       

        public async void onInsertFail(object source, EventArgs args)
        {
            AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
            {
                Title = "Appointment Booking Failed",
                Content = String.Format("You already have an appointment on {0} at {1}", app_date, time),
                CloseButtonText = "OK"

            };
            await bookFail.ShowAsync();
        }

        private async void MySplitView_PaneOpened(SplitView sender, object args)
        {
            
            if (viewModel.doctor != null)
            {
                System.Diagnostics.Debug.WriteLine("Count=" + viewModel.doctor.Description.Count(c => c.Equals('\n')));
                if (viewModel.doctor.Description.Count(c => c.Equals('\n')) >= 1)
                {
                    
                }
                
            }
            await viewModel.GetTests(id);


        }

       

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            bool show = false;
            
            HyperlinkButton temp = sender as HyperlinkButton;
            if(!show)
            {
                temp.Visibility = Visibility.Collapsed;
                
            }
           
                
        }

       

        private async void MyHospListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            
            var hos = e.ClickedItem as Hospital;
            hosp_id = hos.ID;
            await viewModel.GetHospital(hosp_id);
            
            Bindings.Update();
        }

        private void DoctorsInHospitalTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorsInHospitalTemplate temp = (DoctorsInHospitalTemplate)sender;
            temp.ButtonClicked += this.onHospButtonClicked;
        }

        private async void MyHospRating_ValueChanged(RatingControl sender, object args)
        {

            if (sender.Value > 0)
            {
                //Bindings.Update();
                await viewModel.UpdateHospitalRating(hosp_id, (double)sender.Value);
                //Bindings.Update();
                //await viewModel.GetHospitalByDept(address, DeptListbox.SelectedIndex);
                //myHospListView.SelectedIndex = index;
                


            }
        }

        private void HospCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void TimeSlotText_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            Roster r = t.DataContext as Roster;
            if (r.val > 0)
            {
                t.Foreground = new SolidColorBrush(Colors.Green);
                viewModel.enabled = false;
                en = true;
            }
            else
            {
                t.Foreground = new SolidColorBrush(Colors.Red);
                viewModel.enabled = false;
                en = false;
                
            }
        }

       

        private void MyHospSplitView_PaneOpened(SplitView sender, object args)
        {
           
            
        }

        private void RatingClearBtn_Click(object sender, RoutedEventArgs e)
        {
            RatingListBox.SelectedIndex = -1;
            rating = -1;
            onRatingCleared(lexp, uexp, rating);
            
           

        }

        private void ExpClearBtn_Click(object sender, RoutedEventArgs e)
        {
            TenyearExp.IsChecked = false;
            FiveYearExp.IsChecked = false;
            OneYearExp.IsChecked = false;
            lexp = -1;
            uexp = 200;
            onExpCleared(lexp, uexp, rating);
            
            
            {
                viewModel.hospsmain.Clear();
                foreach (var x in viewModel.hosps.Where(h=>h.Rating>=rating))
                    viewModel.hospsmain.Add(x);
            }

        }

        private  void MainTabs_Loaded(object sender, RoutedEventArgs e)
        {
           // await viewModel.GetDoctorsByDept("Chennai".ToUpper(), 0);
        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            DeptListbox.SelectedIndex = 0;
            RatingClearBtn_Click(null, null);
            ExpClearBtn_Click(null, null);
        }

        private  void TenyearExp_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton rb = sender as RadioButton;
            
            if(rb!=null)
            {
                //mySplitView.IsPaneOpen = false;
                
                string exp = rb.Content.ToString();
                doctemp = viewModel.docsmain;
                switch(exp)
                {
                    case "> 10 year exp":
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => Int32.Parse(d.Experience.Split(' ')[0]) > 10))
                        //    viewModel.docsmain.Add(i);
                        lexp = 11;
                        uexp = 200;

                        break;
                    case "5-10 year exp":
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => Int32.Parse(d.Experience.Split(' ')[0]) <= 10 &&
                        //    Int32.Parse(d.Experience.Split(' ')[0]) >= 5))
                        //    viewModel.docsmain.Add(i);
                        lexp = 5;
                        uexp = 10;
                        

                        break;
                    case "< 5 year exp":
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => Int32.Parse(d.Experience.Split(' ')[0]) < 5))
                        //    viewModel.docsmain.Add(i);
                        uexp = 4;
                        lexp = -1;
                        break;

                }
                viewModel.docsmain.Clear();
                onExpChanged(lexp, uexp, rating);

                

                //lexp = -1;
                //uexp = 200;
                //rating = -1;

            }
            else
            {
                //foreach (var i in viewModel.docs)
                //    viewModel.docsmain.Add(i);
            }
            
        }

        private void RatingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if(lb.SelectedIndex!=-1)
            {
                //mySplitView.IsPaneOpen = false;
                doctemp = viewModel.docsmain;
                switch(lb.SelectedIndex+1)
                {
                    case 1:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 4))
                        //    viewModel.docsmain.Add(i);
                        rating = 4;
                        break;
                    case 2:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 3))
                        //    viewModel.docsmain.Add(i);
                        rating = 3;
                        break;
                    case 3:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 2))
                        //    viewModel.docsmain.Add(i);
                        rating = 2;
                        break;
                    case 4:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 1))
                        //    viewModel.docsmain.Add(i);
                        rating = 1;
                        break;

                }
                //if (MainTabs.SelectedIndex == 0)
                {
                    viewModel.docsmain.Clear();

                    onRatingChanged(lexp, uexp, rating);
                }
                
                    viewModel.hospsmain.Clear();
                    foreach (var i in viewModel.hosps.Where(h => h.Rating >= rating))
                        viewModel.hospsmain.Add(i);
                
                //await viewModel.GetDoctorsByDept(address,.SelectedIndex, lexp, uexp, rating);
                //lexp = -1;
                //uexp = 200;
                //rating = -1;
            }

        }

        public async void onTestAddedSuccess(object source, EventArgs args)
        {
            await viewModel.GetLastTest(id);
            //TestScroll.ChangeView(0, 0, 1);
        }

        
    }
}

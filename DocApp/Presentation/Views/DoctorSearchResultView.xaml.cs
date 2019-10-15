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
        public Doctor doc { get; set; }
        public DoctorSearchResultView view { get; set; }

     }

    public sealed partial class DoctorSearchResultView : Page
    {
        public delegate void GridItemClickedEventHandler(object sender, DocSendEventArgs args);
        public event GridItemClickedEventHandler GridItemClicked;

        public DoctorSearchViewModel viewModel;
        string address = "";
        int id;
        int h_id;
        int hosp_id;
        int index;
        bool en;
        string time,app_date;
        MainPage mainPage;
        public DoctorSearchResultView()
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
            mySplitView.IsPaneOpen = false;
            Appointment_Date.MinDate = DateTimeOffset.Now;

        }

        public void onButtonClicked(object source, ButtonClickArgs args)
        {
            //System.Diagnostics.Debug.WriteLine(args.model.Name);
            hosp_id = args.id_val;
            //pos = MyScroll.VerticalOffset;
            //MyScroll.ChangeView(null, 100, 1);
            Book_Pop.Visibility = Visibility.Visible;
            Book_Pop.IsOpen = true;
            HospList.SelectedItem = ((FrameworkElement)source).DataContext;
        }

        public void onHospButtonClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            Book_Pop.Visibility = Visibility.Visible;
            Book_Pop.IsOpen = true;
            DocList.SelectedItem = ((FrameworkElement)source).DataContext;

        }

        public async void onAppCheckSuccess(object source, EventArgs args)
        {
            if(viewModel.ct>0)
            {
                AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
                {
                    Title = "Appointment Booking Failed",
                    Content = String.Format("You already have an appointment on {0} at {1}", app_date, time),
                    CloseButtonText = "OK"

                };
                await bookFail.ShowAsync();
                TimeSlotBox.SelectedIndex = -1;
            }

        }

        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {

            await viewModel.GetTimeSlots(id, hosp_id,app_date);
            Bindings.Update();
        }

        private async void TimeSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedIndex != -1)
            {
                var val = box.SelectedItem as Roster;
                time = val.start_time.ToString();
                if(val.val==0)
                {
                    AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
                    {
                        Title = "Appointment Booking Failed",
                        Content = String.Format("No vacancies on {0} at {1}", app_date, time),
                        CloseButtonText = "OK"

                    };
                    await bookFail.ShowAsync();
                    box.SelectedIndex = -1;
                }
                else
                {
                    await viewModel.CheckApp(1, app_date, time);
                }

            }
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
            Bindings.Update();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            
            viewModel = new DoctorSearchViewModel();
            var args = e1.Parameter as navargs;
            address = args.name;
            mainPage = args.mp;
            DeptListbox.SelectedIndex = args.index;
            if (args.doc)
                MainTabs.SelectedIndex = 0;
            else
                MainTabs.SelectedIndex = 1;
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            viewModel.DoctorsSuccess+=this.onDoctorsSuccess;
            viewModel.InsertFail += this.onInsertFail;
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.AppointmentRead += this.onAppointmentRead;
            viewModel.DoctorRatingUpdateSuccess += this.onDoctorRatingUpdateSucess;
            viewModel.TestimonialAddedSuccess += this.onTestAddedSuccess;
            viewModel.HospitalRatingUpdateSuccess += this.onHospitalUpdateSuccess;
            viewModel.AppointmentCheckSuccess += this.onAppCheckSuccess;
         
            await viewModel.GetDepartments();
            
            //Bindings.Update();

        }

        public void onHospitalUpdateSuccess(object source, EventArgs args)
        {
            myHospSplitView.IsPaneOpen = true;
            int x = -1;
           
            for (int i = 0; i < viewModel.hosps.Count; i++)
                if (viewModel.hosps[i].ID == hosp_id)
                {
                    x = i;
                    break;
                }
            viewModel.hosps.Remove(viewModel.hosps[x]);
            viewModel.hosps.Insert(x, viewModel.hospital);
        }
        
        public void onDoctorsSuccess(object source, EventArgs args)
        {
            mySplitView.IsPaneOpen = false;
            myHospSplitView.IsPaneOpen = false;
        }


        public void onDoctorReadSuccess(object source, EventArgs args)
        {
            mySplitView.IsPaneOpen = true;
        }

        public void onDoctorRatingUpdateSucess(object source, EventArgs args)
        {
            int x=-1;
          
            for(int i=0;i<viewModel.docs.Count;i++)
                if(viewModel.docs[i].ID==id)
                {
                    x = i;
                    break;
                }
            viewModel.docs.Remove(viewModel.docs[x]);
            viewModel.docs.Insert(x, viewModel.doctor);
        }

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            this.mySplitView.IsPaneOpen = false;
            address = n.location;
            
            if (DeptListbox.SelectedItem != null)
            {
                if (MainTabs.SelectedIndex == 0)
                {
                    await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
                    this.mySplitView.IsPaneOpen = false;
                }
                else if (MainTabs.SelectedIndex==1)
                {
                    await viewModel.GetHospitalByDept(address, DeptListbox.SelectedIndex);
                    this.myHospSplitView.IsPaneOpen = false;
                    Bindings.Update();
                }
            }
            
        }
        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {
            
            if (sender.Value > 0)
            {
                
                await viewModel.UpdateDoctor(id, (double)sender.Value);
               
                myListView.SelectedIndex = index;
                myRating.Caption = myRating.Value.ToString();

            }

        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mySplitView.IsPaneOpen = false;
            myHospSplitView.IsPaneOpen = false;
            var listbox = sender as ListBox;
            var select = listbox.SelectedIndex;
            //myListView.SelectedIndex = -1;
            if (myListView.SelectedIndex==-1)
                mySplitView.IsPaneOpen = false;
            if (MainTabs.SelectedIndex == 0)
                await viewModel.GetDoctorsByDept(address, select);
            else if (MainTabs.SelectedIndex == 1)
                await viewModel.GetHospitalByDept(address, select);
            
            //Bindings.Update();
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

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            
            mySplitView.IsPaneOpen = false;
        }

        private void HospitalsInDoctorsTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            HospitalsInDoctorsTemplate temp = (HospitalsInDoctorsTemplate)sender;
            temp.ButtonClicked += this.onButtonClicked;
            
        }

        private void Appointment_Date_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

            if (sender.Date != null)
            {
                var date = sender.Date;
                DateTime t = date.Value.DateTime;
                app_date = String.Format("{0:yyyy-MM-dd}", t);
                System.Diagnostics.Debug.WriteLine("date=" + app_date);
                TimeSlotBox.IsEnabled = true;
            }
            else
            {
                TimeSlotBox.IsEnabled = false;
            }
            Bindings.Update();
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }

        private void Book_Pop_Closed(object sender, object e)
        {
            TimeSlotBox.SelectedIndex = -1;
            TimeSlotBox.IsEnabled = false;
            Appointment_Date.Date = null;
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
            //MyScroll.ChangeView(null, pos, 1);

        }
        private void Book_Pop_Opened(object sender, object e)
        {
            TimeSlotBox.IsEnabled = false;
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.BookAppointment(1, id, hosp_id, app_date, time);
            Book_Pop.IsOpen = false;
            Book_Pop.Visibility = Visibility.Visible;
            
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

        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedIndex == 1)
                await viewModel.GetTests(id);
        }

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Visibility = Visibility.Visible;
            SubmitBtn.Visibility = Visibility.Visible;
            
            
        }

        private async void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.AddTest(1, id, MessageBox.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            MessageBox.Text = "";
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
            //Showbutton.Visibility = Visibility.Collapsed;
            myRating.Value = Double.NaN;
            myRating.Caption = "Your Rating";
            MessageBox.Visibility = Visibility.Collapsed;
            SubmitBtn.Visibility = Visibility.Collapsed;
            if (viewModel.doctor != null)
            {
                System.Diagnostics.Debug.WriteLine("Count=" + viewModel.doctor.Description.Count(c => c.Equals('\n')));
                if (viewModel.doctor.Description.Count(c => c.Equals('\n')) >= 1)
                {
                    DescBox.Height = 20;
                    DescBox.Width = 550;
                    Showbutton.Visibility = Visibility.Visible;
                }
                else
                    Showbutton.Visibility = Visibility.Collapsed;
            }
            await viewModel.GetTests(id);


        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = myListView.SelectedIndex;
          
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            bool show = false;
            
            HyperlinkButton temp = sender as HyperlinkButton;
            if(!show)
            {
                temp.Visibility = Visibility.Collapsed;
                DescBox.TextWrapping = TextWrapping.WrapWholeWords;
                DescBox.Height = Double.NaN;
                DescBox.Width = 750;
                
            }
           
                
        }

        private async void MainTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mySplitView.IsPaneOpen = false;
            myHospSplitView.IsPaneOpen = false;
            if (MainTabs.SelectedIndex == 0)
                await viewModel.GetDoctorsByDept(address,DeptListbox.SelectedIndex);
            else if (MainTabs.SelectedIndex == 1)
                await viewModel.GetHospitalByDept(address, DeptListbox.SelectedIndex);

        }

        private async void MyHospListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            
            var hos = e.ClickedItem as Hospital;
            hosp_id = hos.ID;
            await viewModel.GetHospital(hosp_id);
            myHospSplitView.IsPaneOpen = false;
            myHospSplitView.IsPaneOpen = true;
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
                myHospListView.SelectedIndex = index;
                myHospRating.Caption = myHospRating.Value.ToString();



            }
        }

        private void HospCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            myHospSplitView.IsPaneOpen = false;
        }

        private async void TimeSlotText_Loaded(object sender, RoutedEventArgs e)
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

        private void DocList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void MyHospSplitView_PaneOpened(SplitView sender, object args)
        {
            myHospRating.Value = Double.NaN;
            myHospRating.Caption = "Your Rating";
            
        }

        public async void onTestAddedSuccess(object source, EventArgs args)
        {
            await viewModel.GetLastTest(id);
            //TestScroll.ChangeView(0, 0, 1);
        }
    }
}

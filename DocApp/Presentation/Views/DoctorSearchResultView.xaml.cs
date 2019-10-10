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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /*
     * @todo Style Testimonials and fix testimonial bugs
     * @todo Complete insert Testimonial
     * @todo Complete show more show less functionality for description
     * 
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
        int i;
        int hosp_id;
        int index;
        
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
            System.Diagnostics.Debug.WriteLine(args.model.Name);
            hosp_id = args.model.Hosp_ID;
            //pos = MyScroll.VerticalOffset;
            //MyScroll.ChangeView(null, 100, 1);
            Book_Pop.Visibility = Visibility.Visible;
            Book_Pop.IsOpen = true;
            HospList.SelectedItem = ((FrameworkElement)source).DataContext;
        }
        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {

            await viewModel.GetTimeSlots(id, hosp_id,app_date);
            Bindings.Update();
        }

        private void TimeSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedIndex != -1)
            {
                var val = box.SelectedItem as Roster;
                time = val.start_time.ToString();
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
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            viewModel.DoctorsSuccess+=this.onDoctorsSuccess;
            viewModel.InsertFail += this.onInsertFail;
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.AppointmentRead += this.onAppointmentRead;
            viewModel.DoctorRatingUpdateSuccess += this.onDoctorRatingUpdateSucess;
            viewModel.TestimonialAddedSuccess += this.onTestAddedSuccess;
            await viewModel.GetDoctors(address);
            
           
            await viewModel.GetDepartments();
            
            //Bindings.Update();

        }

        //public void onGridItemClicked(Doctor d)
        //{
        //    if (GridItemClicked != null)
        //        GridItemClicked(this, new DocSendEventArgs { doc = d, view = this });
        //}
        public void onDoctorsSuccess(object source, EventArgs args)
        {
            //mySplitView.IsPaneOpen = false;
        }


        public void onDoctorReadSuccess(object source, EventArgs args)
        {
            //myListView.SelectedIndex = -1;
            //Bindings.Update();
            mySplitView.IsPaneOpen = true;
            


        }

        public async void onDoctorRatingUpdateSucess(object source, EventArgs args)
        {
            //i = index;
            //viewModel.docs.Remove(viewModel.docs[index]);
            //viewModel.docs.Insert(i, viewModel.doctor);
            mySplitView.IsPaneOpen = true;
            await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);

        }

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            this.mySplitView.IsPaneOpen = false;
            address = n.location;
            
            if (DeptListbox.SelectedItem != null)
                await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
           
           this.mySplitView.IsPaneOpen = false;

            
        }
        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {
            
            if (sender.Value > 0)
            {
                Bindings.Update();
                await viewModel.UpdateDoctor(id, (double)sender.Value);
                Bindings.Update();
                //await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
                myListView.SelectedIndex = i;
                myRating.Caption = myRating.Value.ToString();



            }

        }



       

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            var select = listbox.SelectedIndex;
            //myListView.SelectedIndex = -1;
            if (myListView.SelectedIndex==-1)
                mySplitView.IsPaneOpen = false;
            await viewModel.GetDoctorsByDept(address, select);
            //Bindings.Update();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            index = myListView.SelectedIndex;
            var doc = e.ClickedItem as Doctor;
            id = doc.ID;
            await viewModel.GetDoctor(id);
            
            mySplitView.IsPaneOpen = true;
           

        }

        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;
            
            Frame my_frame = grid.FindName("myFrame") as Frame;
            my_frame.Navigate(typeof(HospitalDetailView), (e.ClickedItem as HospitalInDoctorDetails).Hosp_ID, 
                new SuppressNavigationTransitionInfo());
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
                Content = String.Format("Appointment book with Dr. {0} in {1},{2} on {3} at {4}", viewModel.app.doc_name,
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
            //TestScroll.UpdateLayout();
            //TestScroll.ChangeView(0, double.MaxValue, 1);
            
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
            mySplitView.IsPaneOpen = false;
            mySplitView.IsPaneOpen = true;
           // mySplitView.IsPaneOpen = false;

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

        public async void onTestAddedSuccess(object source, EventArgs args)
        {
            await viewModel.GetLastTest(id);
            //TestScroll.ChangeView(0, 0, 1);
        }
    }
}

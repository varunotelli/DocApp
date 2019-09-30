﻿using DocApp.Models;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /*
     * @todo Make new query to get doctors by location,department
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
        int hosp_id;
        double pos;
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

            await viewModel.GetTimeSlots(id, hosp_id);
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
            viewModel.InsertFail += this.onInsertFail;
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.AppointmentRead += this.onAppointmentRead;
            viewModel.DoctorRatingUpdateSuccess += this.onDoctorRatingUpdateSucess;
            await viewModel.GetDoctors(address);
            
           
            await viewModel.GetDepartments();
            
            //Bindings.Update();

        }

        //public void onGridItemClicked(Doctor d)
        //{
        //    if (GridItemClicked != null)
        //        GridItemClicked(this, new DocSendEventArgs { doc = d, view = this });
        //}

        public void onDoctorReadSuccess(object source, EventArgs args)
        {
            //myListView.SelectedIndex = -1;
            //Bindings.Update();
            mySplitView.IsPaneOpen = true;
            


        }

        public async void onDoctorRatingUpdateSucess(object source, EventArgs args)
        {
            await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
        }

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            address = n.location;
            if (DeptListbox.SelectedItem != null)
                await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
            else
                await viewModel.GetDoctors(address);
        }
        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {

            if (sender.Value > 0)
            {
                Bindings.Update();
                await viewModel.UpdateDoctor(id, (double)sender.Value);
                Bindings.Update();
                //await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
               
                myRating.Caption = myRating.Value.ToString();



            }

        }



        //private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //Bindings.Update();

        //    var temp = sender as HyperlinkButton;
        //    System.Diagnostics.Debug.WriteLine("Content="+(temp.Content as TextBlock).Text);
        //    await viewModel.GetDoctorsByDept(address, (temp.Content as TextBlock).Text);
        //    //await viewModel.GetDoctorsByDept(address, (temp.Content as TextBlock).Text);
        //    System.Diagnostics.Debug.WriteLine("Doc dept val=" + viewModel.docs.Count);
        //    Bindings.Update();
        //}

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            var select = listbox.SelectedIndex;
            mySplitView.IsPaneOpen = false;
            await viewModel.GetDoctorsByDept(address, select);
            //Bindings.Update();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Doctor d = e.ClickedItem as Doctor;

            //Frame parentFrame = Window.Current.Content as Frame;

            //MainPage mp1 = parentFrame.Content as MainPage;
            //StackPanel grid = mp1.Content as StackPanel;

            //Frame my_frame = grid.FindName("myFrame") as Frame;
            //my_frame.Navigate(typeof(DoctorDetailView), (e.ClickedItem as Doctor).ID, new SuppressNavigationTransitionInfo());
            var doc = e.ClickedItem as Doctor;
            id = doc.ID;
            await viewModel.GetDoctor(id);
            
            mySplitView.IsPaneOpen = true;

        }

        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {

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
    }
}

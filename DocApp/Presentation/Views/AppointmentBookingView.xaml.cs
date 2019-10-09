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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentBookingView : Page
    {
        int doc_id;
        int hosp_id;
        string time;
        string app_date;
        AppointmentBookingViewModel viewModel;
        public AppointmentBookingView()
        {
            this.InitializeComponent();
            Appointment_Date.IsEnabled = false;
            TimeSlotBox.IsEnabled = false;
            Appointment_Date.MinDate = DateTimeOffset.Now;
            BookButton.IsEnabled = (HospCombo.SelectedIndex != -1) && (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
            
            Bindings.Update();

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

            doc_id = (int)e1.Parameter;
            viewModel = new AppointmentBookingViewModel(doc_id);
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.InsertFail += this.onInsertFail;
            await viewModel.GetDoctor(doc_id);
            Bindings.Update();

        }

        private async void HospCombo_DropDownOpened(object sender, object e)
        {
            await viewModel.GetHospitals(doc_id);
            Bindings.Update();
        }

        private void HospCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedIndex != -1)
            {
                var hosp = box.SelectedItem as HospitalInDoctorDetails;
                hosp_id = hosp.Hosp_ID;
                Appointment_Date.IsEnabled = true;
            }
            else
            {
                
                Appointment_Date.IsEnabled = false;
                Appointment_Date.Date = null;
                TimeSlotBox.IsEnabled = false;
                TimeSlotBox.SelectedIndex = -1;
            }
            BookButton.IsEnabled = (HospCombo.SelectedIndex != -1) && (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1); 
            Bindings.Update();

        }

        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {

            await viewModel.GetTimeSlots(doc_id, hosp_id,app_date);
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
            BookButton.IsEnabled = (HospCombo.SelectedIndex != -1) && (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
            Bindings.Update();
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
            BookButton.IsEnabled = (HospCombo.SelectedIndex != -1) && (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }

        public void onInsertSuccess(object source, EventArgs args)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;

            Frame my_frame = grid.FindName("myFrame") as Frame;
            my_frame.Navigate(typeof(AppointmentsDisplayView), new SuppressNavigationTransitionInfo());
        }

        public void onInsertFail(object source, EventArgs args)
        {
            InsertFail.Visibility = Visibility.Visible;
        }

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {

            InsertFail.Visibility = Visibility.Collapsed;
            await viewModel.BookAppointment(1, doc_id, hosp_id, app_date, time);
            
            
                

        }

        
    }
}

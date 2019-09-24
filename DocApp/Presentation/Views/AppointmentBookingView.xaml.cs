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
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

            doc_id = (int)e1.Parameter;
            viewModel = new AppointmentBookingViewModel(doc_id);
            await viewModel.GetDoctor(doc_id);
   
        }

        private async void HospCombo_DropDownOpened(object sender, object e)
        {
            await viewModel.GetHospitals(doc_id);
        }

        private void HospCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            var hosp = box.SelectedItem as HospitalInDoctorDetails;
            hosp_id = hosp.Hosp_ID;

        }

        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {

            await viewModel.GetTimeSlots(doc_id, hosp_id);
        }

        private void TimeSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            var val = box.SelectedItem as Roster;
            time = val.start_time.ToString();
        }

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            var date = Appointment_Date.Date;
            DateTime t = date.Value.DateTime;
            app_date = String.Format("{0:yyyy-MM-dd}", t);
            System.Diagnostics.Debug.WriteLine("date=" + app_date);
            await viewModel.BookAppointment(1, doc_id, hosp_id, app_date, time);

        }
    }
}

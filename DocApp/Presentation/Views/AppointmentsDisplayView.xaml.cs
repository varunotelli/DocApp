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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentsDisplayView : Page
    {
        AppointmentDisplayViewModel viewModel;
        int id;
        AppointmentDetails temp;
        string app_date, time;
        public AppointmentsDisplayView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

           
            viewModel = new AppointmentDisplayViewModel();
            await viewModel.GetUpcomingApps(1);
            viewModel.AppointmentCheckSuccess += this.onAppCheckSuccess;
            viewModel.AppointmentReadSuccess += this.onAppReadSuccess;


        }

        public void onAppReadSuccess(object source, EventArgs args)
        {
            Book_Pop.IsOpen = true;
            Book_Pop.Visibility = Visibility.Visible;
        }


        public async void onAppCheckSuccess(object source, EventArgs args)
        {
            if (viewModel.ct > 0)
            {
                AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
                {
                    Title = "Appointment Booking Failed",
                    Content = String.Format("You already have an appointment on {0} at {1}", app_date, time),


                };
                await bookFail.ShowAsync();
                TimeSlotBox.SelectedIndex = -1;
            }

        }

        async void onYesButtonClicked(object source, EventArgs args)
        {
            viewModel.apps.Remove(temp);
            await viewModel.CancelApp(id);
        }

        async void onCancelButtonClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            temp= ((FrameworkElement)source).DataContext as AppointmentDetails;
            CancelDialog dialog = new CancelDialog();
            dialog.ButtonClicked += this.onYesButtonClicked;
            await dialog.ShowAsync();

        }

        async void onResBtnClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            await viewModel.GetApp(id);
            temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TimeSlotBox_DropDownOpened(object sender, object e)
        {

        }

        private void TimeSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Appointment_Date_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

        }

        private void Book_Pop_Opened(object sender, object e)
        {

        }

        private void Book_Pop_Closed(object sender, object e)
        {

        }

        private void TimeSlotText_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AppointmentTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            AppointmentTemplate template = sender as AppointmentTemplate;
            template.CancelButtonClicked += this.onCancelButtonClicked;
            template.RescheduleButtonClicked += this.onResBtnClicked;
        }
    }
}

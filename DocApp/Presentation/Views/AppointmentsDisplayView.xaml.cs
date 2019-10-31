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
using Windows.UI;
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
        DateTimeOffset date;
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
            viewModel.AppointmentUpdateSuccess += this.onAppUpdateSuccess;
            


        }

        public void onAppReadSuccess(object source, EventArgs args)
        {
            Appointment_Date.Date = viewModel.date;
            Book_Pop.IsOpen = true;
            Book_Pop.Visibility = Visibility.Visible;
            
        }

        public async void onAppUpdateSuccess(object souce, EventArgs args)
        {
            AppointmentBookSuccess bookSuccess = new AppointmentBookSuccess()
            {
                Title = "Appointment Rescheduled",
                Content = String.Format("Appointment booked with Dr. {0} in {1},{2} is rescheduled on {3} at {4}", 
                temp.doc_name, temp.hosp_name,temp.location, app_date, time),

            };
            //bookSuccess.ButtonClicked += this.onOKButtonClicked;
            //await viewModel.GetAppointments(1);
           
            app_date = viewModel.app.APP_DATE;
            await bookSuccess.ShowAsync();
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
            temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
            await viewModel.GetApp(id);

            //temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
        }

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.UpdateApp(viewModel.app.ID, app_date, time);
            Book_Pop.IsOpen = false;
            Book_Pop.Visibility = Visibility.Collapsed;
        }

        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {
            
            await viewModel.GetTimeSlots(viewModel.app.DOC_ID, viewModel.app.HOS_ID, app_date);
            Bindings.Update();
        }

        private async void TimeSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedIndex != -1)
            {
                var val = box.SelectedItem as Roster;
                time = val.start_time.ToString();
                if (val.val == 0)
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
        }

        private void Appointment_Date_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {
                if (sender.Date != null)
                {
                    var date = sender.Date;
                    DateTime t = date.Value.DateTime;
                    app_date = String.Format("{0:yyyy-MM-dd}", t);
                    System.Diagnostics.Debug.WriteLine("date=" + app_date);

                }
            }
            catch(Exception e)
            { }
            
        }

        private void Book_Pop_Opened(object sender, object e)
        {
            //TimeSlotBox.IsEnabled = false;
            //BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }

        private void Book_Pop_Closed(object sender, object e)
        {
            TimeSlotBox.SelectedIndex = -1;
            //TimeSlotBox.IsEnabled = false;
            //Appointment_Date.Date = null;
            //BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }

        private void TimeSlotText_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            Roster r = t.DataContext as Roster;
            if (r.val > 0)
            {
                t.Foreground = new SolidColorBrush(Colors.Green);
                //viewModel.enabled = false;
                //en = true;
            }
            else
            {
                t.Foreground = new SolidColorBrush(Colors.Red);
                //viewModel.enabled = false;
                //en = false;

            }
        }

        private void AppointmentTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            AppointmentTemplate template = sender as AppointmentTemplate;
            template.CancelButtonClicked += this.onCancelButtonClicked;
            template.RescheduleButtonClicked += this.onResBtnClicked;
        }
    }
}

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
    /// 


    public class UpdateHospEventArgs : EventArgs
    {
        public Hospital hospital { get; set; }
        public SelectedHospView page { get; set; }
    }
    public sealed partial class SelectedHospView : Page
    {
        SelectedHospViewModel viewModel;
        HospitalDoctorView view;
        int id,hosp_id;
        string app_date, time;
        bool en;
        public delegate void UpdateEventHandler(object source, UpdateHospEventArgs args);
        public event UpdateEventHandler UpdateEvent;

        public SelectedHospView()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            var temp = e1.Parameter as DocNavEventArgs;
            hosp_id = temp.val;
            view = (HospitalDoctorView)temp.view;
            viewModel = new SelectedHospViewModel();
            this.UpdateEvent += view.onHospitalUpdateSuccess;
           
            viewModel.InsertFail += this.onInsertFail;
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.AppointmentRead += this.onAppointmentRead;
            viewModel.HospitalRatingUpdateSuccess += this.onHospitalRatingUpdateSucess;

            viewModel.AppointmentCheckSuccess += this.onAppCheckSuccess;
            await viewModel.GetHospital(hosp_id);

        }

        private void DoctorsInHospitalTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorsInHospitalTemplate temp = (DoctorsInHospitalTemplate)sender;
            temp.ButtonClicked += this.onHospButtonClicked;
        }

        public void onHospButtonClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            Book_Pop.Visibility = Visibility.Visible;
            Book_Pop.IsOpen = true;
            DocList.SelectedItem = ((FrameworkElement)source).DataContext;

        }

        private async void onInsertSuccess(object source, EventArgs e)
        {
            await viewModel.GetAppointment(app_date, time);
        }
        private async void onAppCheckSuccess(object source, EventArgs e)
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

        private async void onInsertFail(object source, EventArgs e)
        {
            AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
            {
                Title = "Appointment Booking Failed",
                Content = String.Format("You already have an appointment on {0} at {1}", app_date, time),
                CloseButtonText = "OK"

            };
            await bookFail.ShowAsync();
        }

        void onUpdate()
        {
            if (UpdateEvent != null)
                UpdateEvent(this, new UpdateHospEventArgs() { hospital = viewModel.hospital, page = this });
        }

        private async void onAppointmentRead(object source, EventArgs e)
        {
            AppointmentBookSuccess bookSuccess = new AppointmentBookSuccess()
            {
                Title = "Appointment Booking Confirmed",
                Content = String.Format("Appointment booked with Dr. {0} in {1},{2} on {3} at {4}", viewModel.app.doc_name,
               viewModel.app.hosp_name, viewModel.app.location, viewModel.app.app_date, viewModel.app.Timeslot),

            };

            await bookSuccess.ShowAsync();
        }

        private void DocList_ItemClick(object sender, ItemClickEventArgs e)
        {

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
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
            Bindings.Update();
        }

        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {
            await viewModel.GetTimeSlots(id, hosp_id, app_date);
            Bindings.Update();
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

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.BookAppointment(1, id, hosp_id, app_date, time);
            Book_Pop.IsOpen = false;
            Book_Pop.Visibility = Visibility.Visible;
        }

        private void Book_Pop_Opened(object sender, object e)
        {
            TimeSlotBox.IsEnabled = false;
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }

        private void Book_Pop_Closed(object sender, object e)
        {
            TimeSlotBox.SelectedIndex = -1;
            TimeSlotBox.IsEnabled = false;
            Appointment_Date.Date = null;
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
        }
        public void onHospitalRatingUpdateSucess(object source, EventArgs args)
        {

            onUpdate();
        }

        private async void MyHospRating_ValueChanged(RatingControl sender, object args)
        {
            if (sender.Value > 0)
            {
                
                await viewModel.UpdateHospitalRating(hosp_id, (double)sender.Value);
               
                myHospRating.Caption = myHospRating.Value.ToString();



            }
        }
        
    }
}

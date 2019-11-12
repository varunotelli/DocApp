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
    public class UpdateDocEventArgs:EventArgs
    {
        public Doctor doctor { get; set; }
        public SelectedDocDetailView page { get; set; }
    }

    public sealed partial class SelectedDocDetailView : Page
    {
        SelectedDoctorViewModel viewModel;
        
        string app_date, time;
        int id,hosp_id,app_id;
        AppointmentDetails app_temp;
        bool en;
        INavEvents view;
        public delegate void UpdateEventHandler(object source, UpdateDocEventArgs args);
        public event UpdateEventHandler UpdateEvent;

        public SelectedDocDetailView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            var temp = e1.Parameter as DocNavEventArgs;
            id = temp.val;
            view = temp.view;
            viewModel = new SelectedDoctorViewModel();
            if(view!=null)
                this.UpdateEvent += view.onDoctorUpdateSuccess;
            viewModel.DoctorReadSuccess += this.onDoctorReadSuccess;
            viewModel.InsertFail += this.onInsertFail;
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.AppointmentRead += this.onAppointmentRead;
            viewModel.DoctorRatingUpdateSuccess += this.onDoctorRatingUpdateSucess;
            viewModel.TestimonialAddedSuccess += this.onTestAddedSuccess;
            viewModel.AppRead += this.onAppReadSuccess;
            viewModel.AppointmentUpdated+=this.onAppUpdateSuccess;
            viewModel.AppointmentCheckSuccess += this.onAppCheckSuccess;
            if (temp.vis == 0)
                Backbtn.Visibility = Visibility.Collapsed;
            await viewModel.GetDoctor(id);

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


        public async void onAppUpdateSuccess(object souce, EventArgs args)
        {
            AppointmentBookSuccess bookSuccess = new AppointmentBookSuccess()
            {
                Title = "Appointment Rescheduled",
                Content = String.Format("Appointment booked with Dr. {0} in {1},{2} is rescheduled on {3} at {4}",
                app_temp.doc_name, app_temp.hosp_name, app_temp.location, app_date, time),

            };
            //bookSuccess.ButtonClicked += this.onOKButtonClicked;
            //await viewModel.GetAppointments(1);

            app_date = viewModel.app_vals.APP_DATE;
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
                Res_Pop.IsOpen = false;
                Res_Pop.Visibility = Visibility.Collapsed;
                await bookFail.ShowAsync();
                TimeSlotBox.SelectedIndex = -1;
            }

        }

        public void onAppReadSuccess(object source, EventArgs args)
        {
            App_Date.Date = viewModel.date;
            Res_Pop.IsOpen = true;
            Res_Pop.Visibility = Visibility.Visible;

        }

        
        private async void onTestAddedSuccess(object source, EventArgs e)
        {
            await viewModel.GetLastTest(id);
        }

        private async void onAppointmentRead(object source, EventArgs e)
        {
            AppointmentBookSuccess bookSuccess = new AppointmentBookSuccess()
            {
                Title = "Appointment Booking Confirmed",
                Content = String.Format("Appointment booked with Dr. {0} in {1},{2} on {3} at {4}", viewModel.app.doc_name,
               viewModel.app.hosp_name, viewModel.app.location, viewModel.app.app_date, viewModel.app.Timeslot),

            };
            Tabs.SelectedIndex = 2;
            
            await bookSuccess.ShowAsync();
        }

        private void Res_Pop_Opened(object sender, object e)
        {
            Res_TimeSlotBox.SelectedIndex = -1;
        }

        private void Res_Pop_Closed(object sender, object e)
        {

        }

        private async void Res_TimeSlotBox_DropDownOpened(object sender, object e)
        {
            await viewModel.GetTimeSlots(viewModel.app_vals.DOC_ID, viewModel.app_vals.HOS_ID, app_date);
            Bindings.Update();
        }

        private async void Res_TimeSlotBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private async void ResButton_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.UpdateApp(viewModel.app_vals.ID, app_date, time);
            Res_Pop.IsOpen = false;
            Res_Pop.Visibility = Visibility.Collapsed;
        }

        private void App_Date_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
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
            catch (Exception e)
            { }
        }


        private async void onInsertSuccess(object source, EventArgs e)
        {
            await viewModel.GetAppointment(app_date, time);
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
                UpdateEvent(this, new UpdateDocEventArgs() { doctor=viewModel.doctor,page=this});
        }

        private void onDoctorReadSuccess(object source, EventArgs args)
        {
            Bindings.Update();
        }

        public void onDoctorRatingUpdateSucess(object source, EventArgs args)
        {

            onUpdate();
        }
        async void onCancelButtonClicked(object source, ButtonClickArgs args)
        {
            app_id = args.id_val;
            app_temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
            CancelDialog dialog = new CancelDialog();
            dialog.ButtonClicked += this.onYesButtonClicked;
            await dialog.ShowAsync();

        }
        public async void onYesButtonClicked(object source, EventArgs args)
        {
            viewModel.details.Remove(app_temp);
            await viewModel.CancelApp(app_id);
        }

        async void onResBtnClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            app_temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
            await viewModel.GetApp(id);

            //temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
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
        private async void TimeSlotBox_DropDownOpened(object sender, object e)
        {

            await viewModel.GetTimeSlots(id, hosp_id, app_date);
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
            BookButton.IsEnabled = (Appointment_Date.Date != null) && (TimeSlotBox.SelectedIndex != -1);
            Bindings.Update();
        }

        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedIndex == 1)
                await viewModel.GetTests(id);
            else if (Tabs.SelectedIndex == 2)
                await viewModel.GetAppointmentByDoc(1, id);
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


        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {

            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;

            Frame my_frame = grid.FindName("myFrame") as Frame;
            //my_frame.Navigate(typeof(HospitalDetailView), (e.ClickedItem as HospitalInDoctorDetails).Hosp_ID, 
            //    new SuppressNavigationTransitionInfo());
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            bool show = false;

            HyperlinkButton temp = sender as HyperlinkButton;
            if (!show)
            {
                temp.Visibility = Visibility.Collapsed;
                DescBox.TextWrapping = TextWrapping.WrapWholeWords;
                DescBox.Height = Double.NaN;
                DescBox.Width = 750;

            }


        }

        private void AppTabTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            var temp = sender as AppTabTemplate;
            
            temp.CancelButtonClicked += this.onCancelButtonClicked;
            temp.RescheduleButtonClicked += this.onResBtnClicked;
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
        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {

            if (sender.Value > 0)
            {

                await viewModel.UpdateDoctor(id, (double)sender.Value);

                //myListView.SelectedIndex = index;
                myRating.Caption = myRating.Value.ToString();

            }

        }
    }
}

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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Dashboard : Page,INavEvents
    {
        DashboardViewModel viewModel;
        MainPage mainPage;
        string address,time,app_date;
        int id,hosp_id,app_id;
        bool en;
        bool openflag = false;
        bool flag = false;
        AppointmentDetails app_temp;
        public delegate void PointerEventHandler(object source, EventArgs args);
        public event PointerEventHandler PointerEntered;
        public event PointerEventHandler PointerExited;
        SelectedDocDetailView view;
        public Dashboard()
        {
            this.InitializeComponent();
            mySplitView.IsPaneOpen = false;
            Appointment_Date.MinDate = DateTimeOffset.Now;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

            viewModel = new DashboardViewModel();
            var args = e1.Parameter as navargs;
            address = args.name;
            mainPage = args.mp;
            viewModel.DoctorReadSuccess += this.onDoctorReadSuccess;
            viewModel.InsertFail += this.onInsertFail;
            viewModel.InsertSuccess += this.onInsertSuccess;
            viewModel.AppointmentRead += this.onAppointmentRead;
            viewModel.DoctorRatingUpdateSuccess += this.onDoctorRatingUpdateSucess;
            viewModel.TestimonialAddedSuccess += this.onTestAddedSuccess;
            viewModel.LastHospBooked += this.onLastHospRead;
            viewModel.AppointmentCheckSuccess += this.onAppCheckSuccess;
            viewModel.AppRead += this.onAppReadSuccess;
            viewModel.AppointmentUpdated += this.onAppUpdateSuccess;
            viewModel.NoRecord += this.onNoAppointment;
            viewModel.NoDoctor += this.onNoDoctors;

            //await viewModel.GetRecentSearchDoctors(1);
            await viewModel.GetMostBookedDoc(1);
            await viewModel.GetAppointments(1);
            if (mainPage.ct > 1)
                ActivityText.Visibility = Visibility.Collapsed;
        }

        public void onPointerEntered()
        {
            if (PointerEntered != null)
                PointerEntered(this, EventArgs.Empty);
        }

        public void onPointerExited()
        {
            if (PointerExited != null)
                PointerExited(this, EventArgs.Empty);
        }

        public void onNoDoctors(object source, EventArgs args)
        {
            VisitedDocStack.Visibility = Visibility.Collapsed;
        }


        public void onButtonClicked(object source, ButtonClickArgs args)
        {
            //System.Diagnostics.Debug.WriteLine(args.model.Name);
            hosp_id = args.id_val;
            //pos = MyScroll.VerticalOffset;
            //MyScroll.ChangeView(null, 100, 1);
            LocationBox.Visibility = Visibility.Collapsed;
            Book_Pop.Visibility = Visibility.Visible;
            Book_Pop.IsOpen = true;
            //HospList.SelectedItem = ((FrameworkElement)source).DataContext;
        }

        public void onDoctorRatingUpdateSucess(object source, EventArgs args)
        {
            int x = -1;
            if(flag)
            {
                for (int i = 0; i < viewModel.most_booked_docs.Count; i++)
                    if (viewModel.most_booked_docs[i].ID == id)
                    {
                        x = i;
                        break;
                    }

                viewModel.most_booked_docs.Remove(viewModel.most_booked_docs[x]);
                viewModel.most_booked_docs.Insert(x, viewModel.doctor);
            }
            else
            {
                for (int i = 0; i < viewModel.recent_docs.Count; i++)
                    if (viewModel.recent_docs[i].ID == id)
                    {
                        x = i;
                        break;
                    }

                viewModel.recent_docs.Remove(viewModel.recent_docs[x]);
                viewModel.recent_docs.Insert(x, viewModel.doctor);
            }
        
            //myListView.SelectedItem = viewModel.doctor;
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

        public void onNoAppointment(object source, EventArgs args)
        {
            AppStack.Visibility = Visibility.Collapsed;
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
                System.Diagnostics.Debug.WriteLine("time=" + time);
                if (val.val == 0)
                {
                    AppointmentBookSuccess bookFail = new AppointmentBookSuccess()
                    {
                        Title = "Appointment Booking Failed",
                        Content = String.Format("No vacancies on {0} at {1}", app_date, time),
                        

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

        public void onLastHospRead(object source, EventArgs args)
        {
            //LastBookDialog dialog = new LastBookDialog();
            //dialog.YesButtonClicked += this.onYesButtonClicked;
            //dialog.NoButtonClicked += this.onNoButtonClicked;
            //hosp_id = viewModel.LastBookedHosp.ID;
            //dialog.txt = String.Format("Would you like to book an appointment at the last location ({0},{1})?",
            //    viewModel.LastBookedHosp.Name, viewModel.LastBookedHosp.Location);
            //await dialog.ShowAsync();
            for(int i=0;i<viewModel.hospitals.Count();i++)
                if(viewModel.hospitals[i].Hosp_ID==viewModel.LastBookedHosp.ID)
                {
                    LocationBox.SelectedIndex = i;
                    break;

                }
            //Book_Pop.IsOpen = true;
            //Book_Pop.Visibility = Visibility.Visible;
            //onYesButtonClicked(source, args);
        }

        public void onDoctorReadSuccess(object source, EventArgs args)
        {
            for (int i = 0; i < viewModel.hospitals.Count(); i++)
                if (viewModel.hospitals[i].Hosp_ID == viewModel.LastBookedHosp.ID)
                {
                    LocationBox.SelectedIndex = i;
                    break;

                }
            if(openflag)
            {
                Book_Pop.IsOpen = true;
                Book_Pop.Visibility = Visibility.Visible;
            }

        }

        public async void onYesButtonClicked(object source, EventArgs args)
        {
            viewModel.appointments.Remove(app_temp);
            await viewModel.CancelApp(app_id);
        }

        public async void onNoButtonClicked(object souce, EventArgs args)
        {
            await viewModel.GetDoctor(id);
            mySplitView.IsPaneOpen = false;
            mySplitView.IsPaneOpen = true;
            VisitedDocStack.SetValue(Grid.ColumnProperty, 0);
            VisitedDocStack.SetValue(Grid.ColumnSpanProperty, 2);
            //SearchPanel.Visibility = Visibility.Collapsed;
            AppStack.Visibility = Visibility.Collapsed;
            flag = true;
        }

        async void onCancelButtonClicked(object source, ButtonClickArgs args)
        {
            app_id = args.id_val;
            app_temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
            CancelDialog dialog = new CancelDialog();
            dialog.ButtonClicked += this.onYesButtonClicked;
            await dialog.ShowAsync();

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;

            Frame my_frame = grid.FindName("myFrame") as Frame;
            my_frame.Navigate(typeof(DoctorSearchResultView), new navargs() { mp = mainPage, name = address, doc=true });
        }
        
        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (Tabs.SelectedIndex == 1)
            //    await viewModel.GetTests(id);
        }

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Visibility = Visibility.Visible;
            //SubmitBtn.Visibility = Visibility.Visible;


        }

        private async void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            //await viewModel.AddTest(1, id, MessageBox.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //MessageBox.Text = "";
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

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            int index = temp.SelectedIndex;
            var doc = e.ClickedItem as Doctor;
            id = doc.ID;
            await viewModel.GetDoctor(id);
            mySplitView.IsPaneOpen = false;
            mySplitView.IsPaneOpen = true;
            //SearchPanel.SetValue(Grid.ColumnSpanProperty, 2);
            VisitedDocStack.Visibility = Visibility.Collapsed;
            AppStack.Visibility = Visibility.Collapsed;
            flag = false;
            openflag = false;
            
        }

        private async void MySplitView_PaneOpened(SplitView sender, object args)
        {
            //Showbutton.Visibility = Visibility.Collapsed;
            //myRating.Value = Double.NaN;
            //myRating.Caption = "Your Rating";
            //MessageBox.Visibility = Visibility.Collapsed;
            //SubmitBtn.Visibility = Visibility.Collapsed;
            if (viewModel.doctor != null)
            {
                System.Diagnostics.Debug.WriteLine("Count=" + viewModel.doctor.Description.Count(c => c.Equals('\n')));
                if (viewModel.doctor.Description.Count(c => c.Equals('\n')) >= 1)
                {
                    //DescBox.Height = 20;
                    //DescBox.Width = 550;
                    //Showbutton.Visibility = Visibility.Visible;
                }
                //else
                //    Showbutton.Visibility = Visibility.Collapsed;
            }
            await viewModel.GetTests(id);


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

        private async void BookButton_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.BookAppointment(1, id, hosp_id, app_date, time);
            LocationBox.Visibility = Visibility.Visible;
            Book_Pop.IsOpen = false;
            Book_Pop.Visibility = Visibility.Visible;
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

        private void HospitalsInDoctorsTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            HospitalsInDoctorsTemplate temp = (HospitalsInDoctorsTemplate)sender;
            temp.ButtonClicked += this.onButtonClicked;
        }

        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {

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

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListView).SelectedIndex;

        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            bool show = false;

            HyperlinkButton temp = sender as HyperlinkButton;
            if (!show)
            {
                temp.Visibility = Visibility.Collapsed;
                //DescBox.TextWrapping = TextWrapping.WrapWholeWords;
                //DescBox.Height = Double.NaN;
                //DescBox.Width = 750;

            }


        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {

            mySplitView.IsPaneOpen = false;
            VisitedDocStack.SetValue(Grid.ColumnProperty, 1);
            VisitedDocStack.SetValue(Grid.ColumnSpanProperty, 1);
            //SearchPanel.SetValue(Grid.ColumnSpanProperty, 1);
            //await viewModel.GetMostBookedDoc(1);
            //await viewModel.GetAppointments(1);
            //SearchPanel.Visibility = Visibility.Visible;
            VisitedDocStack.Visibility = Visibility.Visible;
            AppStack.Visibility = Visibility.Visible;
            
            Bindings.Update();
        }

        private async void BookedListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = sender as ListView;
            int index = temp.SelectedIndex;
            var doc = e.ClickedItem as Doctor;
            id = doc.ID;
            await viewModel.GetDoctor(id);
            mySplitView.IsPaneOpen = false;
            mySplitView.IsPaneOpen = true;
            VisitedDocStack.SetValue(Grid.ColumnProperty, 0);
            VisitedDocStack.SetValue(Grid.ColumnSpanProperty, 2);
            //SearchPanel.Visibility = Visibility.Collapsed;
            AppStack.Visibility = Visibility.Collapsed;
            myFrame.Navigate(typeof(SelectedDocDetailView),
                new DocNavEventArgs() { val = (e.ClickedItem as Doctor).ID, view = this }
            , new SuppressNavigationTransitionInfo());
            flag = true;
        }

        private void BookedDocTemp_Loaded(object sender, RoutedEventArgs e)
        {
            BookedDocTemplate temp = (BookedDocTemplate)sender;
            temp.ButtonClicked += this.onRebookButtonClicked;
        }

        public async void onRebookButtonClicked(object source, ButtonClickArgs args)
        {
            BookedListView.SelectedItem = ((FrameworkElement)source).DataContext;
            id = args.id_val;
            openflag = true;
            LocationBox.Visibility = Visibility.Visible;
            await viewModel.GetLastHospital(1, args.id_val);
            await viewModel.GetDoctor(args.id_val);
        }
        

        public async void onTestAddedSuccess(object source, EventArgs args)
        {
            await viewModel.GetLastTest(id);
            //TestScroll.ChangeView(0, 0, 1);
        }

        async void onResBtnClicked(object source, ButtonClickArgs args)
        {
            id = args.id_val;
            app_temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
            await viewModel.GetApp(id);

            //temp = ((FrameworkElement)source).DataContext as AppointmentDetails;
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

        private async void LocationBox_DropDownOpened(object sender, object e)
        {
            //await viewModel.GetDoctor(id);
            Bindings.Update();
        }

        private void LocationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (LocationBox.Visibility.Equals(Visibility.Visible))
                    hosp_id = (LocationBox.SelectedItem as HospitalInDoctorDetails).Hosp_ID;
            }
            catch(Exception _)
            {
                hosp_id = -1;
            }
            

        }

        private void DashboardAppTemplate_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            //onPointerEntered();
        }

        private void DashboardAppTemplate_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //onPointerExited();
        }

        private void DashboardAppTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            var temp = sender as DashboardAppTemplate;
            this.PointerEntered += temp.onPointerEntered;
            this.PointerExited += temp.onPointerExited;
            temp.CancelButtonClicked += this.onCancelButtonClicked;
            temp.RescheduleButtonClicked += this.onResBtnClicked;
            
        }

        private void Res_Pop_Opened(object sender, object e)
        {
            Res_TimeSlotBox.SelectedIndex = -1;
        }

        private void Res_Pop_Closed(object sender, object e)
        {

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

        private void ListView_GotFocus(object sender, RoutedEventArgs e)
        {
            //onPointerEntered();
        }
        public async void onInsertSuccess(object source, EventArgs args)
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
                
            };
            bookSuccess.ButtonClicked += this.onOKButtonClicked;
            AppStack.Visibility = Visibility.Visible;
            await bookSuccess.ShowAsync();
        }

        public async void onOKButtonClicked(object source,EventArgs args)
        {
            await viewModel.GetMostBookedDoc(1);
            await viewModel.GetAppointments(1);
            Bindings.Update();
        }

        public void onDoctorUpdateSuccess(object sender, UpdateDocEventArgs args)
        {
            view = args.page;
            var item = viewModel.most_booked_docs.FirstOrDefault(i => i.ID == args.doctor.ID);
            var index = viewModel.most_booked_docs.IndexOf(item);
            if (item != null)
                item = args.doctor;
            viewModel.most_booked_docs[index] = item;
            Bindings.Update();
        }
    }
}

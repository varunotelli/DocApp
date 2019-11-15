using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.DialogBoxes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
/*@todo Create profile page to track appointments
 *@todo Add reschedule and cancel functionality
 *@todo Fix Filter bugs
 *@todo Complete Dashboard 
 *@todo Add navigation to all listview items
 *@todo Complete Rebook functionality
 *@todo Fix rebook functionality 
 *@todo Add rebook for dashboard
 *@todo SplitView for Hospitals
 * @todo add back button for all pages
 * @todo make new page for hospital
 * @todo fix appointments with doctor tab
 * 
 * @todo change adaptive triggers for appointment
 * @body Horizontal Scroll instead of hiding cols
 * 
 * @todo Call Callbacks twice for search
 * @todo Change customizations like native for book buttons and listboxes
 * @todo Change column width of search bar
 * 
 * @todo Manage places without data with message
 */

namespace DocApp.Presentation.Views
{
    public class navargs1
    {
        public string name { get; set; }
        public string location { get; set; }
        public int dept_id { get; set; }
        public MainPage mp { get; set; }
    }


   public class navargs
    {
        public string name { get; set; }
        public bool location { get; set; }
        public int index { get; set; }
        public MainPage mp { get; set; }
        public bool doc { get; set; }
    }

    public class navargs2:EventArgs
    {
        public string location { get; set; }
        public MainPage mp { get; set; }
    }
   

    public sealed partial class MainPage : Page
    {
        
        AutoSuggestViewModel viewModel;
        bool locflag = false;
        public int ct = -1;
        string val = "Dash";
        public string address = "";
        public delegate void AutoSuggestChangedEventHandler(object source, navargs2 e);
        public event AutoSuggestChangedEventHandler AutoSuggestChanged;
        public delegate void LocationButtonClickedEventHandler(object source, navargs2 e);
        public event LocationButtonClickedEventHandler LocationButtonClicked;
        LocationRequestDialog dialog = new LocationRequestDialog();

        public MainPage()
        {

            this.InitializeComponent();
            
            viewModel = new AutoSuggestViewModel();
            viewModel.LocationChanged += this.onLocationChanged;
            viewModel.LoginEventSuccess += this.onLoginSuccess;
            viewModel.AddressFailed += this.onAddressFailed;
            progring.IsActive = true;
            MyAutoSuggest.IsEnabled = false;
            HospDocSuggest.IsEnabled = false;
            NavView.IsEnabled = false;
            NavView.IsPaneOpen = false;
            NavView.SelectedItem = NavView.MenuItems[0];
            //OpenBtn.IsEnabled = false;
            //MyListBox.SelectedIndex = 0;
            //Dashboardbtn.IsEnabled = false;
            //AppBtn.IsEnabled = false;
            NavView.Focus(FocusState.Programmatic);
            
            //myFrame.Navigate(typeof(MainPageLoadingScreenView));
        
        }

        public void onSeeAllClicked(object source, SeeAllClickedEventArgs args)
        {
            HospDocSuggest.IsFocusEngaged = false;
            myFrame.Navigate(typeof(DoctorSearchResultView), args.navs);
            if(args.navs.doc)
                NavView.SelectedItem = NavView.MenuItems[2];
            else NavView.SelectedItem = NavView.MenuItems[3];
        }

        public void onAutoSuggestChanged(string add)
        {
            viewModel.loc = add;
            if (AutoSuggestChanged != null)
                AutoSuggestChanged(this, new navargs2 { location = add, mp = this });
        }

        public async void onLoginSuccess(object source, LoginSuccessEventArgs args)
        {
            StatusText.Text = "READY";
            await Task.Delay(1000);
            progring.IsActive = false;
            progring.Visibility = Visibility.Collapsed;
            LoadText.Visibility = Visibility.Collapsed;
            MyAutoSuggest.IsEnabled = true;
            HospDocSuggest.IsEnabled = true;
            NavView.IsEnabled = true;
            //OpenBtn.IsEnabled = true;
            //AppBtn.IsEnabled = true;
            //Dashboardbtn.IsEnabled = true;
            StatusText.Visibility = Visibility.Collapsed;
            if (args.ct>1)
            {
                ct = args.ct;
                myFrame.Navigate(typeof(Dashboard), new navargs { name = viewModel.loc, location = true, mp = this },
                    new SuppressNavigationTransitionInfo());

            }
            else
            {
                //Dashboardbtn.Visibility = Visibility.Collapsed;
                myFrame.Navigate(typeof(MainPageBuffer), new navargs { name = viewModel.loc, location = true, mp = this }, 
                    new SuppressNavigationTransitionInfo());
                   
            }
        }

        public void onLocationButtonClicked(string add)
        {
            if (LocationButtonClicked != null)
                LocationButtonClicked(this, new navargs2 {location=add, mp = this });
        }

        public void onLocationChanged(object source, LocationEventArgs e)
        {
            address = e.address;
            LocationProg.IsActive = false;
            onLocationButtonClicked(address);
            //OpenBtn.IsEnabled = true;
            //locflag = true;
            
            //StatusText.Text = "READY";
            //await Task.Delay(1000);
            //progring.IsActive = false;
            //progring.Visibility = Visibility.Collapsed;
            //LoadText.Visibility = Visibility.Collapsed;
            //MyAutoSuggest.IsEnabled = true;
            //HospDocSuggest.IsEnabled = true;
            //AppBtn.IsEnabled = true;
            //StatusText.Visibility = Visibility.Collapsed;
                
           
            //if(HospDocSuggest.Text.Equals(""))
            //    myFrame.Navigate(typeof(MainPageBuffer), new navargs {name=address,location=true,mp=this });
        }


        public void onAddressFailed(object source,EventArgs args)
        {
            LocationProg.IsActive = false;
            InternetFailDialog dialog = new InternetFailDialog()
            {
                Title = "Connection Failed",
                Content = "Unable to connect to the internet. Please try again.",
                CloseButtonText = "OK"
            };
            dialog.ShowAsync();
            //await dialog.ShowAsync();
        }
        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    await viewModel.GetCurrentAddress();
            //}
            //catch(Exception _)
            //{
            //    CoreApplication.Exit();
            //}
            //address = e.address;
            locflag = true;

            //StatusText.Text = "READY";
            //await Task.Delay(1000);
            //progring.IsActive = false;
            //progring.Visibility = Visibility.Collapsed;
            //LoadText.Visibility = Visibility.Collapsed;
            //MyAutoSuggest.IsEnabled = true;
            //HospDocSuggest.IsEnabled = true;
            //AppBtn.IsEnabled = true;
            //StatusText.Visibility = Visibility.Collapsed;


            //if (HospDocSuggest.Text.Equals(""))
            //    myFrame.Navigate(typeof(MainPageBuffer), new navargs { name = viewModel.loc, location = true, mp = this });
            await viewModel.UserLogin();

        }

        

        private async void MyAutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            
            var autosuggestBox = (AutoSuggestBox)sender;
            await viewModel.GetLocalities(autosuggestBox.Text);
            
                
            var items = viewModel.localities;
            //var filtered = items.Where(p => p.StartsWith(autosuggestBox.Text.ToUpper())).Distinct()
            //    .ToArray();
            autosuggestBox.ItemsSource = items;
            
            
            viewModel.locbox = autosuggestBox.Text;
            if(autosuggestBox.Text.Equals(""))
                onAutoSuggestChanged(viewModel.loc);
            else
                onAutoSuggestChanged(autosuggestBox.Text);
            Bindings.Update();
            


        }

        private void HospDocSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            KeyWordBox.IsOpen = false;
            if(!sender.Text.Equals(""))
            {
                if(!MyAutoSuggest.Text.Equals(""))
                    myFrame.Navigate(typeof(HospitalDoctorView), new navargs1 { name=sender.Text,
                        location = MyAutoSuggest.Text.ToUpper(),
                   
                        mp =this }, new SuppressNavigationTransitionInfo());
                else
                    myFrame.Navigate(typeof(HospitalDoctorView), new navargs1
                    {
                        name = sender.Text,
                        location = viewModel.loc.ToUpper(),

                        mp = this
                    }, new SuppressNavigationTransitionInfo());
                NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
                NavView.IsBackEnabled = true;

            }
            else
            {
                if(ct>1)
                {
                    myFrame.Navigate(typeof(Dashboard), new navargs
                    {

                        name = viewModel.loc.ToUpper(),

                        mp = this
                    });
                    NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
                    NavView.IsBackEnabled = false;
                }
                else
                myFrame.Navigate(typeof(MainPageBuffer), new navargs
                {
                    
                    name = viewModel.loc.ToUpper(),

                    mp = this
                });
            }
                
        }

        private void MyAutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            if (myFrame.CanGoBack)
            {
                
                myFrame.GoBack(new SuppressNavigationTransitionInfo());
            }

        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (myFrame.CanGoForward)
                myFrame.GoForward();
        }

        private async void Comboboxitem_DropDownOpened(object sender, object e)
        {
            await viewModel.GetDepartments();
        }

        private void MyAutoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            onAutoSuggestChanged(args.SelectedItem as string);
        }

        private async void HospDocSuggest_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("GOT FOCUS");
            if (MyAutoSuggest.IsFocusEngaged == false)
            {
                KeyWordBox.IsOpen = true;
                HospDocSuggest.IsFocusEngaged = false;
            }
            //await viewModel.GetKeyWords();
            await viewModel.GetRecentSearchDoctors(1);
           
        }

        private void HospDocSuggest_LostFocus(object sender, RoutedEventArgs e)
        {
            KeyWordBox.IsOpen = false;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((sender as ListBox).SelectedIndex!=-1)
            {
                NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
                NavView.IsBackEnabled = true;
                myFrame.Navigate(typeof(SelectedDocDetailView),
                new DocNavEventArgs() { val = ((sender as ListBox).SelectedItem as Doctor).ID, vis=1 }
            , new SuppressNavigationTransitionInfo());
            }
            KeyWordBox.IsOpen = false;
            MyAutoSuggest.IsFocusEngaged = false;
        }

        private void AppBtn_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(AppointmentsDisplayView));
        }

        private void MyAutoSuggest_GotFocus(object sender, RoutedEventArgs e)
        {
            KeyWordBox.IsOpen = false;
        }

        private async void onYesClicked(object sender, EventArgs args)
        {
            await viewModel.GetCurrentAddress();
            LocationProg.IsActive = true;
            
        }

        private async void LocationBtn_Click(object sender, RoutedEventArgs e)
        {
            
            dialog.YesClicked += this.onYesClicked;
            await dialog.ShowAsync();
           
            
        }

        private void Dashboardbtn_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(Dashboard), new navargs
            {

                name = viewModel.loc.ToUpper(),
                mp = this
            });
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            //MySplitView_Main.IsPaneOpen = !MySplitView_Main.IsPaneOpen;
        }

        private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = sender as ListBox;
            int val = temp.SelectedIndex;

            if(val==0)
                myFrame.Navigate(typeof(Dashboard), new navargs
                {

                    name = viewModel.loc.ToUpper(),
                    mp = this
                });
            else if(val==1)
                myFrame.Navigate(typeof(AppointmentsDisplayView), new SuppressNavigationTransitionInfo());
            else if(val==2)
                myFrame.Navigate(typeof(DoctorSearchResultView), new navargs
                {
                    name = viewModel.loc,
                    mp = this,
                    index = 1,
                    doc = true

                }, new SuppressNavigationTransitionInfo());
            else if(val==3)
                myFrame.Navigate(typeof(DoctorSearchResultView), new navargs
                {
                    name = viewModel.loc,
                    mp = this,
                    index = 1,
                    doc = false

                }, new SuppressNavigationTransitionInfo());
        }

        private void LocationCheck_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var temp = sender as CheckBox;
            bool val = temp.IsChecked.HasValue ? temp.IsChecked.Value : false;
            if(val)
            {
                //locstack.Visibility = Visibility.Collapsed;
                locstack2.Visibility = Visibility.Visible;
            }
            else
            {
                locstack2.Visibility = Visibility.Collapsed;
                //locstack.Visibility = Visibility.Visible;
                viewModel.loc = "Chennai";
                onLocationButtonClicked("Chennai");
            }


        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            locmainstack.Visibility = Visibility.Visible;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            NavView.IsBackEnabled = false;
            string block = args.InvokedItemContainer.Tag as string;
            if (block != null)
            {
                if(!block.Equals(val))
                switch (block)
                {
                    case "Dash":
                        sender.Header = "Dashboard";
                        myFrame.Navigate(typeof(Dashboard), new navargs
                        {

                            name = viewModel.loc.ToUpper(),
                            mp = this
                        });
                        break;
                    case "App":
                        sender.Header = "Appointments";
                        myFrame.Navigate(typeof(AppointmentsDisplayView), new SuppressNavigationTransitionInfo());
                        break;
                    case "Doc":
                        sender.Header = "Doctors";
                        myFrame.Navigate(typeof(DoctorSearchResultView), new navargs
                        {
                            name = viewModel.loc,
                            mp = this,
                            index = 1,
                            doc = true

                        }, new SuppressNavigationTransitionInfo());
                        break;

                    case "Hosp":
                        sender.Header = "Hospitals";
                        myFrame.Navigate(typeof(DoctorSearchResultView), new navargs
                        {
                            name = viewModel.loc,
                            mp = this,
                            index = 1,
                            doc = false

                        }, new SuppressNavigationTransitionInfo());
                        break;



                }
            }
        }

        private void NavView_PaneClosed(NavigationView sender, object args)
        {
            locmainstack.Visibility = Visibility.Collapsed;
            //locmainstack.Opacity = 0;

            
        }

        private void NavView_PaneOpened(NavigationView sender, object args)
        {
            locmainstack.Visibility = Visibility.Visible;
            //locmainstack.Opacity = 1;
        }

        private void NavView_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            locmainstack.Visibility = Visibility.Collapsed;
            //locmainstack.Opacity = 0;
        }

        private void NavView_PaneOpening(NavigationView sender, object args)
        {
            locmainstack.Visibility = Visibility.Visible;
            //locmainstack.Opacity = 1;
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            val = args.SelectedItemContainer.Tag as string;
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            NavView.IsBackEnabled = false;
            if (myFrame.CanGoBack)
                myFrame.GoBack();
        }
    }
}

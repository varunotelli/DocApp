using DocApp.Presentation.ViewModels;
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

namespace DocApp.Presentation.Views
{
   public class navargs
    {
        public string name { get; set; }
        public bool location { get; set; }
    }
   

    public sealed partial class MainPage : Page
    {
        
        AutoSuggestViewModel viewModel;
        bool locflag = false;
        public string address = "";

        public MainPage()
        {

            this.InitializeComponent();
            
            viewModel = new AutoSuggestViewModel();
            //viewModel.LocationChanged += this.onLocationChanged;
            myFrame.Navigate(typeof(MainPageBuffer));
            




        }

        public void onLocationChanged(object source, LocationEventArgs e)
        {
            address = e.address;
            locflag = true;
            if(HospDocSuggest.Text.Equals(""))
                myFrame.Navigate(typeof(HospitalDoctorView), new navargs {name=address,location=true });
        }
       
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
           await viewModel.GetCurrentAddress();
           

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
            //if (!sender.Text.Equals(""))
            //    myFrame.Navigate(typeof(HospitalDoctorView), new navargs { name = sender.Text, location = true });
            //else
            //    myFrame.Navigate(typeof(HospitalDoctorView), new navargs { name = address, location = true });
            Bindings.Update();
            


        }

        private void HospDocSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(sender.Text!=null)
            {
                if(sender.Text.Equals(""))
                {
                    if(!locflag)
                        myFrame.Navigate(typeof(MainPageBuffer));
                    else
                        myFrame.Navigate(typeof(HospitalDoctorView), new navargs { name = address, location = true });
                }
                else
                    myFrame.Navigate(typeof(HospitalDoctorView), new navargs { name = sender.Text, location = false }, 
                        new SuppressNavigationTransitionInfo());
               

            }
                
        }

        private void MyAutoSuggest_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            if (myFrame.CanGoBack)
            {
                MyAutoSuggest.Text = "";
                HospDocSuggest.Text = "";
                myFrame.GoBack();
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
    }
}

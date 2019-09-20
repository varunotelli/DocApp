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
/*@todo Add click for listbox and redirect
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
        public MainPage mp { get; set; }
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
        public string address = "";
        public delegate void AutoSuggestChangedEventHandler(object source, navargs2 e);
        public event AutoSuggestChangedEventHandler AutoSuggestChanged;
        public MainPage()
        {

            this.InitializeComponent();
            
            viewModel = new AutoSuggestViewModel();
            viewModel.LocationChanged += this.onLocationChanged;
            //myFrame.Navigate(typeof(MainPageBuffer));
        
        }

        public void onAutoSuggestChanged(string add)
        {
            if (AutoSuggestChanged != null)
                AutoSuggestChanged(this, new navargs2 { location = add, mp = this });
        }

        public void onLocationChanged(object source, LocationEventArgs e)
        {
            address = e.address;
            locflag = true;
            if(HospDocSuggest.Text.Equals(""))
                myFrame.Navigate(typeof(MainPageBuffer), new navargs {name=address,location=true,mp=this });
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
            if(autosuggestBox.Text.Equals(""))
                onAutoSuggestChanged(viewModel.loc);
            else
                onAutoSuggestChanged(autosuggestBox.Text);
            Bindings.Update();
            


        }

        private void HospDocSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            KeyWordBox.IsOpen = false;
            if(sender.Text!=null)
            {
                if(!MyAutoSuggest.Text.Equals(""))
                    myFrame.Navigate(typeof(HospitalDoctorView), new navargs1 { name=sender.Text,
                        location = MyAutoSuggest.Text.ToUpper(),
                   
                        mp =this });
                else
                    myFrame.Navigate(typeof(HospitalDoctorView), new navargs1
                    {
                        name = sender.Text,
                        location = viewModel.loc.ToUpper(),

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

        private void MyAutoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            onAutoSuggestChanged(args.SelectedItem as string);
        }

        private async void HospDocSuggest_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("GOT FOCUS");
            KeyWordBox.IsOpen = true;
            await viewModel.GetKeyWords();
           
        }

        private void HospDocSuggest_LostFocus(object sender, RoutedEventArgs e)
        {
            KeyWordBox.IsOpen = false;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

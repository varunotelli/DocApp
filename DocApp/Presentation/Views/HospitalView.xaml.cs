using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DocApp.Presentation.ViewModels;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public class sendDataArgs : EventArgs
    {
        public string name { get; set; }
    }
    public sealed partial class HospitalView : Page
    {
        ObservableCollection<string> hospitals;
        ObservableCollection<string> doctors;
        HospitalDoctorViewModel viewModel;
        public delegate Task HospComboClickedEventHandler(object source, EventArgs e);
        public event HospComboClickedEventHandler hospComboClicked;
        public delegate Task DocComboClickedEventHandler(object source, EventArgs e);
        public event DocComboClickedEventHandler docComboClicked;
        public delegate void HospComboSelectionChangedEventHandler(object source, sendDataArgs e);
        public event HospComboSelectionChangedEventHandler hospChanged;
        

        public HospitalView()
        {
            
            this.InitializeComponent();
            hospitals = new ObservableCollection<string>();
            doctors = new ObservableCollection<string>();
            viewModel = new HospitalDoctorViewModel();
            
            this.hospComboClicked += viewModel.onHospComboClicked;
            this.docComboClicked += viewModel.ondocComboClicked;

            //this.hospChanged += mp.onHospChanged;
            this.DataContext = viewModel;
        
        }

        public async Task onHospComboClicked()
        {
            if (hospComboClicked != null)
                await hospComboClicked(this, EventArgs.Empty);
        }
        public async Task ondocComboClicked()
        {
            if (docComboClicked != null)
                await docComboClicked(this, EventArgs.Empty);
        }
        public void onHospChanged()
        {
            if (hospChanged != null)
                hospChanged(this, new sendDataArgs { name = HospCombo.SelectedItem as string });
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //var temp = await GetPosition();

            //await viewModel.GetHospitalsDoctors();
            //System.Diagnostics.Debug.WriteLine("viewlocation=" + viewModel.location);
            //return;
            //MyAutoSuggest.IsEnabled = false;
            //MyAutoSuggest.Focus(FocusState.Programmatic);
            //MyAutoSuggest.PlaceholderText = viewModel.location;
        }

        private void HospCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //onHospChanged();
            if(HospCombo.SelectedItem!=null)
            {
                Frame parentFrame = Window.Current.Content as Frame;

                MainPage mp = parentFrame.Content as MainPage;
                ScrollViewer grid = mp.Content as ScrollViewer;
                Frame my_frame = grid.FindName("myFrame") as Frame;
                System.Diagnostics.Debug.WriteLine("Selected=" + HospCombo.SelectedItem);
                my_frame.Navigate(typeof(HospitalDetailView), HospCombo.SelectedItem);
                HospCombo.SelectedItem = null;
            }

        }

        private void DocCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DocCombo.SelectedItem!=null)
            {
                Frame parentFrame = Window.Current.Content as Frame;

                MainPage mp = parentFrame.Content as MainPage;
                ScrollViewer grid = mp.Content as ScrollViewer;
                Frame my_frame = grid.FindName("myFrame") as Frame;
                System.Diagnostics.Debug.WriteLine("Selected=" + DocCombo.SelectedItem);
                my_frame.Navigate(typeof(DoctorDetailView), DocCombo.SelectedItem);
                DocCombo.SelectedItem = null;
            }
        }

        private async void DocCombo_DropDownOpened(object sender, object e)
        {
            await ondocComboClicked();
            
            //doctors.Clear();
            foreach (var h in viewModel.doctors)
                doctors.Add(h.Name);
            
            Bindings.Update();
            

        }



        private async void HospCombo_DropDownOpened(object sender, object e)
        {
            //hospitals.Clear();
            await onHospComboClicked();
            
            foreach (var h in viewModel.hospitals)
                hospitals.Add(h.Name);
           
            Bindings.Update();
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //await viewModel.GetHospitalsDoctors();
            //while (viewModel.location.Equals("")) ;
            //MyAutoSuggest.PlaceholderText = viewModel.loc;
            //Bindings.Update();
            
            await viewModel.GetHospitalsDoctors();
            
        }

        private async void MyAutoSuggest_Loaded(object sender, RoutedEventArgs e)
        {
         
            //await onHospComboClicked();
            //hospitals.Clear();
            //foreach (var h in viewModel.hospitals)
            //    hospitals.Add(h.Name);
        }

        private async void MyAutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var autosuggestBox=(AutoSuggestBox)sender;
            var items = viewModel.hospitals.Select(x => x.Location);
            var filtered = items.Where(p => p.StartsWith(autosuggestBox.Text.ToUpper())).Distinct()
                .ToArray();
            autosuggestBox.ItemsSource = filtered;
            viewModel.locbox = autosuggestBox.Text;
            await viewModel.GetHospitalsDoctors();
            hospitals.Clear();
            foreach (var h in viewModel.hospitals)
                hospitals.Add(h.Name);
            doctors.Clear();
            foreach (var h in viewModel.doctors)
                doctors.Add(h.Name);
            Bindings.Update();



        }

        

        
    }
}

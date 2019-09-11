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
using DocApp.Models;
using DocApp.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
/**
 * @todo Add rating for hospital
 * @body Create a new usecase for updationg of ratings for hospital.
 */ 
namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HospitalDetailView : Page
    {
        
        string name = "";
        ObservableCollection<Doctor> docs = new ObservableCollection<Doctor>();
        public delegate void ListViewItemSelectedEventHandler(object source, EventArgs e);
        public event ListViewItemSelectedEventHandler ListViewItemSelected;
        public HospitalDetailViewModel viewModel;
        
        public HospitalDetailView()
        {
            this.InitializeComponent();
            //DoctorProfile.BackButtonClicked += this.onBackButtonClicked;
            //DoctorProfile.ProfileButtonClicked += this.onProfileButtonClicked;
            
            
        }

        public void OnListViewItemSelected()
        {
            if (ListViewItemSelected != null)
                ListViewItemSelected(this, EventArgs.Empty);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            var hosp = e1.Parameter as Hospital;
            viewModel = new HospitalDetailViewModel(hosp.ID);
            //await viewModel.
            return;
          
        }
        

        
    }
}

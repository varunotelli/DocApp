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
        
        
        ObservableCollection<Doctor> docs = new ObservableCollection<Doctor>();
        public delegate void ListViewItemSelectedEventHandler(object source, EventArgs e);
        public event ListViewItemSelectedEventHandler ListViewItemSelected;
        public HospitalDetailViewModel viewModel;
        int hosp;
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
            hosp =(int) e1.Parameter;
            viewModel = new HospitalDetailViewModel();
            await viewModel.GetHospital(hosp);
            //Bindings.Update();
            return;
          
        }

        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(DoctorDetailView), 
                (e.ClickedItem as DoctorInHospitalDetails).doc_id);
        }

        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {
            if (sender.Value > 0)
            {
                Bindings.Update();
                await viewModel.UpdateHospitalRating(hosp, (double)sender.Value);

                Bindings.Update();
                myRating.Caption = myRating.Value.ToString();



            }
        }
    }
}

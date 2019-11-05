using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Views.Templates;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /*
     * @todo Implement booking functionality
     * 
     */

    public class GridViewClickedEventArgs:EventArgs
    {
        public int ID { get; set; }
    }
    public sealed partial class DoctorDetailView : Page
    {
        public delegate void GridViewItemSelectedEventHandler(object source, EventArgs e);
        public event GridViewItemSelectedEventHandler GridViewItemSelected;
        DoctorDetailViewModel viewModel;
        int id;
        
        public DoctorDetailView()
        {
            this.InitializeComponent();
            
        }

        public void onButtonClicked(object source, ButtonClickArgs args)
        {

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

            var doc =(int) e1.Parameter;
            id = doc;
            viewModel = new DoctorDetailViewModel();
            await viewModel.GetDoctor(doc);
            System.Diagnostics.Debug.WriteLine(viewModel.doctor.Name);
            return;



        }
        public void OnGridViewItemSelected(int id)
        {
            if (GridViewItemSelected != null)
                GridViewItemSelected(this, new GridViewClickedEventArgs {ID=id});
        }

        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var x = e.ClickedItem as HospitalInDoctorDetails;
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(HospitalDetailView), (e.ClickedItem as HospitalInDoctorDetails).Hosp_ID, new SuppressNavigationTransitionInfo());
        }

        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {
            
            if (sender.Value > 0)
            {
                //Bindings.Update();
                //await viewModel.UpdateDoctor(id, (double)sender.Value);

                //Bindings.Update();
                //myRating.Caption = myRating.Value.ToString();
            
            }
            
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            Frame my_frame = grid.FindName("myFrame") as Frame;

            my_frame.Navigate(typeof(AppointmentBookingView), id, new SuppressNavigationTransitionInfo());
        }
    }
}

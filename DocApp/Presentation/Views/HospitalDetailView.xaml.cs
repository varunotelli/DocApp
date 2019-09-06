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
            DoctorProfile.BackButtonClicked += this.onBackButtonClicked;
            
            
        }

        public void OnListViewItemSelected()
        {
            if (ListViewItemSelected != null)
                ListViewItemSelected(this, EventArgs.Empty);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            System.Diagnostics.Debug.WriteLine("Sent val="+(string)e1.Parameter);
            name = (string)e1.Parameter;
            //HospitalDetailsTemplate h = new HospitalDetailsTemplate();
            viewModel = new HospitalDetailViewModel(name);
            var frame = (Frame)Window.Current.Content;
            var page = (MainPage)frame.Content;
            this.ListViewItemSelected += page.OnListViewItemSelected;
            DoctorProfile.BackButtonClicked += page.OnBackButtonClicked;
            await viewModel.GetHospitals();
            
            return;
            
            

        }
        public void onBackButtonClicked(object source, EventArgs e)
        {
            mySplitView.IsPaneOpen = false;
        }
       

        private async void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            OnListViewItemSelected();
            System.Diagnostics.Debug.WriteLine("Item clicked");
           //mySplitView.IsPaneOpen =false;
            System.Diagnostics.Debug.WriteLine(((DoctorInHospitalDetails)e.ClickedItem).Name);
            await viewModel.GetDoctorDetails(((DoctorInHospitalDetails)e.ClickedItem).Name);
           DoctorProfile.DataContext = viewModel.doc;
            try
            {
                if (viewModel.doc != null && viewModel.doc.Name.Equals(((DoctorInHospitalDetails)e.ClickedItem).Name))
                    mySplitView.IsPaneOpen = true;
                else
                {
                    await viewModel.GetDoctorDetails(((DoctorInHospitalDetails)e.ClickedItem).Name);
                    DoctorProfile.DataContext = viewModel.doc;
                    Bindings.Update();
                    mySplitView.IsPaneOpen = true;
                    //mySplitView.IsPaneOpen = false;

                }
            }
            catch(Exception _)
            {
                await viewModel.GetDoctorDetails(((DoctorInHospitalDetails)e.ClickedItem).Name);
                DoctorProfile.DataContext = viewModel.doc;
                Bindings.Update();
                mySplitView.IsPaneOpen = true;
            }
            
            //mySplitView.IsPaneOpen = true;
            //myScroll.ChangeView(0.0f, 0.0f, 1.0f);

        }
    }
}

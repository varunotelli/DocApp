using DocApp.Presentation.ViewModels;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /*
     * @todo Complete Doctor Detail View
     * @body Create new use case to get hospitals in which doctor works and fix UI
     */
    public sealed partial class DoctorDetailView : Page
    {
        public delegate void ListViewItemSelectedEventHandler(object source, EventArgs e);
        public event ListViewItemSelectedEventHandler ListViewItemSelected;
        DoctorDetailViewModel viewModel;
        string name = "";
        public DoctorDetailView()
        {
            this.InitializeComponent();
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            System.Diagnostics.Debug.WriteLine("Sent val=" + (string)e1.Parameter);
            name = (string)e1.Parameter;
            //HospitalDetailsTemplate h = new HospitalDetailsTemplate();
            viewModel = new DoctorDetailViewModel(name);
            var frame = (Frame)Window.Current.Content;
            var page = (MainPage)frame.Content;
            this.ListViewItemSelected += page.OnListViewItemSelected;
            await viewModel.GetDoctor();
            
            return;



        }
        public void OnListViewItemSelected()
        {
            if (ListViewItemSelected != null)
                ListViewItemSelected(this, EventArgs.Empty);
        }

        private void HospList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OnListViewItemSelected();
        }

        private async void MyRating_ValueChanged(RatingControl sender, object args)
        {
            Bindings.Update();
            if (myRating.Value > 0)
            {
                myRating.Caption = myRating.Value.ToString();
                await viewModel.UpdateDoctor(name, myRating.Value);
                Bindings.Update();
            }
        }
    }
}

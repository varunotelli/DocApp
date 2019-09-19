using DocApp.Models;
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
     * @todo Make new query to get doctors by location,department
     * 
     */

     public class DocSendEventArgs: EventArgs
     {
        public Doctor doc { get; set; }
        public DoctorSearchResultView view { get; set; }

     }

    public sealed partial class DoctorSearchResultView : Page
    {
        //public delegate void GridItemClickedEventHandler(object sender, DocSendEventArgs args);
        //public event GridItemClickedEventHandler GridItemClicked;

        public DoctorSearchViewModel viewModel;
        string address = "";
        MainPage mainPage;
        public DoctorSearchResultView()
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            viewModel = new DoctorSearchViewModel();
            var args = e1.Parameter as navargs;
            address = args.name;
            mainPage = args.mp;
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            await viewModel.GetDoctors(address);
            
           
            await viewModel.GetDepartments();
            
            //Bindings.Update();

        }

        //public void onGridItemClicked(Doctor d)
        //{
        //    if (GridItemClicked != null)
        //        GridItemClicked(this, new DocSendEventArgs { doc = d, view = this });
        //}

        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            address = n.location;
            if (DeptListbox.SelectedItem != null)
                await viewModel.GetDoctorsByDept(address, DeptListbox.SelectedIndex);
            else
                await viewModel.GetDoctors(address);
        }

        //private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //Bindings.Update();

        //    var temp = sender as HyperlinkButton;
        //    System.Diagnostics.Debug.WriteLine("Content="+(temp.Content as TextBlock).Text);
        //    await viewModel.GetDoctorsByDept(address, (temp.Content as TextBlock).Text);
        //    //await viewModel.GetDoctorsByDept(address, (temp.Content as TextBlock).Text);
        //    System.Diagnostics.Debug.WriteLine("Doc dept val=" + viewModel.docs.Count);
        //    Bindings.Update();
        //}

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            var select = listbox.SelectedIndex;
            await viewModel.GetDoctorsByDept(address, select);
            Bindings.Update();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Doctor d = e.ClickedItem as Doctor;
            
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp1 = parentFrame.Content as MainPage;
            StackPanel grid = mp1.Content as StackPanel;
           
            Frame my_frame = grid.FindName("myFrame") as Frame;
            my_frame.Navigate(typeof(DoctorDetailView), e.ClickedItem as Doctor);
        }
    }
}

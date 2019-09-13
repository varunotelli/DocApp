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
    public sealed partial class DoctorSearchResultView : Page
    {

        public DoctorSearchViewModel viewModel;
        string address = "";
        public DoctorSearchResultView()
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            viewModel = new DoctorSearchViewModel();
            address = e1.Parameter as string;
            await viewModel.GetDoctors(address);
            
           
            await viewModel.GetDepartments();
            
            //Bindings.Update();

        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            //Bindings.Update();

            var temp = sender as HyperlinkButton;
            System.Diagnostics.Debug.WriteLine("Content="+(temp.Content as TextBlock).Text);
            await viewModel.GetDoctorsByDept(address, (temp.Content as TextBlock).Text);
            //await viewModel.GetDoctorsByDept(address, (temp.Content as TextBlock).Text);
            System.Diagnostics.Debug.WriteLine("Doc dept val=" + viewModel.docs.Count);
            Bindings.Update();
        }
    }
}

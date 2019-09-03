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
    public sealed partial class DoctorDetailView : Page
    {
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
            await viewModel.GetDoctor();
            
            return;



        }
    }
}

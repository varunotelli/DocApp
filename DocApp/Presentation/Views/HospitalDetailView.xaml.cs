﻿using System;
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

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HospitalDetailView : Page
    {
        
        string name = "";
        ObservableCollection<Doctor> docs = new ObservableCollection<Doctor>();
        public HospitalDetailViewModel viewModel;// = new HospitalDetailViewModel()
        public HospitalDetailView()
        {
            this.InitializeComponent();
            //HospitalControl = new HospitalDetailsTemplate();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Sent val="+(string)e.Parameter);
            name = (string)e.Parameter;
            //HospitalDetailsTemplate h = new HospitalDetailsTemplate();
            viewModel = new HospitalDetailViewModel(name);
            await viewModel.GetHospitals();

            //System.Diagnostics.Debug.WriteLine(viewModel.Doctors[0].Name);
            return;
            
            

        }

       
    }
}

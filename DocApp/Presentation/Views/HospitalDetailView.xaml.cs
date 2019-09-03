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
/**
 * @todo Add SplitView for Doctor in Hospital
 * @body Create a new split view for each doctor and display full profile in panel using doc_id of listview clicked item.
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
        public HospitalDetailViewModel viewModel;// = new HospitalDetailViewModel()
        public HospitalDetailView()
        {
            this.InitializeComponent();
            
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            System.Diagnostics.Debug.WriteLine("Sent val="+(string)e1.Parameter);
            name = (string)e1.Parameter;
            //HospitalDetailsTemplate h = new HospitalDetailsTemplate();
            viewModel = new HospitalDetailViewModel(name);
            await viewModel.GetHospitals();
            //this.DataContextChanged += (s, e) => Bindings.Update();
            //myTemplate.DataContext = viewModel.Doctors.First();


            //System.Diagnostics.Debug.WriteLine(viewModel.doc[0].Name);
            return;
            
            

        }

       
    }
}

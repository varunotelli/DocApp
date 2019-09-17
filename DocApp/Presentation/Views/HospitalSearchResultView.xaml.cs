﻿using DocApp.Presentation.ViewModels;
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
    public sealed partial class HospitalSearchResultView : Page
    {
        public HospitalSearchViewModel viewModel;
        string address = "";
        MainPage mainPage;

        public HospitalSearchResultView()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {
            viewModel = new HospitalSearchViewModel();
            var args = e1.Parameter as navargs;
            address = args.name;
            mainPage = args.mp;
            mainPage.AutoSuggestChanged += this.onAutoSuggestChanged;
            await viewModel.GetHospitals(address);


            await viewModel.GetDepartments();

            //Bindings.Update();

        }
        public async void onAutoSuggestChanged(object sender, navargs2 n)
        {
            address = n.location;
            if (DeptListbox.SelectedItem != null)
                await viewModel.GetHospitalByDept(address.ToUpper(), DeptListbox.SelectedIndex);
            else
                await viewModel.GetHospitals(address.ToUpper());
            Bindings.Update();
        }

        private async void DeptListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            int index = listbox.SelectedIndex;
            await viewModel.GetHospitalByDept(address, index+1);
            Bindings.Update();
            
        }
    }
}

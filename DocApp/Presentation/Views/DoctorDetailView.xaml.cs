﻿using DocApp.Models;
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
        int id;
        public DoctorDetailView()
        {
            this.InitializeComponent();
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e1)
        {

            var doc = e1.Parameter as Doctor;
            viewModel = new DoctorDetailViewModel();
            await viewModel.GetDoctor(doc.ID);
            System.Diagnostics.Debug.WriteLine(viewModel.doctor.Name);
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
            
            if (sender.Value!=null && sender.Value > 0)
            {
                Bindings.Update();
                //await viewModel.UpdateDoctor(name, (double)sender.Value);

                Bindings.Update();
                myRating.Caption = myRating.Value.ToString();
                


            }
            
        }
    }
}

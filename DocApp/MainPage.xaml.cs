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
using DocApp.Presentation.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DocApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HospitalView hospitalView;
        public MainPage()
        {
            this.InitializeComponent();
            //hospitalView = new HospitalView();
            //hospitalView.hospChanged += this.onHospChanged;
            myFrame.Navigate(typeof(HospitalView));
        }

        public void onHospChanged(object sender, sendDataArgs e)
        {
            System.Diagnostics.Debug.WriteLine("EVENT CAUGHT");
            myFrame.Navigate(typeof(HospitalDetailView), e.name);
        }
    }
}

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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DocApp.Presentation.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
   
    public sealed partial class MainPageBuffer : Page
    {
        //public delegate void GridViewItemSelectedEventHandler(object source, GridViewSelectedArgs e);
        //public event GridViewItemSelectedEventHandler GridViewItemSelected;
        public MainPageBuffer()
        {
            this.InitializeComponent();
        }
       


        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var griditem = sender as GridView;
            Frame parentFrame = Window.Current.Content as Frame;

            MainPage mp = parentFrame.Content as MainPage;
            StackPanel grid = mp.Content as StackPanel;
            AutoSuggestBox autoSuggestBox = grid.FindName("MyAutoSuggest") as AutoSuggestBox;
            Frame my_frame = grid.FindName("myFrame") as Frame;
            
            if (griditem.SelectedIndex == 0)
            {
                my_frame.Navigate(typeof(DoctorSearchResultView),autoSuggestBox.PlaceholderText);
                
            }

            
        }
    }
}

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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DocApp.Presentation.Views.Controls
{
    public class ComboBoxSelectEventArgs:EventArgs
    {
        public int val { get; set; }
    }
    public sealed partial class ComboBoxControl : UserControl
    {

        public delegate void ComboChangedEvent(object source, ComboBoxSelectEventArgs args);
        public event ComboChangedEvent ComboSelectionChanged; 
        public ComboBoxControl()
        {
            this.InitializeComponent();
        }

        public void onComboChanged()
        {
            if (ComboSelectionChanged != null)
                ComboSelectionChanged(this, new ComboBoxSelectEventArgs { val = myCombo.SelectedIndex });
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            onComboChanged();
        }
    }
}

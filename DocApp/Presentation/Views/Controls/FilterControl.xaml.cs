using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class FilterControl : UserControl
    {
        public ObservableCollection<string> deptnames;
        public delegate void FilterEventHandler(object source, FilterEventArgs args);
        public event FilterEventHandler DeptListChanged;
        public event FilterEventHandler RatingChanged;
        public event FilterEventHandler ExpChanged;
        public event FilterEventHandler RatingCleared;
        public event FilterEventHandler ExpCleared;
        int lexp = -1, uexp = 200, rating = -1;
        public FilterControl()
        {
            this.InitializeComponent();
            this.deptnames = new ObservableCollection<string>();
            this.DataContextChanged += (s, e) => Bindings.Update();

        }
        public void onDeptListChanged(int l, int u, int r, string s = "CARDIOLOGY")
        {
            if (DeptListChanged != null)
                DeptListChanged(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex, deptname = s });
        }


        public void onExpChanged(int l, int u, int r)
        {
            if (ExpChanged != null)
                ExpChanged(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onExpCleared(int l, int u, int r)
        {
            if (ExpCleared != null)
                ExpCleared(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onRatingChanged(int l, int u, int r)
        {
            if (RatingChanged != null)
                RatingChanged(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        public void onRatingCleared(int l, int u, int r)
        {
            if (RatingCleared != null)
                RatingCleared(this, new FilterEventArgs() { lower = l, upper = u, rat = r, deptindex = DeptListbox.SelectedIndex });
        }

        private void ExpClearBtn_Click(object sender, RoutedEventArgs e)
        {
            TenyearExp.IsChecked = false;
            FiveYearExp.IsChecked = false;
            OneYearExp.IsChecked = false;
            lexp = -1;
            uexp = 200;
            onExpCleared(lexp, uexp, rating);

        }

        private void RatingClearBtn_Click(object sender, RoutedEventArgs e)
        {
            RatingListBox.SelectedIndex = -1;
            rating = -1;
            onRatingCleared(lexp, uexp, rating);



        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            DeptListbox.SelectedIndex = 0;
            RatingClearBtn_Click(null, null);
            ExpClearBtn_Click(null, null);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            var select = listbox.SelectedIndex;
            string s = listbox.SelectedItem as string;
            onDeptListChanged(lexp, uexp, rating, s);
        }

        private void TenyearExp_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                //mySplitView.IsPaneOpen = false;

                string exp = rb.Content.ToString();
                
                switch (exp)
                {
                    case "> 10 year exp":
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => Int32.Parse(d.Experience.Split(' ')[0]) > 10))
                        //    viewModel.docsmain.Add(i);
                        lexp = 11;
                        uexp = 200;

                        break;
                    case "5-10 year exp":
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => Int32.Parse(d.Experience.Split(' ')[0]) <= 10 &&
                        //    Int32.Parse(d.Experience.Split(' ')[0]) >= 5))
                        //    viewModel.docsmain.Add(i);
                        lexp = 5;
                        uexp = 10;


                        break;
                    case "< 5 year exp":
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => Int32.Parse(d.Experience.Split(' ')[0]) < 5))
                        //    viewModel.docsmain.Add(i);
                        uexp = 4;
                        lexp = -1;
                        break;

                }
                
                onExpChanged(lexp, uexp, rating);



                //lexp = -1;
                //uexp = 200;
                //rating = -1;

            }
            else
            {
                //foreach (var i in viewModel.docs)
                //    viewModel.docsmain.Add(i);
            }

        }

        private void RatingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedIndex != -1)
            {
                //mySplitView.IsPaneOpen = false;
                
                switch (lb.SelectedIndex + 1)
                {
                    case 1:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 4))
                        //    viewModel.docsmain.Add(i);
                        rating = 4;
                        break;
                    case 2:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 3))
                        //    viewModel.docsmain.Add(i);
                        rating = 3;
                        break;
                    case 3:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 2))
                        //    viewModel.docsmain.Add(i);
                        rating = 2;
                        break;
                    case 4:
                        //viewModel.docsmain.Clear();
                        //foreach (var i in doctemp.Where(d => d.Rating >= 1))
                        //    viewModel.docsmain.Add(i);
                        rating = 1;
                        break;

                }
                //if (MainTabs.SelectedIndex == 0)
                
                    onRatingChanged(lexp, uexp, rating);
                

                

                //await viewModel.GetDoctorsByDept(address,.SelectedIndex, lexp, uexp, rating);
                //lexp = -1;
                //uexp = 200;
                //rating = -1;
            }

        }

    }
}

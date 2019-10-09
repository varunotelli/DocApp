using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DocApp.Presentation.Views.Templates
{
    public sealed partial class DoctorProfileLargeTemplate : UserControl
    {
        public Models.Doctor doctor { get { return this.DataContext as Models.Doctor; } }
        public BitmapImage imagesource { get; set; }
        public DoctorProfileLargeTemplate()
        {
            this.InitializeComponent();
            //if(doctor!=null)
            //    Img.Source = new BitmapImage( new Uri (String.Format("ms-appx:///Assets/Doc{0}.jpeg", doctor.ID)));
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //imagesource = new BitmapImage(new Uri(String.Format("ms-appx:///Assets/Doc{0}.jpg", doctor.ID), UriKind.Absolute));
        }

        private void Img_Loaded(object sender, RoutedEventArgs e)
        {
            imagesource = new BitmapImage(new Uri(doctor.Image, UriKind.Absolute));
            Img.Source = imagesource;
        }

        public ImageSource getSource(Models.Doctor d)
        {
            Image imgtemp = new Image();
            if (d != null)
                imagesource = new BitmapImage(new Uri(String.Format("ms-appx:///Assets/Doc{0}.jpg", d.ID), UriKind.Absolute));
            else
                imagesource = new BitmapImage();
            imgtemp.Source = imagesource;

            return imgtemp.Source;
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            imagesource = new BitmapImage(new Uri(String.Format("ms-appx:///Assets/Doc{0}.jpg", doctor.ID), UriKind.Absolute));
            Img.Source = imagesource;
        }
    }
}

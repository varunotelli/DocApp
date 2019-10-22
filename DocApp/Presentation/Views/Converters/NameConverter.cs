using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DocApp.Presentation.Views.Converters
{
    public class NameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "Dr. " + (value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

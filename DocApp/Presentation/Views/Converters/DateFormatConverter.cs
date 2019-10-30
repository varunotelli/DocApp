using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DocApp.Presentation.Views.Converters
{
    public class DateFormatConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return DateTime.ParseExact(value as string, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

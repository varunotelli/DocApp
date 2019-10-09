using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DocApp.Presentation.Views.Converters
{
    public class RelativeTimeConverter:IValueConverter
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int WEEK= 7 * DAY;
        const int MONTH = 30 * DAY;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime CurrentTime = DateTime.ParseExact(value as string, "yyyy-MM-dd HH:mm:ss", null);
            var ts = new TimeSpan(DateTime.Now.Ticks - CurrentTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return Math.Abs(ts.Seconds) <= 1 ? "Just Now" : Math.Abs(ts.Seconds) + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return Math.Abs(ts.Minutes) + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return Math.Abs(ts.Hours) + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            

            if (delta < 7 * DAY)
                return Math.Abs(ts.Days) + " days ago";

            if(delta<30*DAY)
            {
                int weeks = (int)Math.Floor((double)ts.Days / 7);
                return weeks <= 1 ? "1 week ago" : weeks + " weeks ago";
            }

            if (delta < 12 * MONTH)
            {
                int months = (int)Math.Floor((double)ts.Days / 30);
                return months <= 1 ? "One month ago" : months + " months ago";
            }
            else
            {
                int years = (int)Math.Floor((double)ts.Days / 365);
                return years <= 1 ? "One year ago" : years + " years ago";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Views.ViewInterfaces
{
    public interface IFilter
    {
        void onDeptListChanged(object source, FilterEventArgs args);
        void onExpListChanged(object source, FilterEventArgs args);
        void onExpCleared(object source, FilterEventArgs args);
        void onRatingChanged(object source, FilterEventArgs args);
        void onRatingCleared(object source, FilterEventArgs args);
        void onComboChanged(object source, OrderEventArgs args);
        void onLocationButtonClicked(object source, navargs2 n);
        void onAutoSuggestChanged(object sender, navargs2 n);


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Views
{
    public interface INavEvents
    {
        void onDoctorUpdateSuccess(object sender, UpdateDocEventArgs args);
        void onListClicked(object source, EventArgs args);
    }
}

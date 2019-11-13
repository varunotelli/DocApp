using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Views.ViewInterfaces
{
    public interface IHospEvents
    {
        void onHospitalUpdateSuccess(object sender, UpdateHospEventArgs args);
    }
}

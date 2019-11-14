using DocApp.Presentation.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Views.ViewInterfaces
{
    public interface IFrames
    {
        void onComboChanged(object source, ComboBoxSelectEventArgs args);
        void onDoctorsSuccess(object source, EventArgs args);
    }
}

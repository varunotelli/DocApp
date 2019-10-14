using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Callbacks
{
    public interface ICheckAppointmentViewCallback
    {
        bool CheckAppointmentViewSuccess(int count);
        bool CheckAppointmentViewFail();
    }
}

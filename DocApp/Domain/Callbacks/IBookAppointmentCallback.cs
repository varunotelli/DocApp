using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.Callbacks
{
    public interface IBookAppointmentCallback
    {
        bool AppReadSuccess(Appointment appointment);
        bool AppReadFail();
    }
}

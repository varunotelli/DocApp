using DocApp.Domain.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface IAppointmentDetails
    {
        Task GetAppointment(int id, IAppointmentListCallback callback);
        Task GetAppointmentByID(string app_date, string time, IAppointmentCallback callback);
    }
}

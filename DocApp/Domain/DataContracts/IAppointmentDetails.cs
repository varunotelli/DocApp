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
        Task GetUpcomingApps(int p_id, IUpcomingAppCallback callback);
        Task GetAppointmentByDoc(int p_id, int doc_id,IAppByDocCallback callback);
    }
}

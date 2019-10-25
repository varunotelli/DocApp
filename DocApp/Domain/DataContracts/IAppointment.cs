using DocApp.Domain.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface IAppointment
    {
        Task AddAppointment(int p_id,int doc_id,int hosp_id,string app_date, string start, IBookAppointmentCallback callback);
        Task CheckAppointment(int p_id, string app_date, string time, ICheckAppointmentCallback callback);
        Task CancelAppointment(int id, ICancelAppCallback callback);
        Task UpdateAppointment(int id, string app_date, string time, IUpdateAppCallback callback);
        Task GetAppointmentByID(int id, IAppCallback callback);
    }
}

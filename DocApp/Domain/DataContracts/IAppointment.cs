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
        Task AddAppointment(int p_id,int doc_id,int hosp_id,string app_date, string start, IAppointmentCallback callback);
    }
}

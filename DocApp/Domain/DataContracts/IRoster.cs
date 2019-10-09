using DocApp.Domain.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface IRoster
    {
        Task GetTimeSlots(int doc_id, int hosp_id, string app_date,IRosterCallback callback);
    }
}

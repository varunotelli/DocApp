using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Data
{
    public class AppointmentDetailsDAO:IAppointmentDetails 
    {
        public async Task GetAppointment(int p_id, IAppointmentListCallback callback)
        {
            List<AppointmentDetails> results = new List<AppointmentDetails>();
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                var apps = await DBHandler.db.Table<Appointment>().ToListAsync();
                var docs = await DBHandler.db.Table<Doctor>().ToListAsync();
                var hosp = await DBHandler.db.Table<Hospital>().ToListAsync();
                var details = (from a in apps
                               join d in docs
                               on a.DOC_ID equals d.ID
                               join h in hosp
                               on a.HOS_ID equals h.ID
                               where apps.Any(g=>g.PATIENT_ID.Equals(p_id))
                               select new AppointmentDetails
                               {
                                   app_date = a.APP_DATE,
                                   doc_name=d.Name,
                                   hosp_name=h.Name,
                                   id=a.ID,
                                   location=h.Location,
                                   Timeslot=a.start_time

                               }
                    ).OrderBy(x=>x.app_date);
                foreach (var x in details)
                {
                    x.app_date = DateTime.ParseExact(x.app_date, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
                    results.Add(x);
                }
                if (results != null)
                    callback.GetAppSuccess(results);
                else
                    callback.GetAppFail();


            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Appointment details select exception="+e.Message);
            }

        }
    }
}

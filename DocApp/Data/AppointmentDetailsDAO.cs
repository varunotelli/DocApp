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
                               where apps.Any(g => g.PATIENT_ID.Equals(p_id))
                               select new AppointmentDetails
                               {
                                   app_date = a.APP_DATE,
                                   doc_name = d.Name,
                                   hosp_name = h.Name,
                                   id = a.ID,
                                   location = h.Location,
                                   Timeslot = a.start_time

                               }
                    ).OrderBy(x => x.app_date).ThenBy(x => x.Timeslot);
                foreach (var x in details.Where(a=>DateTime.Parse(a.app_date)==
                DateTime.Parse(DateTime.Now.Date.ToString("yyyy-MM-dd")) && DateTime.Parse(a.Timeslot).CompareTo(
                    DateTime.Parse(DateTime.Now.TimeOfDay.ToString())) > 0

                ))
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
        public async Task GetAppointmentByID(string app_date,string time, IAppointmentCallback callback)
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
                               where a.APP_DATE.Equals(app_date) && a.start_time.Equals(time)
                               select new AppointmentDetails
                               {
                                   app_date = a.APP_DATE,
                                   doc_name = d.Name,
                                   hosp_name = h.Name,
                                   id = a.ID,
                                   location = h.Location,
                                   Timeslot = a.start_time

                               }
                    );
                foreach (var x in details)
                {
                    x.app_date = DateTime.ParseExact(x.app_date, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
                    results.Add(x);
                }
                if (results != null)
                    callback.GetAppByIDSuccess(results.First());
                else
                    callback.GetAppByIDFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Appointment details select exception=" + e.Message);
            }

        }

        public async Task GetUpcomingApps(int p_id,IUpcomingAppCallback callback)
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
                               where apps.Any(g => g.PATIENT_ID.Equals(p_id))
                               select new AppointmentDetails
                               {
                                   app_date = a.APP_DATE,
                                   doc_name = d.Name,
                                   hosp_name = h.Name,
                                   id = a.ID,
                                   location = h.Location,
                                   Timeslot = a.start_time

                               }
                    ).OrderBy(x => x.app_date).ThenBy(x => x.Timeslot);
                foreach (var x in details.Where(a => DateTime.Parse(a.app_date) >=
                DateTime.Parse(DateTime.Now.Date.ToString("yyyy-MM-dd")) 

                ))
                {
                    if (DateTime.Parse(x.app_date) == DateTime.Parse(DateTime.Now.Date.ToString("yyyy-MM-dd")))
                    {
                        if (DateTime.Parse(x.Timeslot).CompareTo(DateTime.Parse(DateTime.Now.TimeOfDay.ToString())) < 0)
                            continue;
                        
                    }
                    x.app_date = DateTime.ParseExact(x.app_date, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
                    results.Add(x);
                }
                if (results != null)
                    callback.UpcomingAppSuccess(results);
                else
                    callback.UpcomingAppFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Appointment details select exception=" + e.Message);
            }
        }
    }
}

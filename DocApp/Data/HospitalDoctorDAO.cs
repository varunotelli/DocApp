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
    public class HospitalDoctorDAO : IHospitalInDoctorList
    {
        public async Task GetHospitalByDoctor(string docname, IHospitalDoctorCallback HospitalDoctorCallBack)
        {
            List<HospitalInDoctorDetails> results = new
                List<HospitalInDoctorDetails>();

            try
            {

                //var db = await dbHandler.DBConnection();
                DoctorDBHandler.DBConnection();
                var docs = await DoctorDBHandler.db.Table<Doctor>().ToListAsync();
                var roster = await DoctorDBHandler.db.Table<Roster>().ToListAsync();
                var hosp = await DoctorDBHandler.db.Table<Hospital>().ToListAsync();
                var details = (from h in hosp
                               join r in roster
                               on h.ID equals r.hosp_id
                               where docs.Any(d => d.Name.Equals(docname) && d.ID == r.doc_id)
                               select new HospitalInDoctorDetails
                               {
                                   
                                   Name = h.Name,
                                   Location=h.Location,
                                   Start_Time = r.start_time.Split(':')[0] + ":" + r.start_time.Split(':')[1],
                                   End_Time = r.end_time.Split(':')[0] + ":" + r.end_time.Split(':')[1],
                                   Fees = r.fee,
                                   Rating = h.Rating,
                                   Number_of_Rating = h.Number_Of_Rating
                                   


                               }
                    );

                foreach (var x in details)
                    results.Add(x);

                if (results != null && results.Count() > 0)
                {
                    HospitalDoctorCallBack.ReadSuccess(results);
                    await DoctorDBHandler.db.CloseAsync();
                }

                else HospitalDoctorCallBack.ReadFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SELECT EXCEPTION" + e.Message);
            }
        }
    }
}

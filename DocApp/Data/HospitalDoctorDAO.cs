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
        public async Task GetHospitalByDoctor(int id, IHospitalDoctorCallback HospitalDoctorCallBack)
        {
            List<HospitalInDoctorDetails> results = new
                List<HospitalInDoctorDetails>();

            try
            {

                //var db = await dbHandler.DBConnection();
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                var docs = await DBHandler.db.Table<Doctor>().ToListAsync();
                var roster = await DBHandler.db.Table<Roster>().ToListAsync();
                var hosp = await DBHandler.db.Table<Hospital>().ToListAsync();
                var details = (from h in hosp
                               join r in roster
                               on h.ID equals r.hosp_id
                               where docs.Any(d => d.ID.Equals(id) && d.ID == r.doc_id)
                               select new HospitalInDoctorDetails
                               {
                                   Hosp_ID=h.ID,
                                   Name = h.Name,
                                   Location=h.Location,
                                   Start_Time = r.start_time.Split(':')[0] + ":" + r.start_time.Split(':')[1],
                                   End_Time = r.end_time.Split(':')[0] + ":" + r.end_time.Split(':')[1],
                                   Fees = r.fee,
                                   Rating = h.Rating,
                                   Number_of_Rating = h.Number_Of_Rating
                                   


                               }
                    ).Distinct();

                foreach (var x in details)
                    results.Add(x);


                if (results != null)
                {
                    results = results.GroupBy(h => h.Hosp_ID).Select(g => g.FirstOrDefault()).ToList();
                    HospitalDoctorCallBack.ReadSuccess(results);
                    await DBHandler.db.CloseAsync();
                }

                else HospitalDoctorCallBack.ReadFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("HOSPITAL DOCTOR SELECT EXCEPTION" + e.Message);
            }
        }
    }
}

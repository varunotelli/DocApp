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
    public class HospitalDoctorDAO: IDoctorInHospitalList
    {
        public async Task GetDoctorDetailByHospital(string hospname, IDoctorHospitalCallBack doctorHospitalCallBack)
        {
            List<DoctorInHospitalDetails> results = new 
                List<DoctorInHospitalDetails>();

            try
            {

                //var db = await dbHandler.DBConnection();
                DoctorDBHandler.DBConnection();
                var docs =await DoctorDBHandler.db.Table<Doctor>().ToListAsync();
                var roster=await DoctorDBHandler.db.Table<Roster>().ToListAsync();
                var hosp = await DoctorDBHandler.db.Table<Hospital>().ToListAsync();
                var details = (from d in docs
                               join r in roster
                               on d.ID equals r.doc_id
                               where hosp.Any(h=>h.Name.Equals(hospname) && h.ID == r.hosp_id)
                               select new DoctorInHospitalDetails { Name=d.Name,
                               Designation=d.Designation,
                               Experience=d.Experience,
                               Start_Time=r.start_time,
                               End_Time=r.end_time,
                               fees=r.fee,
                               max_patients=r.max_patients
                              
                               }
                    ) ;
                
                foreach (var x in details)
                    results.Add(x);
                //System.Diagnostics.Debug.WriteLine("QUERY COUNT=" + results[0].fees);
                if (results != null && results.Count() > 0)
                    doctorHospitalCallBack.ReadSuccess(results);
                else doctorHospitalCallBack.ReadFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SELECT EXCEPTION" + e.Message);
            }
        }
    }
}

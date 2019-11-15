﻿using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Data
{
    public class DoctorHospitalDAO : IDoctorInHospitalList
    {
        public async Task GetDoctorDetailByHospital(int hospid, IDoctorHospitalCallBack doctorHospitalCallBack)
        {
            List<DoctorInHospitalDetails> results = new
                List<DoctorInHospitalDetails>();

            try
            {

                //var db = await dbHandler.DBConnection();
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                var docs = await DBHandler.db.Table<Doctor>().ToListAsync();
                var roster = await DBHandler.db.Table<Roster>().ToListAsync();
                var hosp = await DBHandler.db.Table<Hospital>().ToListAsync();
                var details = (from d in docs
                               join r in roster
                               on d.ID equals r.doc_id
                               where hosp.Any(h => h.ID.Equals(hospid) && h.ID == r.hosp_id)
                               select new DoctorInHospitalDetails
                               {
                                   doc_id = d.ID,
                                   Name = d.Name,
                                   Designation = d.Designation,
                                   Experience = d.Experience,
                                   Start_Time = r.start_time,
                                   End_Time = r.end_time,
                                   fees = r.fee,
                                   Rating = d.Rating,
                                   Number_of_Rating = d.Number_of_Rating,
                                   max_patients = r.max_patients,
                                   Image = d.Image


                               }
                    );

                results = details.ToList();
                //System.Diagnostics.Debug.WriteLine("QUERY COUNT=" + results[0].fees);
                if (results != null)
                {
                    results = results.GroupBy(d => d.doc_id).Select(g => g.FirstOrDefault()).ToList();
                    doctorHospitalCallBack.ReadSuccess(results);
                    await DBHandler.db.CloseAsync();
                }
                else doctorHospitalCallBack.ReadFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DOCTOR HOSPITAL SELECT EXCEPTION" + e.Message);
            }
        }
    }
}

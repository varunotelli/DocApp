using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Domain;
using DocApp.Models;
using SQLite;
using SQLitePCL;
using DocApp.Domain.DataContracts;
using DocApp.Domain.Callbacks;

namespace DocApp.Data
{
    class HospitalListDAO: IHospitalList
    {
        public async Task GetHospitalsAsync(IHospitalCallback hospitalCallback)
        {
            List<Hospital> results= new List<Hospital>();
            
            try
            {
                
                //var db = await dbHandler.DBConnection();
                DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Hospital>(
                    "SELECT * FROM HOSPITAL ");
                System.Diagnostics.Debug.WriteLine("results="+results.Count());
                if (results != null && results.Count > 0)
                {
                    hospitalCallback.ReadSuccess(results);
                    await DBHandler.db.CloseAsync();
                }
                else
                    hospitalCallback.ReadFail();

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SELECT EXCEPTION" + e.Message);
            }
            
            
        }
        public async Task GetHospitalByNameAsync(string name, IHospitalCallback hospitalCallback)
        {
            DBHandler.DBConnection();
            var results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL " +
                "WHERE NAME LIKE'{0}%'",name));
            if (results != null && results.Count > 0)
            {
                hospitalCallback.ReadSuccess(results);
                //await DoctorDBHandler.db.CloseAsync();
            }
            else
                hospitalCallback.ReadFail();
            System.Diagnostics.Debug.WriteLine("hosp dao val=" + results[0].Number_Of_Rating);

        }

        public async Task GetHospitalByLocationAsync(string name, IHospitalCallback hospitalCallback)
        {
            DBHandler.DBConnection();
            var results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL " +
                "WHERE LOCATION='{0}'", name));
            if (results != null && results.Count > 0)
            {
                hospitalCallback.ReadSuccess(results);
                //await DoctorDBHandler.db.CloseAsync();
            }
            else
                hospitalCallback.ReadFail();

        }

        public async Task GetHospitalbyIdAsync(int id, IHospitalCallback hospitalCallback)
        {
            List<Hospital> results = new List<Hospital>();

            try
            {

                //var db = await dbHandler.DBConnection();
                DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL {0}",id)
                    );
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null && results.Count > 0)
                {
                    hospitalCallback.ReadSuccess(results);
                    //await DoctorDBHandler.db.CloseAsync();
                }
                else
                    hospitalCallback.ReadFail();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SELECT EXCEPTION" + e.Message);
            }

        }


    }
}


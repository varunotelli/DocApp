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
        public async Task GetHospitalsAsync(IHospitalListCallback hospitalCallback)
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
        public async Task GetHospitalByNameAsync(string name, string location, IHospitalListCallback hospitalCallback)
        {
            DBHandler.DBConnection();
            var results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL " +
                "WHERE NAME LIKE'{0}%' AND LOCATION='{1}'",name,location));
            if (results != null && results.Count > 0)
            {
                hospitalCallback.ReadSuccess(results);
                //await DoctorDBHandler.db.CloseAsync();
            }
            else
                hospitalCallback.ReadFail();
            System.Diagnostics.Debug.WriteLine("hosp dao val=" + results[0].Number_Of_Rating);

        }

        public async Task GetHospitalByLocationAsync(string name, IHospitalListCallback hospitalCallback)
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

        public async Task GetHospitalbyIdAsync(int id, IHospitalDetailCallback hospitalCallback)
        {
            List<Hospital> results = new List<Hospital>();

            try
            {

                //var db = await dbHandler.DBConnection();
                DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL where ID ={0}",id)
                    );
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null)
                {
                    hospitalCallback.HospitalDetailReadSuccess(results.First());
                    //await DoctorDBHandler.db.CloseAsync();
                }
                else
                    hospitalCallback.HospitalDetailReadFail();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SELECT EXCEPTION" + e.Message);
            }

        }

        public async Task GetHospitalByDept(string location,int dept_id, IHospitalListCallback hospitalCallback)
        {
            DBHandler.DBConnection();
            List<Hospital> results = new List<Hospital>();

            try
            {

                //var db = await dbHandler.DBConnection();
                DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL WHERE ID IN(" +
                    "SELECT HOSP_ID FROM ROSTER GROUP BY DOC_ID HAVING DOC_ID IN(" +
                    "SELECT ID FROM DOCTOR WHERE DEPT_ID={0})" +
                    "AND COUNT(*)>0) AND LOCATION='{1}'", dept_id,location)
                    );
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null)
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

        public async Task UpdateHospitalRating(int id, double rating, IHospitalUpdateCallback hospitalCallback)
        {

            DBHandler.DBConnection();
            try
            {

                await DBHandler.db.ExecuteAsync(String.Format("UPDATE HOSPITAL SET RATING=(((RATING*NUMBER_OF_RATING)+{0})/(NUMBER_OF_RATING+1))" +
                "WHERE ID={1}", rating, id)
                );
                System.Diagnostics.Debug.WriteLine("Update rating dao Success");
                await DBHandler.db.CloseAsync();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Update rating dao exception=" + e.Message);
                hospitalCallback.HospitalUpdateFail();

            }
            try
            {
                DBHandler.DBConnection();
                await DBHandler.db.ExecuteAsync(String.Format("UPDATE HOSPITAL SET NUMBER_OF_RATING=NUMBER_OF_RATING+1 " +
                    "WHERE ID={0}", id)
                );
                System.Diagnostics.Debug.WriteLine("Update number of rating dao Success");
                await DBHandler.db.CloseAsync();
            }
            catch (Exception e)

            {
                System.Diagnostics.Debug.WriteLine("Update number of rating dao exception=" + e.Message);
                hospitalCallback.HospitalUpdateFail();
            }
            try
            {
                DBHandler.DBConnection();
                var results = await DBHandler.db.QueryAsync<Hospital>(String.Format("SELECT * FROM HOSPITAL " +
                "WHERE ID={0}", id));

                if (results != null)
                {
                    System.Diagnostics.Debug.WriteLine("Update Select dao Success");
                    System.Diagnostics.Debug.WriteLine("Update Select rating=" + results[0].Number_Of_Rating);
                    hospitalCallback.HospitalUpdateSuccess(results.First());
                    await DBHandler.db.CloseAsync();
                }
                else
                    hospitalCallback.HospitalUpdateFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Update Select dao exception=" + e.Message);
                hospitalCallback.HospitalUpdateFail();
            }
            //await DoctorDBHandler.db.CloseAsync();
        }


    }
}


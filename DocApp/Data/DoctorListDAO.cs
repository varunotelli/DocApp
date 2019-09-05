using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Models;
using DocApp.Data;
using SQLite;
using SQLitePCL;
using DocApp.Domain.DataContracts;
using DocApp.Domain.Callbacks;

namespace DocApp.Data
{
    public class DoctorListDAO: IDoctorList
    {
        //IDoctorCallback doctorCallback;
        public async Task GetDoctorsAsync(IDoctorCallback doctorCallback)
        {
            List<Doctor> results = new List<Doctor>();

            try
            {

                //var db = await dbHandler.DBConnection();
                DoctorDBHandler.DBConnection();
                results = await DoctorDBHandler.db.QueryAsync<Doctor>(
                    "SELECT * FROM DOCTOR ");
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null)
                {
                    doctorCallback.ReadSuccess(results);
                    await DoctorDBHandler.db.CloseAsync();
                }
                else
                    doctorCallback.ReadFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SELECT EXCEPTION" + e.Message);
            }
            

        }
        public async Task GetDoctorByNameAsync( string name, IDoctorCallback doctorCallback)
        {
            DoctorDBHandler.DBConnection();
            var results = await DoctorDBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE NAME='{0}'", name));
           //await DoctorDBHandler.db.CloseAsync();
            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                await DoctorDBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            
            
        }

        public async Task GetDoctorByHospitalNameAsync(string name, IDoctorCallback doctorCallback)
        {
            DoctorDBHandler.DBConnection();
            List<Doctor> results = await DoctorDBHandler.
                db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN (" +
                "SELECT ID FROM HOSPITAL WHERE NAME='{0}'))", name));
            System.Diagnostics.Debug.WriteLine("Hosp name " + results.Count());

            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                await DoctorDBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            
        }
        public async Task GetDoctorByHospitalLocationAsync(string name, IDoctorCallback doctorCallback)
        {
            DoctorDBHandler.DBConnection();
            List<Doctor> results = await DoctorDBHandler.
                db.QueryAsync<Doctor>(String.Format("SELECT DISTINCT * FROM DOCTOR WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN (" +
                "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}'))", name));
            System.Diagnostics.Debug.WriteLine("Hosp name " + results.Count());

            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                await DoctorDBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            //await DoctorDBHandler.db.CloseAsync();

        }

        public async Task UpdateDoctorRating(string name, double rating, IDoctorUpdateCallback doctorCallback)
        {
            
            DoctorDBHandler.DBConnection();
            try
            {

                await DoctorDBHandler.db.ExecuteAsync(String.Format("UPDATE DOCTOR SET RATING=(((RATING*NUMBER_OF_RATING)+{0})/(NUMBER_OF_RATING+1))" +
                "WHERE NAME='{1}'", rating, name)
                );
                System.Diagnostics.Debug.WriteLine("Update rating dao Success");
                await DoctorDBHandler.db.CloseAsync();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Update rating dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();

            }
            try
            {
                DoctorDBHandler.DBConnection();
                await DoctorDBHandler.db.ExecuteAsync(String.Format("UPDATE DOCTOR SET NUMBER_OF_RATING=NUMBER_OF_RATING+1 " +
                    "WHERE NAME='{0}'", name)
                );
                System.Diagnostics.Debug.WriteLine("Update number of rating dao Success");
                await DoctorDBHandler.db.CloseAsync();
            }
            catch(Exception e)

            {
                System.Diagnostics.Debug.WriteLine("Update number of rating dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();
            }
            try
            {
                DoctorDBHandler.DBConnection();
                var results = await DoctorDBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE NAME='{0}'", name));
                
                if (results != null)
                {
                    System.Diagnostics.Debug.WriteLine("Update Select dao Success");
                    doctorCallback.DoctorUpdateSuccess(results.First());
                    //await DoctorDBHandler.db.CloseAsync();
                }
                else
                    doctorCallback.DoctorUpdateFail();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Update Select dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();
            }
            //await DoctorDBHandler.db.CloseAsync();
        }
                
            
            
            
        
    }
}

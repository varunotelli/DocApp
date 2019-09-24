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
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Doctor>(
                    "SELECT * FROM DOCTOR ");
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null)
                {
                    doctorCallback.ReadSuccess(results);
                    await DBHandler.db.CloseAsync();
                }
                else
                    doctorCallback.ReadFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DOCTOR ASYNC SELECT EXCEPTION" + e.Message);
            }
            

        }
        public async Task GetDoctorByNameAsync( string name, string location,  IDoctorCallback callback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            List<Doctor> results = new List<Doctor>();
            try
            {
                results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE NAME LIKE '{1}%' AND ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN(" +
                "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}')" +
                ")" , location,  name));
                System.Diagnostics.Debug.WriteLine("Results val=" + results.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Dept loc select exception " + e.Message);
            }
            if (results != null)
                callback.ReadSuccess(results);
            else
                callback.ReadFail();


        }

        public async Task GetDoctorByHospitalNameAsync(string name, IDoctorCallback doctorCallback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            List<Doctor> results = await DBHandler.
                db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN (" +
                "SELECT ID FROM HOSPITAL WHERE NAME='{0}'))", name));
            System.Diagnostics.Debug.WriteLine("Hosp name " + results.Count());

            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                await DBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            
        }
        public async Task GetDoctorByHospitalLocationAsync(string name, IDoctorCallback doctorCallback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            List<Doctor> results = await DBHandler.
                db.QueryAsync<Doctor>(String.Format("SELECT DISTINCT * FROM DOCTOR WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN (" +
                "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}'))", name.ToUpper()));
            System.Diagnostics.Debug.WriteLine("Hosp name " + results.Count());

            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                await DBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            //await DoctorDBHandler.db.CloseAsync();

        }

        public async Task UpdateDoctorRating(int id, double rating, IDoctorUpdateCallback doctorCallback)
        {

            if (DBHandler.db == null)
                DBHandler.DBConnection();
            try
            {

                await DBHandler.db.ExecuteAsync(String.Format("UPDATE DOCTOR SET RATING=(((RATING*NUMBER_OF_RATING)+{0})/(NUMBER_OF_RATING+1))" +
                "WHERE ID={1}", rating, id)
                );
                System.Diagnostics.Debug.WriteLine("Update rating dao Success");
                await DBHandler.db.CloseAsync();

            }
            catch (Exception e)
            {
               // System.Diagnostics.Debug.WriteLine("Update rating dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();

            }
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                await DBHandler.db.ExecuteAsync(String.Format("UPDATE DOCTOR SET NUMBER_OF_RATING=NUMBER_OF_RATING+1 " +
                    "WHERE ID={0}", id)
                );
                System.Diagnostics.Debug.WriteLine("Update number of rating dao Success");
                await DBHandler.db.CloseAsync();
            }
            catch(Exception e)

            {
                //System.Diagnostics.Debug.WriteLine("Update number of rating dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();
            }
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                var results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE ID={0}", id));

                if (results != null)
                {
                    System.Diagnostics.Debug.WriteLine("Update Select dao Success");
                    System.Diagnostics.Debug.WriteLine("Update Select rating="+results[0].Number_of_Rating);
                    doctorCallback.DoctorUpdateSuccess(results.First());
                    await DBHandler.db.CloseAsync();
                }
                else
                    doctorCallback.DoctorUpdateFail();
            }
            catch(Exception e)
            {
                //System.Diagnostics.Debug.WriteLine("Update Select dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();
            }
            //await DoctorDBHandler.db.CloseAsync();
        }

        public async Task GetDoctorByIdAsync(int id, IDoctorDetailCallback doctorCallback)
        {
            //Doctor d = new Doctor();
            try
            {

                //var db = await dbHandler.DBConnection();
                if(DBHandler.db==null)
                    DBHandler.DBConnection();
                List<Doctor> results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR WHERE ID={0}",id));
                
                
                if (results != null)
                {
                    doctorCallback.DoctorDetailReadSuccess(results.First());
                    await DBHandler.db.CloseAsync();
                }
                else
                    doctorCallback.DoctorDetailReadFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DOC BY ID SELECT EXCEPTION" + e.Message);
            }



        }

        public async Task GetDoctorByDeptLocationAsync(string location,int dept,IDoctorCallback doctorCallback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            List<Doctor> results = new List<Doctor>();
            try
            {
                results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN(" +
                "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}')" +
                ")" +
                "AND DEPT_ID ={1}", location, dept));
                System.Diagnostics.Debug.WriteLine("Results val=" + results.Count);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Dept loc select exception " + e.Message);
            }
            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                await DBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            
        }

        public async Task GetDoctorsByDeptCount(int dept,int hosp_id, IDoctorCountByDeptCallback callback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            var count = await DBHandler.db.ExecuteScalarAsync<int>(String.Format("select count(*) from roster group by DOC_ID " +
                "having DOC_ID in " +
                "(select id from DOCTOR where DEPT_ID={0}) " +
                "and HOSP_ID={1}",dept,hosp_id)
                );
            if (count > 0)
                callback.ReadCountSuccess(count);
            else callback.ReadCountFail();        }

        public Task GetDoctorCountAsync(int dept, int hosp_id, IDoctorCountByDeptCallback callback)
        {
            throw new NotImplementedException();
        }

        //public async Task SearchDoctor(string name, string location, int dept, IDoctorCallback callback)
        //{
        //    DBHandler.DBConnection();
        //    List<Doctor> results = new List<Doctor>();
        //    try
        //    {
        //        results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
        //        "WHERE NAME LIKE '{2}%' AND ID IN (" +
        //        "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN(" +
        //        "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}')" +
        //        ")" +
        //        "AND DEPT_ID ={1}", location, dept, name));
        //        System.Diagnostics.Debug.WriteLine("Results val=" + results.Count);
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Dept loc select exception " + e.Message);
        //    }
        //    if (results != null)
        //        callback.ReadSuccess(results);
        //    else
        //        callback.ReadFail();

        //}
    }
}

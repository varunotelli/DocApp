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
        public async Task GetDoctorByNameAsync( string name, IDoctorCallback doctorCallback)
        {
            DBHandler.DBConnection();
            var results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE NAME LIKE '{0}%' AND NAME NOT NULL", name));
           //await DoctorDBHandler.db.CloseAsync();
            if (results != null && results.Count>0)
            {
                doctorCallback.ReadSuccess(results);
                //await DoctorDBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            
            
        }

        public async Task GetDoctorByHospitalNameAsync(string name, IDoctorCallback doctorCallback)
        {
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
            DBHandler.DBConnection();
            List<Doctor> results = await DBHandler.
                db.QueryAsync<Doctor>(String.Format("SELECT DISTINCT * FROM DOCTOR WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN (" +
                "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}'))", name.ToUpper()));
            System.Diagnostics.Debug.WriteLine("Hosp name " + results.Count());

            if (results != null)
            {
                doctorCallback.ReadSuccess(results);
                //await DoctorDBHandler.db.CloseAsync();
            }
            else
                doctorCallback.ReadFail();
            //await DoctorDBHandler.db.CloseAsync();

        }

        public async Task UpdateDoctorRating(string name, double rating, IDoctorUpdateCallback doctorCallback)
        {
            
            DBHandler.DBConnection();
            try
            {

                await DBHandler.db.ExecuteAsync(String.Format("UPDATE DOCTOR SET RATING=(((RATING*NUMBER_OF_RATING)+{0})/(NUMBER_OF_RATING+1))" +
                "WHERE NAME='{1}'", rating, name)
                );
                System.Diagnostics.Debug.WriteLine("Update rating dao Success");
                await DBHandler.db.CloseAsync();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Update rating dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();

            }
            try
            {
                DBHandler.DBConnection();
                await DBHandler.db.ExecuteAsync(String.Format("UPDATE DOCTOR SET NUMBER_OF_RATING=NUMBER_OF_RATING+1 " +
                    "WHERE NAME='{0}'", name)
                );
                System.Diagnostics.Debug.WriteLine("Update number of rating dao Success");
                await DBHandler.db.CloseAsync();
            }
            catch(Exception e)

            {
                System.Diagnostics.Debug.WriteLine("Update number of rating dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();
            }
            try
            {
                DBHandler.DBConnection();
                var results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE NAME LIKE '{0}%'", name));

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
                System.Diagnostics.Debug.WriteLine("Update Select dao exception=" + e.Message);
                doctorCallback.DoctorUpdateFail();
            }
            //await DoctorDBHandler.db.CloseAsync();
        }

        public async Task GetDoctorByIdAsync(int id, IDoctorCallback doctorCallback)
        {
            //Doctor d = new Doctor();
            try
            {

                //var db = await dbHandler.DBConnection();
                DBHandler.DBConnection();
                List<Doctor> results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR WHERE ID={0}",id));
                
                
                if (results != null)
                {
                    doctorCallback.ReadSuccess(results);
                    //await DoctorDBHandler.db.CloseAsync();
                }
                else
                    doctorCallback.ReadFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DOC BY ID SELECT EXCEPTION" + e.Message);
            }



        }

        public async Task GetDoctorByDeptLocationAsync(string location,string dept,IDoctorCallback doctorCallback)
        {
            DBHandler.DBConnection();
            List<Doctor> results = new List<Doctor>();
            try
            {
                results = await DBHandler.db.QueryAsync<Doctor>(String.Format("SELECT * FROM DOCTOR " +
                "WHERE ID IN (" +
                "SELECT DOC_ID FROM ROSTER WHERE HOSP_ID IN(" +
                "SELECT ID FROM HOSPITAL WHERE LOCATION='{0}')" +
                ")" +
                "AND DEPT_ID IN (" +
                " SELECT ID FROM DEPARTMENT WHERE NAME='{1}')", location, dept));
                System.Diagnostics.Debug.WriteLine("Results val=" + results.Count);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Dept loc select exception " + e.Message);
            }
            if (results != null)
                doctorCallback.ReadSuccess(results);
            else
                doctorCallback.ReadFail();
            
        }

        public async Task GetDoctorsByDeptCount(int dept,int hosp_id, IDoctorCountByDeptCallback callback)
        {
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
    }
}

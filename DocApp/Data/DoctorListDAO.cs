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
                    doctorCallback.ReadSuccess(results);
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
            if (results != null)
                doctorCallback.ReadSuccess(results);
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
                doctorCallback.ReadSuccess(results);
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
                doctorCallback.ReadSuccess(results);
            else
                doctorCallback.ReadFail();

        }

    }
}

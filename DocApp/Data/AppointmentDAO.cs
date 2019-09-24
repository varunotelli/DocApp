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
    public class AppointmentDAO : IAppointment
    {
        
        public async Task AddAppointment(int p_id, int doc_id, int hosp_id, string app_date, string start, 
            IAppointmentCallback callback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            try
            {

                await DBHandler.db.ExecuteAsync(String.Format("INSERT INTO APPOINTMENT (PATIENT_ID,DOC_ID, HOS_ID, APP_DATE, " +
                    "START_TIME) VALUES({0},{1},{2},'{3}','{4}')", p_id, doc_id, hosp_id, app_date, start)
                ); 
                System.Diagnostics.Debug.WriteLine("Appointment insert dao Success");
                await DBHandler.db.CloseAsync();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Appointment insert dao exception=" + e.Message);
                callback.AppReadFail();

            }
            
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                var results = await DBHandler.db.QueryAsync<Appointment>("SELECT * FROM APPOINTMENT " +
                "ORDER BY ID DESC LIMIT 1");

                if (results != null)
                {
                    System.Diagnostics.Debug.WriteLine("appointment Select dao Success");

                    callback.AppReadSuccess(results.First());
                    await DBHandler.db.CloseAsync();
                }
                else
                    callback.AppReadFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("appointment select dao exception=" + e.Message);
                callback.AppReadFail();
            }
        }
    }
}

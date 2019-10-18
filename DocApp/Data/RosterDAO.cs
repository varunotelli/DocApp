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
    public class RosterDAO : IRoster
    {
        List<Roster> results;
        public async Task GetTimeSlots(int doc_id, int hosp_id, string app_date, IRosterCallback callback)
        {
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Roster>(String.Format("SELECT *, MAX_PATIENTS-" +
                    "(SELECT COUNT(*) FROM APPOINTMENT WHERE DOC_ID={0} AND HOS_ID={1} AND APP_DATE='{2}' AND " +
                    "APPOINTMENT.START_TIME=ROSTER.START_TIME) AS VAL" +
                    " FROM ROSTER WHERE DOC_ID={0} AND HOSP_ID={1} "
                    , doc_id, hosp_id,app_date));
                //System.Diagnostics.Debug.WriteLine("results=" + DateTime.Now.ToString("HH:mm:ss"));
                if (results != null)
                {
                    List<Roster> resultmain = new List<Roster>();
                    foreach(var x in results)
                    {
                        string s = app_date + " " + x.start_time;
                        DateTime d1 = DateTime.Parse(s);
                        DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        if (d1.CompareTo(d2) >=0)
                            resultmain.Add(x);
                           
                    }
                    callback.RosterReadSuccess(resultmain);
                    await DBHandler.db.CloseAsync();
                }
                else
                    callback.RosterReadFail();
            }
            
        
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DOCTOR ASYNC SELECT EXCEPTION" + e.Message);
            }
        }
    }
}

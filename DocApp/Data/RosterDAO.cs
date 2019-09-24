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
        public async Task GetTimeSlots(int doc_id, int hosp_id, IRosterCallback callback)
        {
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Roster>(String.Format("SELECT * FROM ROSTER WHERE DOC_ID={0} AND HOSP_ID={1}", doc_id, hosp_id));
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null)
                {
                    callback.RosterReadSuccess(results);
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

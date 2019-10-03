using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Data
{
    public class TestDAO : ITest
    {
        public async Task AddTestimonial(int pid, int doc, string message, string time, ITestCallback callback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            try
            {

                await DBHandler.db.ExecuteAsync(String.Format("INSERT INTO TESTIMONIAL (PATIENT_ID,DOC_ID, MESSAGE, POSTED_TIME " +
                    ") VALUES({0},{1},'{2}','{3}')", pid, doc, message, time)
                );
                //System.Diagnostics.Debug.WriteLine("Appointment insert dao Success");
                await DBHandler.db.CloseAsync();
                callback.TestReadSuccess();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Testimonial insert dao exception=" + e.Message);
                callback.TestReadFail();
                return;

            }
            
        }
    }
}

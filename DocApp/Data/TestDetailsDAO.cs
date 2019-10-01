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
    public class TestDetailsDAO:ITestDetails
    {
        public async Task GetTestDetails(int doc_id,ITestDetailsCallback callback)
        {
            List<TestDetails> results = new List<TestDetails>();
            try
            {
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                var patients = await DBHandler.db.Table<Patient>().ToListAsync();
                var tests = await DBHandler.db.Table<Testimonial>().ToListAsync();
                
                var details = (from t in tests
                               join p in patients
                               on t.Patient_ID equals p.ID
                               
                               where tests.Any(g => t.Doc_ID==doc_id)
                               select new TestDetails
                               {
                                   doc_id=t.Doc_ID,
                                   message=t.message,
                                   patient_name=p.name,
                                   posted_time=t.posted_time,
                                   p_id=p.ID,
                                   test_id=t.ID
                               }
                    ).OrderBy(x => x.posted_time);
                foreach (var x in details)
                {
                    //x.app_date = DateTime.ParseExact(x.app_date, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
                    results.Add(x);
                }
                if (results != null)
                    callback.TestDetailsReadSuccess(results);
                else
                    callback.TestDetailsReadFail();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Testimonial details select exception=" + e.Message);
            }
        }
    }
}

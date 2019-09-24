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
    public class KeywordDAO : IKeywordList
    {
        public async Task GetKeywordsAsync(IKeyWordCallback callback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            List<KeyWord> results = new List<KeyWord>();
            try
            {
                results = await DBHandler.db.QueryAsync<KeyWord>("select * from keywords group by DEPT_ID");
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Keyword DB Exception"+ e.Message);
            }
            if (results != null)
                callback.KeyWordReadSuccess(results);
            else
                callback.KeyWordReadFail();
        }
    }
}

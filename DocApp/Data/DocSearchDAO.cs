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
    public class DocSearchDAO:IDoc_Search
    {
        public async Task AddDocSearch(Doc_Search d,IDoc_SearchCallback callback)
        {
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            try
            {
                int x=await DBHandler.db.InsertAsync(d);
                if (x > 0)
                    callback.Doc_SearchInsertSuccess(x);
                else callback.Doc_SearchInsertFail();

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Doc_Search insert DAO fail");
            }
        }
    }
}

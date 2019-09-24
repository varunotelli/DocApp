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
    public class DepartmentDAO : IDepartmentList
    {
        public async Task GetDepartmentList(IDepartmentListCallBack callback)
        {
            List<Department> results = new List<Department>();

            try
            {

                //var db = await dbHandler.DBConnection();
                if (DBHandler.db == null)
                    DBHandler.DBConnection();
                results = await DBHandler.db.QueryAsync<Department>(
                    "SELECT * FROM DEPARTMENT ");
                System.Diagnostics.Debug.WriteLine("results=" + results.Count());
                if (results != null && results.Count>0)
                {
                    callback.DepartmentDataReadSuccess(results);
                    //await DBHandler.db.CloseAsync();
                }
                else
                    callback.DepartmentDataReadFail();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DEPARTMENT SELECT EXCEPTION " + e.Message);
            }

        }
    }
}

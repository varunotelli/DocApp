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
    public class LoginDAO:ILogin
    {
        public async Task LoginAsync(Logins l,ILoginCallback callback)
        {
            int count = -1;
            if (DBHandler.db == null)
                DBHandler.DBConnection();
            try
            {
                await DBHandler.db.InsertAsync(l);
                await DBHandler.db.CloseAsync();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Login DAO fail "+e.Message);
                return;
            }

            if (DBHandler.db == null)
                DBHandler.DBConnection();
            try
            {
                count = await DBHandler.db.ExecuteScalarAsync<int>("select count(*) from LOGINS where USER_ID=1");
            }
            catch(Exception e)
            {
                callback.GetLoginFail();
                return;
            }
            callback.GetLoginSuccess(count);
            
        }
    }
}

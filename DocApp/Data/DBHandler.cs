using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;
using Windows.Storage;

namespace DocApp.Data
{
    public class DBHandler
    {
        public static SQLiteAsyncConnection db;
        public static SQLiteAsyncConnection DBConnection()
        {
            System.Diagnostics.Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
            var dbPath=Path.Combine(ApplicationData.Current.LocalFolder.Path, "DoctorDB.db");
            db = new SQLiteAsyncConnection(dbPath);
            return db;
        }
    }
}

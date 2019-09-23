using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Domain.Callbacks;
using DocApp.Models;
using SQLite;
using SQLitePCL;

namespace DocApp.Domain.DataContracts
{
    public interface IHospitalList
    {
        Task GetHospitalsAsync(IHospitalListCallback hospitalCallback);
        Task GetHospitalbyIdAsync(int id,IHospitalDetailCallback hospitalCallback);//Get List of Hospitals from Database
        Task GetHospitalByNameAsync(string name, string location, IHospitalListCallback hospitalCallback); // Get Selected Hospital
        Task GetHospitalByLocationAsync(string location, IHospitalListCallback hospitalCallback);
        Task GetHospitalByDept(string location,int dept_id, IHospitalListCallback hospitalCallback);
        Task UpdateHospitalRating(int id, double rating, IHospitalUpdateCallback hospitalCallback);

    }
}

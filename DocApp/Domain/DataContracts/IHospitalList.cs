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
        Task GetHospitalsAsync(IHospitalCallback hospitalCallback);
        Task GetHospitalbyIdAsync(int id,IHospitalCallback hospitalCallback);//Get List of Hospitals from Database
        Task GetHospitalByNameAsync(string name, IHospitalCallback hospitalCallback); // Get Selected Hospital
        Task GetHospitalByLocationAsync(string location, IHospitalCallback hospitalCallback);
        Task GetHospitalByDept(string location,int dept_id, IHospitalCallback hospitalCallback);

    }
}

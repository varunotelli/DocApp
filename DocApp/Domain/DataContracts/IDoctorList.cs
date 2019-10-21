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
    public interface IDoctorList
    {
        Task GetDoctorsAsync(IDoctorCallback dcall); //Get List of Hospitals from Database
        Task GetDoctorByIdAsync(int id, IDoctorDetailCallback callback);
        Task GetDoctorByNameAsync(string name, string location, IDoctorCallback doctorCallback);
        Task GetDoctorCountAsync(int dept, int hosp_id, IDoctorCountByDeptCallback callback);
        Task GetDoctorByHospitalNameAsync(string name, IDoctorCallback doctorCallback);
        Task GetDoctorByHospitalLocationAsync(string name, IDoctorCallback doctorCallback);
        Task GetDoctorByDeptLocationAsync(string location, int dept, IDoctorCallback doctorCallback,
            int lexp = -1, int uexp = 200, int rating = -1);
        Task UpdateDoctorRating(int id, double rating, IDoctorUpdateCallback doctorCallback);
        Task GetRecentDoctor(int id, IRecentDoctorCallback callback);
        Task GetMostBookedDoctor(int id, IMostBookedDoctorCallback callback);
    }
        
}

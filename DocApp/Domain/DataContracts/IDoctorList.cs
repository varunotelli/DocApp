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
        Task GetDoctorByNameAsync(string name, IDoctorCallback doctorCallback);
        Task GetDoctorByHospitalNameAsync(string name, IDoctorCallback doctorCallback);
        Task GetDoctorByHospitalLocationAsync(string name, IDoctorCallback doctorCallback);



    }
}

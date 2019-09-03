﻿using DocApp.Domain.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface IDoctorInHospitalList
    {
        Task GetDoctorDetailByHospital(string hospname, IDoctorHospitalCallBack doctorHospitalCallBack);
    }
}

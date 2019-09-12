﻿using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Callbacks
{
    public interface IDoctorDetailViewCallBack
    {
        bool DataReadSuccess(Doctor d);
        bool DataReadFail();
        //bool DoctorDetailViewSuccess(Doctor d);
        //bool DoctorDetailViewFail();
    }
}

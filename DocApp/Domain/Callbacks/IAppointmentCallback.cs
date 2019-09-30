﻿using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.Callbacks
{
    public interface IAppointmentCallback
    {
        bool GetAppByIDSuccess(AppointmentDetails appointment);
        bool GetAppByIDFail();
    }
}

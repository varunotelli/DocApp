using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Models;

namespace DocApp.Presentation.Callbacks
{
    public interface IDoctorViewCallBack
    {
        bool DataReadSuccess(List<Doctor> d);
        bool DataReadFail();
    }
}

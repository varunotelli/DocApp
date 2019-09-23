using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.Callbacks
{
    public interface IHospitalUpdateCallback
    {
        bool HospitalUpdateSuccess(Hospital h);
        bool HospitalUpdateFail();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Models;
using DocApp.Presentation.Callbacks;

namespace DocApp.Domain.Callbacks
{
    public interface IHospitalListCallback
    {
        bool ReadSuccess(List<Hospital> h);
        bool ReadFail();
    }
}

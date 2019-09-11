using DocApp.Domain.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface ILocationList
    {
        Task GetLocationsAsync(string val, ILocationCallBack addressCallBack);
    }
}

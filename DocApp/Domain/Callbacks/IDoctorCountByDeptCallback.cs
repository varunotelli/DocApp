using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.Callbacks
{
    public interface IDoctorCountByDeptCallback
    {
        bool ReadCountSuccess(int count);
        bool ReadCountFail();
    }
}

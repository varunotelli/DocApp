using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.Callbacks
{
    public interface ILoginCallback
    {
        bool GetLoginSuccess(int val);
        bool GetLoginFail();
    }
}

using DocApp.Domain.Callbacks;
using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface ILogin
    {
        Task LoginAsync(Logins l, ILoginCallback callback);
    }
}

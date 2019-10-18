using DocApp.Data;
using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.UseCase
{
    public class LoginUseCase : UseCaseBase, ILoginCallback
    {
        ILoginViewCallback viewCallback;
        int ct = -1;
        Logins l;
        public LoginUseCase(Logins temp)
        {
            this.l = temp;
        }


        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ILoginViewCallback)p;
        }

        internal override async Task Action()
        {
            ILogin login = new LoginDAO();
            try
            {
                await login.LoginAsync(l, this);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("LOGIN USE CASE FAIL");
            }
            if (ct != -1)
                viewCallback.LoginViewSuccess(ct);
            else
                viewCallback.LoginViewFail();


;        }
        public bool GetLoginFail()
        {
            return false;
        }

        public bool GetLoginSuccess(int val)
        {
            this.ct = val;
            return true;
        }
    }
}

using DocApp.Data;
using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.UseCase
{
    public class UpdateAppUseCase : UseCaseBase, IUpdateAppCallback
    {
        IUpdateAppViewCallback viewCallback;
        bool flag = false;
        string app_date, timeslot;
        int id;

        public UpdateAppUseCase(int x,string a, string t)
        {
            this.id = x;
            this.app_date = a;
            this.timeslot = t;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IUpdateAppViewCallback)p;
        }

        public bool UpdateAppFail()
        {
            return false;
        }

        public bool UpdateAppSuccess(bool f)
        {
            this.flag = f;
            return true;
        }

        internal override async Task Action()
        {
            IAppointment appointment = new AppointmentDAO();
            await appointment.UpdateAppointment(id, app_date, timeslot, this);
            if (flag)
                viewCallback.UpdateAppViewSuccess(flag);
            else viewCallback.UpdateAppViewFail();
        }
    }
}

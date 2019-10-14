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
    public class CheckAppointmentUseCase : UseCaseBase, ICheckAppointmentCallback
    {
        ICheckAppointmentViewCallback viewCallback;
        int count,p_id;
        string app_date, time;
        public CheckAppointmentUseCase(int p, string a , string t)
        {
            this.p_id = p;
            this.app_date = a;
            this.time = t;
        }


        public bool CheckAppointmentFail()
        {
            return false;
        }

        public bool CheckAppointmentSuccess(int x)
        {
            count = x;
            return true;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ICheckAppointmentViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointment appointment = new AppointmentDAO();
            await appointment.CheckAppointment(p_id, app_date, time, this);
            if (count >= 0)
                viewCallback.CheckAppointmentViewSuccess(count);
            else viewCallback.CheckAppointmentViewFail();

        }
    }
}

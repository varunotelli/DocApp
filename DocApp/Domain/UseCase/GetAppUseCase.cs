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
    public class GetAppUseCase : UseCaseBase, IAppCallback
    {
        Appointment appointment;
        IAppViewCallback viewCallback;
        int id;

        public GetAppUseCase(int x)
        {
            this.id = x;

        }

        public bool AppFail()
        {
            return false;
        }

        public bool AppSuccess(Appointment a)
        {
            this.appointment = a;
            return true;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IAppViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointment app = new AppointmentDAO();
            await app.GetAppointmentByID(id, this);
            if (appointment != null)
                viewCallback.AppViewSuccess(appointment);
            else viewCallback.AppViewFail();

        }
    }
}

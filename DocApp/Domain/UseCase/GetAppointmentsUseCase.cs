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
    public class GetAppointmentsUseCase : UseCaseBase,IAppointmentListCallback
    {

        IAppDisplayViewCallback viewCallback;
        List<AppointmentDetails> apps;
        int id;
        public GetAppointmentsUseCase(int x)
        {
            this.id = x;
            apps = new List<AppointmentDetails>();
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IAppDisplayViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointmentDetails applist = new AppointmentDetailsDAO();
            await applist.GetAppointment(id, this);
            if (apps != null)
                viewCallback.GetAppsReadViewSuccess(apps);
            else
                viewCallback.GetAppsReadViewFail();
        }
        public bool GetAppFail()
        {
            return false;
        }

        public bool GetAppSuccess(List<AppointmentDetails> appointments)
        {
            this.apps = appointments;
            System.Diagnostics.Debug.WriteLine("appdate="+apps[0].app_date);
            return true;
        }
    }
}

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
    public class GetAppointmentByIDUseCase:UseCaseBase,IAppointmentCallback
    {
        IAppByIDViewCallback viewCallback;
        AppointmentDetails app;
        int id;
        string app_date, time;
        public GetAppointmentByIDUseCase(string app_date1,string time1)
        {
            this.app_date = app_date1;
            this.time = time1;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IAppByIDViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointmentDetails applist = new AppointmentDetailsDAO();
            await applist.GetAppointmentByID(app_date,time, this);
            if (app != null)
                viewCallback.AppByIDViewSuccess(app);
            viewCallback.AppByIDViewFail();
        }
        public bool GetAppByIDFail()
        {
            return false;
        }

        public bool GetAppByIDSuccess(AppointmentDetails appointment)
        {
            this.app = appointment;
            //System.Diagnostics.Debug.WriteLine("appdate=" + apps[0].app_date);
            return true;
        }
    }
}

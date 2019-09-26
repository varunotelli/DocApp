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
    public class BookAppointmentUseCase : UseCaseBase, IBookAppointmentCallback
    {
        IAppBookingViewCallback viewCallback;
        Appointment appointment;
        int p_id, hosp_id,doc_id;
        string app_date, start;

        public BookAppointmentUseCase(int p,int d,int h, string date, string s)
        {
            this.p_id = p;
            this.doc_id = d;
            this.hosp_id = h;
            this.app_date = date;
            this.start = s;
            //this.end = e;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IAppBookingViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointment App = new AppointmentDAO();
            try
            {
                await App.AddAppointment(p_id, doc_id, hosp_id, app_date, start,this);
            }
            catch(Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("appointment Usecase exception="+e1.Message);
                viewCallback.AppViewReadFail();
            }
            if (appointment != null)
                viewCallback.AppViewReadSuccess(appointment);
            else viewCallback.AppViewReadFail();
        }

        public bool AppReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Appointment Use case fail");
            return false;
        }

        public bool AppReadSuccess(Appointment a)
        {
            System.Diagnostics.Debug.WriteLine("Appointment Use case success");
            this.appointment = a;
            return true;
        }
    }
}

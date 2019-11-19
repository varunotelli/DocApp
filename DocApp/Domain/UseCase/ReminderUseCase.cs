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
    public class ReminderUseCase : UseCaseBase,IReminderCallback
    {

        IReminderViewCallback viewCallback;
        int p_id;
        string app_date, time;
        AppointmentDetails result;

        public ReminderUseCase(int p,string a , string t)
        {
            this.p_id = p;
            this.app_date = a;
            this.time = t;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IReminderViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointmentDetails appointmentDetails = new AppointmentDetailsDAO();
            //var temp1 = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));
            //var temp2 = DateTime.Parse(DateTime.Now.AddMinutes(1).ToString("HH:mm") + ":00");
            //var diff = temp1.Subtract(temp2).Milliseconds;
            //await Task.Delay(diff);
            
                
            while (true)
            {

                if (DateTime.Now.Second == 0)
                {
                    await appointmentDetails.ReminderForApps(p_id, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm"), this);
                    await Task.Delay(60 * 1000);
                }
                if (result != null)
                {
                    viewCallback.ReminderViewSuccess(result);
                    //await Task.Delay()
                }
               
                result = null;
                //await Task.Delay(60 * 1000);
            }
            

        }

        public bool ReminderReadFail()
        {
            return false;
        }

        public bool ReminderReadSuccess(AppointmentDetails detail)
        {
            this.result = detail;
            return true;
        }
    }
}

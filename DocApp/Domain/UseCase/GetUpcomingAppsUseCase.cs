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
    public class GetUpcomingAppsUseCase : UseCaseBase, IUpcomingAppCallback
    {
        IUpcomingAppViewCallback viewCallback;
        List<AppointmentDetails> details;
        int p_id;
        public GetUpcomingAppsUseCase(int id)
        {
            this.p_id = id;
            details = new List<AppointmentDetails>();
        }
        public override void SetCallBack<P>(P p)
        {
            this.viewCallback=(IUpcomingAppViewCallback)p;
        }

        internal override async Task Action()
        {
            try
            {
                IAppointmentDetails appointmentDetails = new AppointmentDetailsDAO();
                await appointmentDetails.GetUpcomingApps(p_id, this);
                if (details != null)
                    viewCallback.UpcomingAppViewSuccess(details);
                else viewCallback.UpcomingAppViewFail();
            }
            catch(Exception)
            {

            }
        }

        public bool UpcomingAppFail()
        {
            return false;
        }

        public bool UpcomingAppSuccess(List<AppointmentDetails> appointments)
        {
            this.details = appointments;
            return true;
        }
    }
}

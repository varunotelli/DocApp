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
    public class GetAppByDocUseCase : UseCaseBase, IAppByDocCallback
    {
        int doc_id,p_id;
        IAppByDocViewCallback viewCallback;
        List<AppointmentDetails> appointments;
        public GetAppByDocUseCase(int d,int p)
        {
            appointments = new List<AppointmentDetails>();
            this.doc_id = d;
            this.p_id = p;
        }
        
        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IAppByDocViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointmentDetails appointmentDetails = new AppointmentDetailsDAO();
            await appointmentDetails.GetAppointmentByDoc(p_id,doc_id,this);
            if (appointments != null)
                viewCallback.AppByDocViewSuccess(appointments);
            else viewCallback.AppByDocViewFail();
        }
        public bool AppByDocFail()
        {
            return false;
        }

        public bool AppByDocSuccess(List<AppointmentDetails> details)
        {
            this.appointments = details;
            return true;
        }

    }
}

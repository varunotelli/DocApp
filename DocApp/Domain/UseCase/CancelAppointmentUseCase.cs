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
    public class CancelAppointmentUseCase : UseCaseBase, ICancelAppCallback
    {
        ICancelAppViewCallback viewCallback;
        int id;
        bool flag = false;
        public CancelAppointmentUseCase(int x)
        {
            this.id = x;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ICancelAppViewCallback)p;
        }

        internal override async Task Action()
        {
            IAppointment appointment = new AppointmentDAO();
            await appointment.CancelAppointment(id,this);
            if (flag)
                viewCallback.CancelAppViewSuccess(true);
            else viewCallback.CancelAppViewFail();

        }

        public bool CancelAppSuccess(bool f)
        {
            this.flag = f;
            return true;
        }

        public bool CancelAppFail()
        {
            return false;
        }
    }
}

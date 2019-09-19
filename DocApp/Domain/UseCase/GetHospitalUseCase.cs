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
    public class GetHospitalUseCase : UseCaseBase,IHospitalDetailCallback
    {
        Hospital hosp;
        IHospitalDetailViewCallBack callback;
        int id;
        public GetHospitalUseCase(int x)
        {
            this.id = x;
            this.hosp = new Hospital();
        }

        

        public bool HospitalDetailReadFail()
        {
            return false;
        }

        public bool HospitalDetailReadSuccess(Hospital h)
        {
            this.hosp = h;
            return true;
        }

        public override void SetCallBack<P>(P p)
        {
            this.callback = (IHospitalDetailViewCallBack)p;
        }

        internal override async Task Action()
        {
            IHospitalList hospitalList = new HospitalListDAO();
            await hospitalList.GetHospitalbyIdAsync(id,this);
            if (hosp != null)
                callback.DataReadSuccess(hosp);
            else callback.DataReadFail();
        }
    }
}

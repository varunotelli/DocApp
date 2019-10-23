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
    public class GetLastHospitalUseCase : UseCaseBase, ILastHospitalCallback
    {

        ILastHospitalViewCallback viewCallback;
        Hospital h;
        int p_id, doc_id;

        public GetLastHospitalUseCase(int p,int d)
        {
            this.p_id = p;
            this.doc_id = d;
        }

        public override void SetCallBack<P>(P p)
        {
            viewCallback = (ILastHospitalViewCallback)p;
        }

        internal override async Task Action()
        {
            IHospitalList hospitalList = new HospitalListDAO();
            await hospitalList.GetLastHospital(p_id, doc_id, this);
            if (h != null)
                viewCallback.LastHospitalViewSuccess(h);
            else viewCallback.LastHospitalViewFail();
        }

        public bool ILastHospitalFail()
        {
            return false;
        }

        public bool ILastHospitalSuccess(Hospital hospital)
        {
            this.h = hospital;
            return true;
        }
    }
}

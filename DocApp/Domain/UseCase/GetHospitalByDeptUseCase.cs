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
    public class GetHospitalByDeptUseCase : UseCaseBase, IDoctorCountByDeptCallback
    {
        int ct;
        int dept_id;
        int hosp_id;
        IHospitalByDeptViewCallback viewCallback;

        public GetHospitalByDeptUseCase(int d,int h)
        {
            this.dept_id = d;
            this.hosp_id = h;
        }
        
        public bool ReadCountFail()
        {
            return false;
        }

        public bool ReadCountSuccess(int count)
        {
            this.ct = count;
            return true;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IHospitalByDeptViewCallback)p;
        }

        internal override async Task Action()
        {
            IDoctorList doctors = new DoctorListDAO();
            await doctors.GetDoctorCountAsync(dept_id, hosp_id, this);
            if (ct > 0)
                viewCallback.ReadCountViewSuccess(ct);
            else viewCallback.ReadCountViewFail();


;        }
    }
}

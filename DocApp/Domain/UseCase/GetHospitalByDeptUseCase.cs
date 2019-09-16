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
    public class GetHospitalByDeptUseCase : UseCaseBase, IHospitalCallback
    {
        List<Hospital> hosps;
        int dept_id;
        string location;
        
        IHospitalByDeptViewCallback viewCallback;

        public GetHospitalByDeptUseCase(string loc,int d)
        {
            hosps = new List<Hospital>();
            this.dept_id = d;
            this.location = loc;
            
        }

        public bool ReadFail()
        {
            return false;
        }

        public bool ReadSuccess(List<Hospital> hospitals)
        {
            this.hosps = hospitals;
            return true;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IHospitalByDeptViewCallback)p;
        }

        internal override async Task Action()
        {

            IHospitalList hospitalList = new HospitalListDAO();
            await hospitalList.GetHospitalByDept(location,dept_id, this);
            if (hosps != null)
                viewCallback.ReadViewSuccess(hosps);
            else viewCallback.ReadViewFail();

;       }
    }
}

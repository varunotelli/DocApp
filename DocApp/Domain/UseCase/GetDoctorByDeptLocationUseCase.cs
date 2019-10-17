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
    public class GetDoctorByDeptLocationUseCase : UseCaseBase, IDoctorCallback
    {
        IDoctorDeptLocationViewCallback viewCallback;
        List<Doctor> docs;
        string location = "";
        int dept;
        int lexp = -1, uexp=-1, rating = -1;

        
    

        public GetDoctorByDeptLocationUseCase(string loc,int d, int le=-1, int ue=-1,int r=-1)
        {
            docs = new List<Doctor>();
            location = loc;
            dept=d;
            lexp = le;
            uexp = ue;
            rating = r;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IDoctorDeptLocationViewCallback)p;
        }

        internal override async Task Action()
        {
            IDoctorList doctorList = new DoctorListDAO();
            try
            {
                await doctorList.GetDoctorByDeptLocationAsync(location.ToUpper(), dept,this, lexp,uexp,rating);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("doc dept usercase exception " + e.Message);
            }
            if (docs != null)
                viewCallback.DeptLocationViewSuccess(docs);
            else
                viewCallback.DeptLocationViewFail();

        }
        public bool ReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Dept loc use case fail");
            return false;
        }

        public bool ReadSuccess(List<Doctor> doctors)
        {
            System.Diagnostics.Debug.WriteLine("Dept loc use case success");
            this.docs = doctors;
            return true;
        }


    }
}

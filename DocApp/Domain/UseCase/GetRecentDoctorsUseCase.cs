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
    public class GetRecentDoctorsUseCase : UseCaseBase, IRecentDoctorCallback
    {
        IRecentDoctorViewCallback viewCallback;
        List<Doctor> doctors = new List<Doctor>();
        int id;
        public GetRecentDoctorsUseCase(int x)
        {
            this.id = x;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IRecentDoctorViewCallback)p;
        }



        internal override async Task Action()
        {
            IDoctorList doctorList = new DoctorListDAO();
            try
            {
                await doctorList.GetRecentDoctor(id, this);
                if (doctors != null)
                {
                    //doctors.Reverse();
                    viewCallback.SearchDocViewSuccess(doctors);
                }
                    
                else viewCallback.SearchDocViewFail();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }
        public bool RecentDocFail()
        {
            throw new NotImplementedException();
        }

        public bool RecentDocSuccess(List<Doctor> d)
        {
            foreach (var x in d)
                doctors.Add(x);
            return true;
        }
    }
}

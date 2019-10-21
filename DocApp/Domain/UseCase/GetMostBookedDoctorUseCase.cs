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
    public class GetMostBookedDoctorUseCase : UseCaseBase, IMostBookedDoctorCallback
    {
        IMostBookedDocViewCallback viewCallback;
        int id;
        List<Doctor> doctors;
        public GetMostBookedDoctorUseCase(int x)
        {
            this.id = x;
            this.doctors = new List<Doctor>();
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IMostBookedDocViewCallback)p;
        }

        internal override async Task Action()
        {
            IDoctorList doctorList = new DoctorListDAO();
            try
            {
                await doctorList.GetMostBookedDoctor(id, this);
                if (doctors != null)
                    viewCallback.MostBookedDocViewSuccess(doctors);
                else viewCallback.MostBookedDocViewFail();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("most booked doc usecase fail " + e.Message);
            }
            
        }

        public bool MostBookedDocReadSuccess(List<Doctor> d)
        {
            this.doctors = d;
            return true;
        }

        public bool MostBookedDocReadFail()
        {
            return false;
        }
    }
}

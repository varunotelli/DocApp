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
    public class UpdateDoctorRatingUseCase : UseCaseBase, IDoctorUpdateCallback
    {
        string name = "";
        double rating = 0;
        DoctorDetailViewCallBack doctorDetailViewCallBack;
        Doctor doctor = new Doctor();
        public UpdateDoctorRatingUseCase(string name1,double r)
        {
            this.name = name1;
            this.rating = r;
            
        }
        

        public override void SetCallBack<P>(P p)
        {
            this.doctorDetailViewCallBack = (DoctorDetailViewCallBack)p;
        }

        internal override async Task Action()
        {
            IDoctorList doctorList = new DoctorListDAO();
            try
            {   
                await doctorList.UpdateDoctorRating(name, rating, this);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DB UPDATE EXCEPTION" + e.Message);
            }
            //if (doctor != null)
            //    doctorDetailViewCallBack.DoctorDetailViewSuccess(doctor);
            //else
            //    doctorDetailViewCallBack.DoctorDetailViewFail()
;        }
        public bool DoctorUpdateFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor Update DAO Fail");
            return false;
        }

        public bool DoctorUpdateSuccess(Doctor results)
        {
            this.doctor = results;
            System.Diagnostics.Debug.WriteLine("Doctor Update DAO Success"+ results.Number_of_Rating);
            return true;
        }
    }
}

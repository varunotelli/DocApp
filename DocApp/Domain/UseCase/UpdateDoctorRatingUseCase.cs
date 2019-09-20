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
        int id;
        double rating = 0;
        IDoctorDetailViewCallBack doctorDetailViewCallBack;
        Doctor doctor = new Doctor();
        public UpdateDoctorRatingUseCase(int x,double r)
        {
            this.id = x;
            this.rating = r;
            
        }
        

        public override void SetCallBack<P>(P p)
        {
            this.doctorDetailViewCallBack = (IDoctorDetailViewCallBack)p;
        }

        internal override async Task Action()
        {
            IDoctorList doctorList = new DoctorListDAO();
            try
            {   
                await doctorList.UpdateDoctorRating(id, rating, this);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DB UPDATE EXCEPTION" + e.Message);
            }
            if (doctor != null)
                doctorDetailViewCallBack.DataReadSuccess(doctor);
            else
                doctorDetailViewCallBack.DataReadFail();
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

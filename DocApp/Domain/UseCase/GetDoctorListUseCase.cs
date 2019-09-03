using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Models;
using DocApp.Presentation.ViewModels;
using DocApp.Presentation.Callbacks;
using DocApp.Data;
using DocApp.Domain.DataContracts;
using DocApp.Domain.Callbacks;

namespace DocApp.Domain.UseCase
{
    public class GetDoctorListUseCase: UseCaseBase, IDoctorCallback
    {
        List<Doctor> doctors = new List<Doctor>();
        //HospitalViewCallback hospitalUseCaseCallback;
        DoctorViewCallback doctorUseCaseCallback;
        //IDoctorCallback dcall;
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            
            IDoctorList DoctorList = new DoctorListDAO();
            
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await DoctorList.GetDoctorsAsync(this);
                System.Diagnostics.Debug.WriteLine(doctors.Count());
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("DB EXCEPTION" + e.Message);
            }


            if (doctors != null && doctors.Count > 0)
                doctorUseCaseCallback.DataReadSuccess(doctors);
            else doctorUseCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.doctorUseCaseCallback = (DoctorViewCallback)p;
        }


        public bool ReadSuccess(List<Doctor> docs)
        {
            this.doctors = new List<Doctor>();
            this.doctors = docs;
            System.Diagnostics.Debug.WriteLine("DAO READ SUCCESS!!!");
              
            return true;
        }
        public bool ReadFail()
        {
            System.Diagnostics.Debug.WriteLine("DAO READ FAIL!!!");
            return false;

        }
    }
}

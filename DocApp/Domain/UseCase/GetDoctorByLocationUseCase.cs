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
    class GetDoctorByLocationUseCase : UseCaseBase, IDoctorCallback
    {
        string location = "";
        List<Doctor> doctors = new List<Doctor>();
        //HospitalViewCallback hospitalUseCaseCallback;
        DoctorViewCallback doctorUseCaseCallback;
        public GetDoctorByLocationUseCase(string name)
        {
            this.location = name;
        }
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IDoctorList DoctorList = new DoctorListDAO();
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                if (this.location.Equals("CURRENT LOCATION"))
                    await DoctorList.GetDoctorsAsync(this);
                else
                    await DoctorList.GetDoctorByHospitalLocationAsync(this.location, this);
                
                //System.Diagnostics.Debug.WriteLine(doctors.Count());
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("get doctor by location usecase DB EXCEPTION" + e.Message);
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

        public bool ReadSuccess(List<Doctor> doc)
        {
            this.doctors = new List<Doctor>();
            this.doctors = doc;
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

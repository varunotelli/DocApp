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
    class GetDoctorByHospitalUseCase: UseCaseBase, IDoctorHospitalCallBack
    {
        string hospitalName = "";
        List<DoctorInHospitalDetails> doctors = new List<DoctorInHospitalDetails>();
        //HospitalViewCallback hospitalUseCaseCallback;
        DoctorHospitalDetailViewCallback doctorUseCaseCallback;
        public GetDoctorByHospitalUseCase(string name)
        {
            this.hospitalName = name;
        }
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IDoctorInHospitalList DoctorList = new DoctorHospitalDAO();
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await DoctorList.GetDoctorDetailByHospital(hospitalName,this);
                System.Diagnostics.Debug.WriteLine(doctors.Count());
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("get doctor by hospital usecase DB EXCEPTION" + e.Message);
            }


            if (doctors != null && doctors.Count >0)
                doctorUseCaseCallback.DataReadSuccess(doctors);
            else doctorUseCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.doctorUseCaseCallback = (DoctorHospitalDetailViewCallback)p;
        }

        public bool ReadSuccess(List<DoctorInHospitalDetails> doc)
        {
            this.doctors = new List<DoctorInHospitalDetails>();
            this.doctors = doc;
            System.Diagnostics.Debug.WriteLine("Doctor fees=" + doc[0].fees);
            System.Diagnostics.Debug.WriteLine("Doctor DAO READ SUCCESS!!!");

            return true;
        }
        public bool ReadFail()
        {
            System.Diagnostics.Debug.WriteLine("DAO READ FAIL!!!");
            return false;

        }
    }
}

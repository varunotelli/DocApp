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
    public class GetHospitalByDoctorUseCase : UseCaseBase, IHospitalDoctorCallback
    {

        HospitalDoctorViewCallBack useCaseCallback;
        List<HospitalInDoctorDetails> hospitals;
        int id;
        public GetHospitalByDoctorUseCase(int n)
        {
            this.id = n;
        }
        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (HospitalDoctorViewCallBack)p;
        }

        internal override async Task Action()
        {
            IHospitalInDoctorList HospitalList = new HospitalDoctorDAO();
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await HospitalList.GetHospitalByDoctor(id, this);
                System.Diagnostics.Debug.WriteLine(hospitals.Count());
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("get hospital by doctor usecase DB EXCEPTION" + e.Message);
            }


            if (hospitals != null && hospitals.Count > 0)
                useCaseCallback.DataReadSuccess(hospitals);
            else useCaseCallback.DataReadFail();
        }

        public bool ReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Hospitals of Docs DAO read fail");
            return false;
        }

        public bool ReadSuccess(List<HospitalInDoctorDetails> hosp)
        {

            System.Diagnostics.Debug.WriteLine("Hospitals of Docs DAO read success");
            this.hospitals = hosp;
            System.Diagnostics.Debug.WriteLine("hospital count=" + hospitals.Count());
            return true;
        }
    }
}

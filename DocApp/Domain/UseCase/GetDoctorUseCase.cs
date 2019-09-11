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
    public class GetDoctorUseCase: UseCaseBase, IDoctorCallback
    {
        Doctor doc = new Doctor();
        int id;
        //HospitalViewCallback hospitalUseCaseCallback;
        DoctorDetailViewCallBack doctorUseCaseCallback;
        //IDoctorCallback dcall;
        public GetDoctorUseCase(int n)
        {
            this.id = n;
        }

        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            
            IDoctorList DoctorList = new DoctorListDAO();
            
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await DoctorList.GetDoctorByIdAsync(id,this);
                System.Diagnostics.Debug.WriteLine("doc val="+doc.ID);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine(" get doctor list DB EXCEPTION" + e.Message);
            }


            if (doc!=null)
                doctorUseCaseCallback.DataReadSuccess(doc);
            else doctorUseCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.doctorUseCaseCallback = (DoctorDetailViewCallBack)p;
        }


        public bool ReadSuccess(List<Doctor> docs)
        {
            this.doc = docs.First();
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

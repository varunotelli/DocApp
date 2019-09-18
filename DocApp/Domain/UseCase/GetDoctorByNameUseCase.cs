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
    class GetDoctorByNameUseCase : UseCaseBase, IDoctorCallback
    {
        List<Doctor> doc = new List<Doctor>();
        IDoctorViewCallBack useCaseCallback;
        public string name="";
        string location = "";
        int dept;
        public GetDoctorByNameUseCase(string n, string l)
        {
            this.name = n;
            this.location = l;
            
        }
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IDoctorList DoctorList = new DoctorListDAO();

            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await DoctorList.GetDoctorByNameAsync(name,location, this);
                
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("get doctor by name use case DB EXCEPTION" + e.Message);
            }


            if (doc != null)
            {
                System.Diagnostics.Debug.WriteLine("get doctor by name use case success");

                useCaseCallback.DataReadSuccess(doc);
            }
            else
            {
                useCaseCallback.DataReadFail();
                System.Diagnostics.Debug.WriteLine("get doctor by name use case fail");
            }


                // + hospitals.Count());


            }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (IDoctorViewCallBack)p;
        }


        public bool ReadSuccess(List<Doctor> docs)
        {

            this.doc = docs;
            System.Diagnostics.Debug.WriteLine("DOC DETAIL DAO READ SUCCESS!!!");

            return true;
        }
        public bool ReadFail()
        {
            System.Diagnostics.Debug.WriteLine("DOC DETAIL DAO READ FAIL!!!");
            return false;

        }
    }
}


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
    public class GetHospitalByNameUseCase : UseCaseBase,IHospitalCallback
    {
        List<Hospital> hospital = new List<Hospital>();
        IHospitalViewCallback useCaseCallback;
        public string name="";
        public string location;
        public GetHospitalByNameUseCase(string n,string loc)
        {
            this.name = n;
            this.location = loc;
        }
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IHospitalList HospitalList = new HospitalListDAO();
            
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await HospitalList.GetHospitalByNameAsync(name,location,this);
                //System.Diagnostics.Debug.WriteLine("hosp val="+hospital.Number_Of_Rating);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("Get Hospital by name use case DB EXCEPTION" + e.Message);
            }


            if (hospital != null)
            {
                useCaseCallback.DataReadSuccess(hospital);
                //System.Diagnostics.Debug.WriteLine(hospital.Location);
            }
            else useCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (IHospitalViewCallback)p;
        }


        public bool ReadSuccess(List<Hospital> hosp)
        {
            
            this.hospital = hosp;
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

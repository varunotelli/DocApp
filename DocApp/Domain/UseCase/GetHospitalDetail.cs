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
    public class GetHospitalDetail : UseCaseBase,IHospitalCallback
    {
        Hospital hospital = new Hospital();
        HospitalDetailCallBack useCaseCallback;
        public string name1;
        public GetHospitalDetail(string name)
        {
            this.name1 = name;
        }
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IHospitalList HospitalList = new HospitalListDAO();
            
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await HospitalList.GetHospitalByNameAsync(name1,this);
                System.Diagnostics.Debug.WriteLine(hospital.Location);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("DB EXCEPTION" + e.Message);
            }


            if (hospital != null)
            {
                useCaseCallback.DataReadSuccess(ref hospital);
                System.Diagnostics.Debug.WriteLine(hospital.Location);
            }
            else useCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (HospitalDetailCallBack)p;
        }


        public bool ReadSuccess(List<Hospital> hosp)
        {
            
            this.hospital = hosp[0];
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

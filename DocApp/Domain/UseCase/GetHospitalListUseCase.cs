using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Models;
using DocApp.Data;
using DocApp.Domain;
using DocApp.Domain.UseCase;
using DocApp.Domain.DataContracts;
using DocApp.Presentation.Callbacks;
using DocApp.Domain.Callbacks;

namespace DocApp.Domain.Usecase
{
    public class GetHospitalListUseCase : UseCaseBase, IHospitalCallback
    {    
        List<Hospital> hospitals = new List<Hospital>();
        HospitalViewCallback useCaseCallback;
        
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IHospitalList HospitalList = new HospitalListDAO();
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await HospitalList.GetHospitalsAsync(this);
                System.Diagnostics.Debug.WriteLine(hospitals.Count());
                
            }
            catch(Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("DB EXCEPTION"+e.Message);
                
            }
            
            
            if (hospitals != null && hospitals.Count > 0)
                useCaseCallback.DataReadSuccess(ref hospitals);
            else useCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (HospitalViewCallback)p;
        }

        public bool ReadSuccess(List<Hospital> hosp)
        {
            this.hospitals = new List<Hospital>();
            this.hospitals = hosp;
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

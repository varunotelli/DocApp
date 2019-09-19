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
    public class GetHospitalByLocationUseCase : UseCaseBase, IHospitalListCallback
    {
        List<Hospital> hospitals = new List<Hospital>();
        string location = "";
        IHospitalLocationPresenterCallBack useCaseCallback;
        public GetHospitalByLocationUseCase(string x)
        {
            this.location = x;
        }

        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IHospitalList HospitalList = new HospitalListDAO();
            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await HospitalList.GetHospitalByLocationAsync(location.ToUpper(),this);
                System.Diagnostics.Debug.WriteLine(hospitals.Count());

            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("get hospital by location usecase DB EXCEPTION" + e.Message);

            }


            if (hospitals != null && hospitals.Count > 0)
                useCaseCallback.HospitalLocationReadSuccess(hospitals);
            else useCaseCallback.HospitalLocationReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (IHospitalLocationPresenterCallBack)p;
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

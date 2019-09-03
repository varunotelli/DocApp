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
    public class GetAddressUseCase: UseCaseBase,IAddressCallBack
    {
       
        GetAddressPresenterCallback useCaseCallback;
        double latitude;
        double longitude;
        RootObject myAddress = null;
        public GetAddressUseCase(double lat, double lon)
        {
            this.latitude = lat;
            this.longitude = lon;
        }
        internal override async Task Action()
        {
            
            
            IGetAddress address = new OpenAddressProxy();
            
            try
            {
                System.Diagnostics.Debug.WriteLine("In API use case");
                await address.GetAddressAsync(latitude,longitude,this);
                
                
            }
            catch (Exception e)
            {
                
                System.Diagnostics.Debug.WriteLine("API EXCEPTION" + e.Message);
                useCaseCallback.DataReadFail();
            }

            if(myAddress!=null)
                useCaseCallback.DataReadFromAPISuccess(myAddress);
            else
                useCaseCallback.DataReadFail();


        }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (GetAddressPresenterCallback)p;
        }

        public bool ReadFromAPISuccess(RootObject root)
        {
            this.myAddress = root;
            return true;
        }

        public bool ReadFromAPIFail()
        {
            return false;
        }
    }
}

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
using Windows.Devices.Geolocation;

namespace DocApp.Domain.UseCase
{
    public class GetAddressUseCase: UseCaseBase,IAddressCallBack
    {
       
        IGetAddressPresenterCallback useCaseCallback;
        double latitude;
        double longitude;
        RootObject myAddress = null;
        //public GetAddressUseCase(double lat, double lon)
        //{
        //    this.latitude = lat;
        //    this.longitude = lon;
        //}
        public async Task<Geoposition> GetPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
            var pos = await geolocator.GetGeopositionAsync();
            return pos;
        }



        internal override async Task Action()
        {

            var temp = await GetPosition();
            double latitude = temp.Coordinate.Latitude;
            double longitude = temp.Coordinate.Longitude;
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
            this.useCaseCallback = (IGetAddressPresenterCallback)p;
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

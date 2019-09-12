using DocApp.Domain.UseCase;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace DocApp.Presentation.ViewModels
{
    public class LocationEventArgs : EventArgs
    {
        public string address { get; set; }
    }
    public class AutoSuggestViewModel: IGetLocationPresenterCallback, IGetAddressPresenterCallback, IDepartmentViewCallback,
        INotifyPropertyChanged
    {
        public double latitude;
        public double longitude;
        public ObservableCollection<string> localities;
        public ObservableCollection<string> depts;
        public UseCaseBase getLocalityList;
        public UseCaseBase getDepts;
        private string location = "Current Location";
        public UseCaseBase getAddress;
        public delegate void LocationChangedEventHandler(object source, LocationEventArgs e);
        public event LocationChangedEventHandler LocationChanged;
        public string loc
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                RaisePropertyChanged("loc");
            }
        }
        public string locbox = "";
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));


            }
        }

        public AutoSuggestViewModel()
        {
            depts = new ObservableCollection<string>();
        }

        public void onLocationChanged()
        {
            if (LocationChanged != null)
                LocationChanged(this, new LocationEventArgs { address = loc });
        }

        public async Task GetLocalities(string s)
        {
            localities = new ObservableCollection<string>();
            getLocalityList = new GetLocationsUseCase(s);
            getLocalityList.SetCallBack<IGetLocationPresenterCallback>(this);
            await getLocalityList.Execute();
        }

        public async Task<Geoposition> GetPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
            var pos = await geolocator.GetGeopositionAsync();
            return pos;
        }

        public async Task GetCurrentAddress()
        {
            var temp = await GetPosition();
            latitude = temp.Coordinate.Latitude;
            longitude = temp.Coordinate.Longitude;

            getAddress = new GetAddressUseCase(latitude, longitude);
            

            //getDoctor.SetCallBack<DoctorViewCallback>(this);

            getAddress.SetCallBack<IGetAddressPresenterCallback>(this);

            try
            {


                await getAddress.Execute();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }

        }

        public async Task GetDepartments()
        {
            getDepts = new GetDeptsUseCase();
            getDepts.SetCallBack<IDepartmentViewCallback>(this);
            await getDepts.Execute();
        }


        public bool DataFromPractoSuccess(RootLocationObject r)
        {
            System.Diagnostics.Debug.WriteLine("Viewmodel location success");
            localities.Clear();
            foreach (var x in r.results.@default.matches)
                localities.Add(x.suggestion);
            return true;
        }

        public bool DataFromPractoFail()
        {
            System.Diagnostics.Debug.WriteLine("Viewmodel location fail");
            return false;
        }



        public bool DataReadFromAPISuccess(RootObject r)
        {

            loc = r.address.neighbourhood;
            System.Diagnostics.Debug.WriteLine("location=" + location);
            onLocationChanged();
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("API Viewmodel fail");
            return false;
        }

        public bool DepartmentDataReadSuccess(List<Department> d)
        {
            
            foreach (var x in d)
                depts.Add(x.name);
            return true;
        }

        public bool DepartmentDataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Main page depts fail");
            return false;
        }
    }
}

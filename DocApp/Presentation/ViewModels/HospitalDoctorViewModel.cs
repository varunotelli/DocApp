using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Domain.Usecase;
using DocApp.Domain.UseCase;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using Windows.Devices.Geolocation;

namespace DocApp.Presentation.ViewModels
{



    class HospitalDoctorViewModel: HospitalViewCallback, DoctorViewCallback, GetAddressPresenterCallback,INotifyPropertyChanged
    {
        public double latitude;
        public double longitude;
        private string location = "CURRENT LOCATION";
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
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
                
                
            }
        }

        public ObservableCollection<Doctor> doctors;// = new ObservableCollection<Doctor>();
        public ObservableCollection<Hospital> hospitals;// = new ObservableCollection<Hospital>();
        public ObservableCollection<string> doctormain = new ObservableCollection<string>();
        public ObservableCollection<string> hospitalmain = new ObservableCollection<string>();
        public ObservableCollection<string> addresses = new ObservableCollection<string>();
        public UseCaseBase getDoctor;// = new GetDoctorListUseCase();
        public UseCaseBase getHospital;// = new GetHospitalByLocationUseCase();
        public UseCaseBase getAddress;
        public UseCaseBase getHospitalList;
        public HospitalDoctorViewModel()
        {
            doctors = new ObservableCollection<Doctor>();
            hospitals = new ObservableCollection<Hospital>();
            //addresses.Clear();
            //foreach (var x in hospitals.Select(x => x.Location).Distinct())
            //    addresses.Add(x);
        }

       

        public void onHospComboClicked(object source, EventArgs args)
        {
            getHospitalList = new GetHospitalListUseCase();
            getHospitalList.SetCallBack<HospitalViewCallback>(this);
            getHospitalList.Execute();
        }
        public void ondocComboClicked(object source, EventArgs args)
        {
            if(locbox.Equals(""))
                getDoctor = new GetDoctorByLocationUseCase(loc);
            else
                getDoctor = new GetDoctorByLocationUseCase(locbox);
            getDoctor.SetCallBack<DoctorViewCallback>(this);

            getDoctor.Execute();
        }

        public async Task<Geoposition> GetPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
            var pos = await geolocator.GetGeopositionAsync();
            return pos;
        }

        public async Task GetHospitalsDoctors()
        {
            var temp =await GetPosition();
            latitude = temp.Coordinate.Latitude;
            longitude = temp.Coordinate.Longitude;
            
            getAddress = new GetAddressUseCase(latitude, longitude);
            doctormain = new ObservableCollection<string>();
            hospitalmain = new ObservableCollection<string>();
            
            //getDoctor.SetCallBack<DoctorViewCallback>(this);
            
            getAddress.SetCallBack<GetAddressPresenterCallback>(this);

            try
            {

                
                await getAddress.Execute();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }
            


        }


        public bool DataReadSuccess(List<Doctor> d)
        {

            this.doctors = new ObservableCollection<Doctor>(d);
            doctormain.Clear();
            foreach (var x in this.doctors)
                this.doctormain.Add(x.Name);

            System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
            System.Diagnostics.Debug.WriteLine("success" + doctormain.Count());
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("FAILED!!!");
            return false;
        }

        public bool DataReadSuccess(ref List<Hospital> h)
        {
            string temp = "";
            if (locbox.Equals(""))
                temp = loc;
            else temp = locbox;
            hospitalmain.Clear();
            System.Diagnostics.Debug.WriteLine("h size="+h.Count());
            if (location.Equals("CURRENT LOCATION"))
            {
                foreach(var x in h)
                {
                    hospitals.Add(x);
                    hospitalmain.Add(x.Name);
                }
            }
            else
            {
                if(hospitals.Select(x=> x.Location).Contains(temp))
                //System.Diagnostics.Debug.WriteLine("IN else");
                    foreach(var x in h)
                    {
                        if (x.Location.Equals(temp))
                        {
                            this.hospitals.Add(x);
                            hospitalmain.Add(x.Name);
                        }

                    }
                
                
            }
            //hospitalmain.Clear();
            //foreach (var x in this.hospitals)
            //    this.hospitalmain.Add(x.Name);

            System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
            System.Diagnostics.Debug.WriteLine("success" + hospitalmain.Count());
            return true;
        }
        public bool DataReadFromAPISuccess(RootObject r)
        {

            loc = r.address.neighbourhood.ToUpper();
            System.Diagnostics.Debug.WriteLine("location="+location);
            
            return true;
        }



    }
}

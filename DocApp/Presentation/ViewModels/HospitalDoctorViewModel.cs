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
    


    class HospitalDoctorViewModel: HospitalViewCallback, DoctorViewCallback, HospitalLocationPresenterCallBack
        , DoctorLocationPresenterCallBack
    {
        public double latitude;
        public double longitude;
        


        public ObservableCollection<Doctor> doctors;// = new ObservableCollection<Doctor>();
        public ObservableCollection<Hospital> hospitals;// = new ObservableCollection<Hospital>();

        public UseCaseBase getDoctor;// = new GetDoctorListUseCase();
        public UseCaseBase getHospital;// = new GetHospitalByLocationUseCase();
        public UseCaseBase getHosps;
        public UseCaseBase getDocs;
        
        public HospitalDoctorViewModel()
        {
            doctors = new ObservableCollection<Doctor>();
            hospitals = new ObservableCollection<Hospital>();
            
        }

       
       
        public async Task GetHospitals(string s)
        {
            getHospital = new GetHospitalByNameUseCase(s);
            getHospital.SetCallBack<HospitalViewCallback>(this);
            await getHospital.Execute();
        }

        public async Task GetDoctors(string s)
        {
            getDoctor = new GetDoctorByNameUseCase(s);
            getDoctor.SetCallBack<DoctorViewCallback>(this);
            await getDoctor.Execute();
        }

        public async Task GetHospitalByLocation(string loc)
        {
            getHosps = new GetHospitalByLocationUseCase(loc.ToUpper());
            getHosps.SetCallBack<HospitalLocationPresenterCallBack>(this);
            await getHosps.Execute();
        }
        public async Task GetDoctorByLocation(string loc)
        {
            getDocs = new GetDoctorByLocationUseCase(loc.ToUpper());
            getDocs.SetCallBack<DoctorLocationPresenterCallBack>(this);
            await getDocs.Execute();
        }

        public bool DataReadSuccess(List<Hospital> h)
        {
            if(!hospitals.SequenceEqual(h))
            foreach (var x in h)
                this.hospitals.Add(x);
            

            System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
            
            return true;
        }
        public bool DataReadSuccess(List<Doctor> d)
        {
            if (!doctors.SequenceEqual(new ObservableCollection<Doctor>(d)))
            {
                
                foreach (var x in d)
                    doctors.Add(x);
            }
            else System.Diagnostics.Debug.WriteLine("EQUAL");
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("FAILED!!!");
            return false;
        }

        public bool HospitalLocationReadSuccess(List<Hospital> h)
        {
            System.Diagnostics.Debug.WriteLine("Hosp Location viewmodel success");
            foreach (var x in h)
                this.hospitals.Add(x);
            return true;
        }

        public bool HospitalLocationReadFail()
        {
            System.Diagnostics.Debug.WriteLine(" Hosp Location viewmodel fail");
            return false;
        }

        public bool DoctorLocationReadSuccess(List<Doctor> d)
        {
            System.Diagnostics.Debug.WriteLine(" Doc Location viewmodel success");
            foreach (var x in d)
                this.doctors.Add(x);
            return true;
        }

        public bool DoctorLocationReadFail()
        {
            System.Diagnostics.Debug.WriteLine(" Doc Location viewmodel fail");
            return false;
        }
    }
}

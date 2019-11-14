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
    public class DoctorSearchViewModel : DoctorDetailsAbstract, IDepartmentViewCallback,IDoctorDeptLocationViewCallback,IDoctorLocationPresenterCallBack,
        IHospitalByDeptViewCallback, IHospitalDetailViewCallBack,
        IHospitalLocationPresenterCallBack,  IDoctorHospitalDetailViewCallback,
        IHospitalRatingUpdateViewCallback, IGetAddressPresenterCallback
    {

        //public AppointmentDetails app;
        //public ObservableCollection<string> deptnames;
        //public ObservableCollection<HospitalInDoctorDetails> hospitals;
        //public ObservableCollection<DoctorInHospitalDetails> Doctors;
        //public ObservableCollection<Roster> timeslots;
        //public ObservableCollection<Doctor> docsmain;
        //public ObservableCollection<Doctor> docs;
        //public ObservableCollection<Hospital> hosps;
        //public ObservableCollection<Hospital> hospsmain;
        //public ObservableCollection<TestDetails> tests;
        //public delegate void ReadSuccessEventHandler(object source, EventArgs args);
        //public ReadSuccessEventHandler DoctorReadSuccess;
        //public ReadSuccessEventHandler DoctorRatingUpdateSuccess;
        //public ReadSuccessEventHandler HospitalRatingUpdateSuccess;
        //public ReadSuccessEventHandler DoctorsSuccess;
        //public delegate void InsertSuccessEventHandler(object source, EventArgs e);
        //public event InsertSuccessEventHandler InsertSuccess;
        //public event InsertFailEventHandler TestimonialAddedSuccess;
        //public delegate void InsertFailEventHandler(object source, EventArgs e);
        //public event InsertFailEventHandler InsertFail;
        //public event InsertFailEventHandler TestimonialAddedFail;
        //public delegate void AppointmentReadEventHandler(object source, EventArgs e);
        //public event AppointmentReadEventHandler AppointmentRead;
        //public event AppointmentReadEventHandler AppointmentCheckSuccess;
        //public int ct = -1;

        //private Doctor doc;
        //public Doctor doctor
        //{
        //    get { return doc; }
        //    set
        //    {
        //        doc = value;
        //        RaisePropertyChanged("doctor");

        //    }
        //}
        public delegate void DeptReadEventHandler(object source, EventArgs args);
        public DeptReadEventHandler DeptRead;


        private Hospital h;
        public Hospital hospital
        {
            get { return h; }
            set
            {
                h = value;
                RaisePropertyChanged("hospital");

            }
        }
        



        public DoctorSearchViewModel()
        {
            deptnames = new ObservableCollection<string>();
            docsmain = new ObservableCollection<Doctor>();
            docs= new ObservableCollection<Doctor>();
            hospitals = new ObservableCollection<HospitalInDoctorDetails>();
            timeslots = new ObservableCollection<Roster>();
            tests = new ObservableCollection<TestDetails>();
            hosps = new ObservableCollection<Hospital>();
            hospsmain = new ObservableCollection<Hospital>();
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            
        }

        public void onDeptRead()
        {
            if (DeptRead != null)
                DeptRead(this, EventArgs.Empty);
        }

        public void onHospitalRatingUpdateSuccess()
        {
            if (HospitalRatingUpdateSuccess != null)
                HospitalRatingUpdateSuccess(this, EventArgs.Empty);
        }


        public async Task GetDepartments()
        {
            UseCaseBase getDepts = new GetDeptsUseCase();
            getDepts.SetCallBack<IDepartmentViewCallback>(this);
            await getDepts.Execute();
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
            //var temp = await GetPosition();
            //double latitude = temp.Coordinate.Latitude;
            //double longitude = temp.Coordinate.Longitude;

            UseCaseBase getAddress = new GetAddressUseCase();


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

        public async Task GetDoctors(string location)
        {
            UseCaseBase getDocs = new GetDoctorByLocationUseCase(location);
            getDocs.SetCallBack<IDoctorLocationPresenterCallBack>(this);
            await getDocs.Execute();
        }

        public async Task GetDoctorsByDept(string location, int dept, int lexp=-1, int uexp=200,  int rating=-1)
        {
            UseCaseBase getDocs = new GetDoctorByDeptLocationUseCase(location, dept,lexp, uexp, rating);
            getDocs.SetCallBack<IDoctorDeptLocationViewCallback>(this);
            await getDocs.Execute();
        }

        public async Task GetHospitals(string location)
        {
            UseCaseBase getHosp = new GetHospitalByLocationUseCase(location);
            getHosp.SetCallBack<IHospitalLocationPresenterCallBack>(this);
            await getHosp.Execute();
        }

        public async Task GetHospitalByDept(string location, int dept,int rating=-1)
        {
            UseCaseBase getHosp = new GetHospitalByDeptUseCase(location.ToUpper(), dept,rating);
            getHosp.SetCallBack<IHospitalByDeptViewCallback>(this);
            await getHosp.Execute();
        }

        public async Task GetHospital(int id)
        {
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            UseCaseBase getHospital = new GetHospitalUseCase(id);
            UseCaseBase getDoctorByHospital = new GetDoctorByHospitalUseCase(id);
            getHospital.SetCallBack<IHospitalDetailViewCallBack>(this);
            getDoctorByHospital.SetCallBack<IDoctorHospitalDetailViewCallback>(this);
            try
            {
                await getHospital.Execute();
                await getDoctorByHospital.Execute();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }
        }

        public async Task UpdateHospitalRating(int id, double rating)
        {
            UseCaseBase updateHospital = new UpdateHospitalRatingUseCase(id, rating);
            updateHospital.SetCallBack(this);
            await updateHospital.Execute();
        }


       



        public bool DepartmentDataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Department view fail");
            return false;
        }

        public bool DepartmentDataReadSuccess(List<Department> d)
        {
            deptnames.Add("ALL");
            foreach (var x in d)
                deptnames.Add(x.name);
            onDeptRead();
            return true;
        }

        public bool DeptLocationViewSuccess(List<Doctor> doctors)
        {
            docs.Clear();
            foreach (var x in doctors)
                docs.Add(x);
            onDoctorsSuccess();
            return true;
        }

        public bool DeptLocationViewFail()
        {
            onDoctorsSuccess();
            System.Diagnostics.Debug.WriteLine("Dept location view fail");
            return false;
        }

        public bool DoctorLocationReadSuccess(List<Doctor> doctors)
        {
            //docs = new ObservableCollection<Doctor>();
            
            docs.Clear();
            
            foreach (var x in doctors)
                docs.Add(x);
            onDoctorsSuccess();
            return true;
        }

        public bool DoctorLocationReadFail()
        {
           
            System.Diagnostics.Debug.WriteLine("Doc location view fail");
            return false;
        }

        

        public bool HospitalLocationReadFail()
        {
            return false;
        }

        public bool HospitalLocationReadSuccess(List<Hospital> h)
        {
            hosps.Clear();
            
            foreach (var x in h)
            {
                hosps.Add(x);
                
            }
            onHospsSuccess();
            return true;
        }

        public bool ReadViewFail()
        {
            return false;
        }

        public bool ReadViewSuccess(List<Hospital> h)
        {
            hosps.Clear();
            //hospsmain.Clear();
            foreach (var x in h)
            {
                hosps.Add(x);
                //hospsmain.Add(x);
            }
            onHospsSuccess();
            return true;
        }
        public bool DataReadSuccess(Hospital h)
        {
            this.hospital = h;
            //onHospitalRatingUpdateSuccess();
            return true;
        }

        
        public bool DataReadSuccess(List<DoctorInHospitalDetails> d)
        {
            Doctors.Clear();
            foreach (var x in d)
                Doctors.Add(x);
            return true;
        }

        public bool HospitalUpdateSuccess(Hospital h)
        {
            this.hospital = h;
            onHospitalRatingUpdateSuccess();
            return true;
        }

        public bool HospitalUpdateFail()
        {
            return false;
        }

       

        public bool DataReadFromAPISuccess(RootObject r)
        {
            throw new NotImplementedException();
        }
    }
}

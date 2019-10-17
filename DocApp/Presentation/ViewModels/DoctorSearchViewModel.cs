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
    public class DoctorSearchViewModel : IDepartmentViewCallback,IDoctorDeptLocationViewCallback,IDoctorLocationPresenterCallBack,
        INotifyPropertyChanged,IDoctorDetailViewCallBack,IDoctorRatingUpdateViewCallback,IHospitalDoctorViewCallBack,IRosterViewCallback,
        IAppBookingViewCallback,IAppByIDViewCallback,ITestDetailsViewCallback,ITestViewCallback,ILastTestViewCallback, IHospitalByDeptViewCallback, 
        IHospitalLocationPresenterCallBack, IHospitalDetailViewCallBack, IDoctorHospitalDetailViewCallback,
        IHospitalRatingUpdateViewCallback,ICheckAppointmentViewCallback, IGetAddressPresenterCallback
    {
        public UseCaseBase getDepts;
        public UseCaseBase getDocs;
        public UseCaseBase getDoc;
        public UseCaseBase updateDoc;
        public UseCaseBase getHosp;
        public UseCaseBase getTimeSlots;
        public UseCaseBase bookApp;
        public UseCaseBase getApp;
        public UseCaseBase getTest;
        public UseCaseBase addTest;
        public UseCaseBase getHosps;
        public UseCaseBase getHospital;
        public UseCaseBase getDoctorByHospital;
        public UseCaseBase updateHospital;
        public UseCaseBase checkApp;
        public AppointmentDetails app;
        public ObservableCollection<string> deptnames;
        public ObservableCollection<HospitalInDoctorDetails> hospitals;
        public ObservableCollection<DoctorInHospitalDetails> Doctors;
        public ObservableCollection<Roster> timeslots;
        public ObservableCollection<Doctor> docsmain;
        public ObservableCollection<Doctor> docs;
        public ObservableCollection<Hospital> hosps;
        public ObservableCollection<Hospital> hospsmain;
        public ObservableCollection<TestDetails> tests;
        public delegate void ReadSuccessEventHandler(object source, EventArgs args);
        public ReadSuccessEventHandler DoctorReadSuccess;
        public ReadSuccessEventHandler DoctorRatingUpdateSuccess;
        public ReadSuccessEventHandler HospitalRatingUpdateSuccess;
        public ReadSuccessEventHandler DoctorsSuccess;
        public delegate void InsertSuccessEventHandler(object source, EventArgs e);
        public event InsertSuccessEventHandler InsertSuccess;
        public event InsertFailEventHandler TestimonialAddedSuccess;
        public delegate void InsertFailEventHandler(object source, EventArgs e);
        public event InsertFailEventHandler InsertFail;
        public event InsertFailEventHandler TestimonialAddedFail;
        public delegate void AppointmentReadEventHandler(object source, EventArgs e);
        public event AppointmentReadEventHandler AppointmentRead;
        public event AppointmentReadEventHandler AppointmentCheckSuccess;
        public int ct = -1;

        private Doctor doc;
        public Doctor doctor
        {
            get { return doc; }
            set
            {
                doc = value;
                RaisePropertyChanged("doctor");

            }
        }
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
        private bool e;
        public bool enabled
        {
            get { return e; }
            set
            {
                e = value;
                RaisePropertyChanged("enabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));

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


        public void onAppCheckSuccess()
        {
            if (AppointmentCheckSuccess != null)
                AppointmentCheckSuccess(this, EventArgs.Empty);
        }
        

        public void onDoctorsSuccess()
        {
            if (DoctorsSuccess != null)
                DoctorsSuccess(this, EventArgs.Empty);
        }

        public void onDoctorReadSuccess()
        {
            if (DoctorReadSuccess != null)
                DoctorReadSuccess(this, EventArgs.Empty);
        }

        public void onHospitalRatingUpdateSuccess()
        {
            if (HospitalRatingUpdateSuccess != null)
                HospitalRatingUpdateSuccess(this, EventArgs.Empty);
        }

        public void onDoctorRatingUpdateSuccess()
        {
            if (DoctorRatingUpdateSuccess != null)
                DoctorRatingUpdateSuccess(this, EventArgs.Empty);
        }
        public void onInsertSuccess()
        {
            if (InsertSuccess != null)
                InsertSuccess(this, EventArgs.Empty);
        }

        public void onTestimonialAddedSuccess()
        {
            if (TestimonialAddedSuccess != null)
                TestimonialAddedSuccess(this, EventArgs.Empty);
        }

        public void onInsertFail()
        {
            if (InsertFail != null)
                InsertFail(this, EventArgs.Empty);
        }

        public void onTestimonialAddedFail()
        {
            if (TestimonialAddedFail != null)
                TestimonialAddedFail(this, EventArgs.Empty);
        }

        public void onAppointmentRead()
        {
            if (AppointmentRead != null)
                AppointmentRead(this, EventArgs.Empty);
        }


        public async Task CheckApp(int p_id,string app_date, string time)
        {
            checkApp = new CheckAppointmentUseCase(p_id, app_date, time);
            checkApp.SetCallBack(this);
            await checkApp.Execute();
        }

        public async Task GetDoctor(int id)
        {
            
            getDoc = new GetDoctorUseCase(id);
            getHosp = new GetHospitalByDoctorUseCase(id);

            getDoc.SetCallBack<IDoctorDetailViewCallBack>(this);
            getHosp.SetCallBack<IHospitalDoctorViewCallBack>(this);
            try
            {

                await getDoc.Execute();
                await getHosp.Execute();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }
        }

        public async Task GetTimeSlots(int doc_id, int hosp_id, string app_date)
        {
            getTimeSlots = new GetRosterUseCase(doc_id, hosp_id,app_date);
            getTimeSlots.SetCallBack(this);
            await getTimeSlots.Execute();

        }

        public async Task UpdateDoctor(int id, double rating)
        {
            updateDoc = new UpdateDoctorRatingUseCase(id, rating);
            updateDoc.SetCallBack(this);
            try
            {
                await updateDoc.Execute();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("UPDATE VIEWMODEL EXCEPTION=" + e.Message);
            }
        }
        public async Task GetDepartments()
        {
            getDepts = new GetDeptsUseCase();
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
            getDocs = new GetDoctorByLocationUseCase(location);
            getDocs.SetCallBack<IDoctorLocationPresenterCallBack>(this);
            await getDocs.Execute();
        }

        public async Task GetDoctorsByDept(string location, int dept, int lexp=-1, int uexp=200,  int rating=-1)
        {
            getDocs = new GetDoctorByDeptLocationUseCase(location, dept+1,lexp, uexp, rating);
            getDocs.SetCallBack<IDoctorDeptLocationViewCallback>(this);
            await getDocs.Execute();
        }

        public async Task GetHospitals(string location)
        {
            getHosp = new GetHospitalByLocationUseCase(location);
            getHosp.SetCallBack<IHospitalLocationPresenterCallBack>(this);
            await getHosp.Execute();
        }

        public async Task GetHospitalByDept(string location, int dept)
        {
            getHosp = new GetHospitalByDeptUseCase(location.ToUpper(), dept + 1);
            getHosp.SetCallBack<IHospitalByDeptViewCallback>(this);
            await getHosp.Execute();
        }

        public async Task GetHospital(int id)
        {
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            getHospital = new GetHospitalUseCase(id);
            getDoctorByHospital = new GetDoctorByHospitalUseCase(id);
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
            updateHospital = new UpdateHospitalRatingUseCase(id, rating);
            updateHospital.SetCallBack(this);
            await updateHospital.Execute();
        }


        public async Task BookAppointment(int p_id, int doc_id, int hosp_id, string app_date, string start)
        {
            bookApp = new BookAppointmentUseCase(p_id, doc_id, hosp_id, app_date, start);
            bookApp.SetCallBack(this);
            try
            {
                await bookApp.Execute();
        
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Appointment EXCEPTION=" + e.Message);
            }

        }
        public async Task GetAppointment(string app_date, string start)
        {
            getApp = new GetAppointmentByIDUseCase(app_date, start);
            getApp.SetCallBack(this);
            try
            {

                await getApp.Execute();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get Appointment EXCEPTION=" + e.Message);
            }

        }

        public async Task GetTests(int id)
        {
            getTest = new GetTestDetailsUseCase(id);
            getTest.SetCallBack(this);
            try
            {
                await getTest.Execute();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get testimonials EXCEPTION=" + e.Message);
            }
        }

        public async Task AddTest(int pid,int doc,string msg, string time)
        {
            addTest = new AddTestimonialUseCase(pid, doc, msg, time);
            addTest.SetCallBack(this);
            try
            {
                await addTest.Execute();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Add testimonials view EXCEPTION=" + e.Message);
            }
        }

        public async Task GetLastTest(int doc)
        {
            getTest = new GetLastAddedTestUseCase(doc);
            getTest.SetCallBack(this);
            try
            {
                await getTest.Execute();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine("Add testimonials view EXCEPTION=" + e.Message);
            }
        }




        public bool DepartmentDataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Department view fail");
            return false;
        }

        public bool DepartmentDataReadSuccess(List<Department> d)
        {
            
            foreach (var x in d)
                deptnames.Add(x.name);
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
            
            return true;
        }

        public bool DoctorLocationReadFail()
        {
           
            System.Diagnostics.Debug.WriteLine("Doc location view fail");
            return false;
        }

        public bool DataReadSuccess(Doctor d)
        {
            this.doctor = d;
            //onDoctorReadSuccess();
            return true;
        }

        public bool DataReadFail()
        {
            return false;
        }

        public bool DoctorUpdateSuccess(Doctor d)
        {
            this.doctor = d;
            onDoctorRatingUpdateSuccess();
            return true;
        }

        public bool DoctorUpdateFail()
        {
            return false;
        }

        public bool DataReadSuccess(List<HospitalInDoctorDetails> h)
        {
             hospitals.Clear();
            
            foreach (var x in h)
                hospitals.Add(x);
            //onDoctorReadSuccess();
            return true;
        }

        public bool RosterViewReadSuccess(List<Roster> l)
        {
            timeslots.Clear();
            foreach (var x in l)
                timeslots.Add(x);
            return true;
        }

        public bool RosterViewReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Roster view fail");
            return false;
        }

        public bool AppViewReadSuccess(Appointment appointment)
        {
            //this.app = appointment;
            onInsertSuccess();
            return true;
        }

        public bool AppViewReadFail()
        {
            onInsertFail();
                return false;
        }

        public bool AppByIDViewSuccess(AppointmentDetails appointment)
        {
            this.app = appointment;
            onAppointmentRead();
            return true;
        }

        public bool AppByIDViewFail()
        {
            return false;
        }

        public bool TestDetailsReadViewSuccess(List<TestDetails> t)
        {
            //t.OrderByDescending(x => x.posted_time);
            tests.Clear();
            foreach (var x in t.OrderByDescending(x => x.posted_time))
            {
                //x.posted_time = DateTime.ParseExact(x.posted_time, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
                tests.Add(x);
            }
            

            return true;
        }

        public bool TestDetailsReadViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Testimonial view fail");
            return false;
        }

        public bool TestReadViewSucces()
        {
            onTestimonialAddedSuccess();
            return true;
        }

        public bool TestReadViewFail()
        {
            onTestimonialAddedFail();
            return false;
        }

        public bool LastTestViewSuccess(TestDetails detail)
        {
            //detail.posted_time = DateTime.ParseExact(detail.posted_time, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm");
            tests.Insert(0, detail);
            tests.OrderByDescending(x => x.posted_time);
            return true;
        }

        public bool LastTestViewFail()
        {
            return false;
        }

        public bool HospitalLocationReadFail()
        {
            return false;
        }

        public bool HospitalLocationReadSuccess(List<Hospital> h)
        {
            hosps.Clear();
            hospsmain.Clear();
            foreach (var x in h)
            {
                hosps.Add(x);
                hospsmain.Add(x);
            }
            
            return true;
        }

        public bool ReadViewFail()
        {
            return false;
        }

        public bool ReadViewSuccess(List<Hospital> h)
        {
            hosps.Clear();
            hospsmain.Clear();
            foreach (var x in h)
            {
                hosps.Add(x);
                hospsmain.Add(x);
            }
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

        public bool CheckAppointmentViewSuccess(int count)
        {
            ct = count;
            onAppCheckSuccess();
            return true;
        }

        public bool CheckAppointmentViewFail()
        {
            return false;
        }

        public bool DataReadFromAPISuccess(RootObject r)
        {
            throw new NotImplementedException();
        }
    }
}

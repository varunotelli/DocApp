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

namespace DocApp.Presentation.ViewModels
{
    public class DoctorSearchViewModel : IDepartmentViewCallback,IDoctorDeptLocationViewCallback,IDoctorLocationPresenterCallBack,
        INotifyPropertyChanged,IDoctorDetailViewCallBack,IDoctorRatingUpdateViewCallback,IHospitalDoctorViewCallBack,IRosterViewCallback,
        IAppBookingViewCallback,IAppByIDViewCallback,ITestDetailsViewCallback,ITestViewCallback,ILastTestViewCallback
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
        public AppointmentDetails app;
        public ObservableCollection<string> deptnames;
        public ObservableCollection<HospitalInDoctorDetails> hospitals;
        public ObservableCollection<Roster> timeslots;
        public ObservableCollection<Doctor> docs;
        public ObservableCollection<TestDetails> tests;
        public delegate void DoctorReadSuccessEventHandler(object source, EventArgs args);
        public DoctorReadSuccessEventHandler DoctorReadSuccess;
        public DoctorReadSuccessEventHandler DoctorRatingUpdateSuccess;
        public delegate void InsertSuccessEventHandler(object source, EventArgs e);
        public event InsertSuccessEventHandler InsertSuccess;
        public event InsertFailEventHandler TestimonialAddedSuccess;
        public delegate void InsertFailEventHandler(object source, EventArgs e);
        public event InsertFailEventHandler InsertFail;
        public event InsertFailEventHandler TestimonialAddedFail;
        public delegate void AppointmentReadEventHandler(object source, EventArgs e);
        public event AppointmentReadEventHandler AppointmentRead;

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
            docs= new ObservableCollection<Doctor>();
            hospitals = new ObservableCollection<HospitalInDoctorDetails>();
            timeslots = new ObservableCollection<Roster>();
            tests = new ObservableCollection<TestDetails>();
            
        }

        public void onDoctorReadSuccess()
        {
            if (DoctorReadSuccess != null)
                DoctorReadSuccess(this, EventArgs.Empty);
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

        public async Task GetDoctors(string location)
        {
            getDocs = new GetDoctorByLocationUseCase(location);
            getDocs.SetCallBack<IDoctorLocationPresenterCallBack>(this);
            await getDocs.Execute();
        }

        public async Task GetDoctorsByDept(string location, int dept)
        {
            getDocs = new GetDoctorByDeptLocationUseCase(location, dept+1);
            getDocs.SetCallBack<IDoctorDeptLocationViewCallback>(this);
            await getDocs.Execute();
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
            return true;
        }

        public bool DeptLocationViewFail()
        {
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
    }
}

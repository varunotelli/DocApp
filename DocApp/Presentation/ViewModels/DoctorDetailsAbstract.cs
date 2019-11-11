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
    public abstract class DoctorDetailsAbstract : INotifyPropertyChanged, IDoctorDetailViewCallBack, IDoctorRatingUpdateViewCallback,
        IHospitalDoctorViewCallBack, IRosterViewCallback, IAppByDocViewCallback,
        IAppBookingViewCallback, IAppByIDViewCallback, ITestDetailsViewCallback, ITestViewCallback, ILastTestViewCallback,
        
        ICheckAppointmentViewCallback
    {
        public AppointmentDetails app;
        public ObservableCollection<string> deptnames;
        public ObservableCollection<HospitalInDoctorDetails> hospitals;
        public ObservableCollection<DoctorInHospitalDetails> Doctors;
        public ObservableCollection<AppointmentDetails> details;
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

        public async Task CheckApp(int p_id, string app_date, string time)
        {
            UseCaseBase checkApp = new CheckAppointmentUseCase(p_id, app_date, time);
            checkApp.SetCallBack(this);
            await checkApp.Execute();
        }

        public async Task GetDoctor(int id)
        {

            UseCaseBase getDoc = new GetDoctorUseCase(id);
            UseCaseBase getHosp = new GetHospitalByDoctorUseCase(id);

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
            UseCaseBase getTimeSlots = new GetRosterUseCase(doc_id, hosp_id, app_date);
            getTimeSlots.SetCallBack(this);
            await getTimeSlots.Execute();

        }

        public async Task UpdateDoctor(int id, double rating)
        {
            UseCaseBase updateDoc = new UpdateDoctorRatingUseCase(id, rating);
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
        public async Task BookAppointment(int p_id, int doc_id, int hosp_id, string app_date, string start)
        {
            UseCaseBase bookApp = new BookAppointmentUseCase(p_id, doc_id, hosp_id, app_date, start);
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
            UseCaseBase getApp = new GetAppointmentByIDUseCase(app_date, start);
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
            UseCaseBase getTest = new GetTestDetailsUseCase(id);
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

        public async Task AddTest(int pid, int doc, string msg, string time)
        {
            UseCaseBase addTest = new AddTestimonialUseCase(pid, doc, msg, time);
            addTest.SetCallBack(this);
            try
            {
                await addTest.Execute();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Add testimonials view EXCEPTION=" + e.Message);
            }
        }

        public async Task GetLastTest(int doc)
        {
            UseCaseBase getTest = new GetLastAddedTestUseCase(doc);
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

        public async Task GetAppointmentByDoc(int p_id, int doc_id)
        {
            UseCaseBase getAppByDoc = new GetAppByDocUseCase(p_id, doc_id);
            getAppByDoc.SetCallBack(this);
            await getAppByDoc.Execute();

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
            onDoctorReadSuccess();
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

        public bool AppByDocViewSuccess(List<AppointmentDetails> appdetails)
        {
            details.Clear();
            foreach (var x in appdetails)
                details.Add(x);
            return true;
        }

        public bool AppByDocViewFail()
        {
            return false;
        }
    }
}

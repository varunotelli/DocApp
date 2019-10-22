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
        IHospitalDoctorViewCallBack, IRosterViewCallback,
        IAppBookingViewCallback, IAppByIDViewCallback, ITestDetailsViewCallback, ITestViewCallback, ILastTestViewCallback,
        IHospitalDetailViewCallBack, IDoctorHospitalDetailViewCallback,
        ICheckAppointmentViewCallback
    {
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


        public bool AppByIDViewFail()
        {
            throw new NotImplementedException();
        }

        public bool AppByIDViewSuccess(AppointmentDetails appointment)
        {
            throw new NotImplementedException();
        }

        public bool AppViewReadFail()
        {
            throw new NotImplementedException();
        }

        public bool AppViewReadSuccess(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public bool CheckAppointmentViewFail()
        {
            throw new NotImplementedException();
        }

        public bool CheckAppointmentViewSuccess(int count)
        {
            throw new NotImplementedException();
        }

        public bool DataReadFail()
        {
            throw new NotImplementedException();
        }

        public bool DataReadSuccess(Doctor d)
        {
            throw new NotImplementedException();
        }

        public bool DataReadSuccess(List<HospitalInDoctorDetails> d)
        {
            throw new NotImplementedException();
        }

        public bool DataReadSuccess(Hospital h)
        {
            throw new NotImplementedException();
        }

        public bool DataReadSuccess(List<DoctorInHospitalDetails> d)
        {
            throw new NotImplementedException();
        }

        public bool DoctorUpdateFail()
        {
            throw new NotImplementedException();
        }

        public bool DoctorUpdateSuccess(Doctor d)
        {
            throw new NotImplementedException();
        }

        public bool LastTestViewFail()
        {
            throw new NotImplementedException();
        }

        public bool LastTestViewSuccess(TestDetails detail)
        {
            throw new NotImplementedException();
        }

        public bool RosterViewReadFail()
        {
            throw new NotImplementedException();
        }

        public bool RosterViewReadSuccess(List<Roster> l)
        {
            throw new NotImplementedException();
        }

        public bool TestDetailsReadViewFail()
        {
            throw new NotImplementedException();
        }

        public bool TestDetailsReadViewSuccess(List<TestDetails> tests)
        {
            throw new NotImplementedException();
        }

        public bool TestReadViewFail()
        {
            throw new NotImplementedException();
        }

        public bool TestReadViewSucces()
        {
            throw new NotImplementedException();
        }
    }
}

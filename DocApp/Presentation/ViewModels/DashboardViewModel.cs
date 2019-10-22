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
    public class DashboardViewModel: IRecentDoctorViewCallback,IMostBookedDocViewCallback,IAppDisplayViewCallback, 
        INotifyPropertyChanged, IDoctorDetailViewCallBack, IDoctorRatingUpdateViewCallback, IHospitalDoctorViewCallBack, 
        IRosterViewCallback,
        IAppBookingViewCallback, IAppByIDViewCallback, ITestDetailsViewCallback, ITestViewCallback, ILastTestViewCallback, 
        IDoctorHospitalDetailViewCallback,
        ICheckAppointmentViewCallback, IGetAddressPresenterCallback
    {
        
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


        public ObservableCollection<Doctor> recent_docs;
        public ObservableCollection<Doctor> most_booked_docs;
        public ObservableCollection<AppointmentDetails> appointments;

        

        public DashboardViewModel()
        {
            this.recent_docs = new ObservableCollection<Doctor>();
            this.most_booked_docs = new ObservableCollection<Doctor>();
            this.appointments = new ObservableCollection<AppointmentDetails>();
            
            
            timeslots = new ObservableCollection<Roster>();
            tests = new ObservableCollection<TestDetails>();
            hosps = new ObservableCollection<Hospital>();
            hospsmain = new ObservableCollection<Hospital>();
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
        }
        public async Task GetRecentSearchDoctors(int id)
        {

            try
            {
                UseCaseBase getRecentDoctors = new GetRecentDoctorsUseCase(id);
                getRecentDoctors.SetCallBack(this);
                await getRecentDoctors.Execute();
            }

            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Recent doctor view fail " + e.Message);

            }
        }

        public async Task GetMostBookedDoc(int id)
        {
            try
            {
                UseCaseBase getMostBookedDoc = new GetMostBookedDoctorUseCase(id);
                getMostBookedDoc.SetCallBack(this);
                await getMostBookedDoc.Execute();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("most booked doc view exception " + e.Message);
            }
        }

        public async Task GetAppointments(int id)
        {
            UseCaseBase getApps = new GetAppointmentsUseCase(id);
            appointments = new ObservableCollection<AppointmentDetails>();
            getApps.SetCallBack(this);
            await getApps.Execute();
        }

        public bool MostBookedDocViewFail()
        {
            return false;
        }

        public bool MostBookedDocViewSuccess(List<Doctor> d)
        {
            most_booked_docs.Clear();
            foreach (var x in d)
                most_booked_docs.Add(x);
            return true;
        }

        public bool SearchDocViewFail()
        {
            return false;
        }

        public bool SearchDocViewSuccess(List<Doctor> doctors)
        {
            recent_docs.Clear();
            foreach (var x in doctors)
                recent_docs.Add(x);
            return true;
        }

        public bool GetAppsReadViewSuccess(List<AppointmentDetails> apps)
        {
            appointments.Clear();
            foreach (var x in apps)
                appointments.Add(x);
            return true;
        }

        public bool GetAppsReadViewFail()
        {
            return false;
        }

        public bool DataReadSuccess(Doctor d)
        {
            throw new NotImplementedException();
        }

        public bool DataReadFail()
        {
            throw new NotImplementedException();
        }

        public bool DoctorUpdateSuccess(Doctor d)
        {
            throw new NotImplementedException();
        }

        public bool DoctorUpdateFail()
        {
            throw new NotImplementedException();
        }

        public bool DataReadSuccess(List<HospitalInDoctorDetails> d)
        {
            throw new NotImplementedException();
        }

        public bool RosterViewReadSuccess(List<Roster> l)
        {
            throw new NotImplementedException();
        }

        public bool RosterViewReadFail()
        {
            throw new NotImplementedException();
        }

        public bool AppViewReadSuccess(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public bool AppViewReadFail()
        {
            throw new NotImplementedException();
        }

        public bool AppByIDViewSuccess(AppointmentDetails appointment)
        {
            throw new NotImplementedException();
        }

        public bool AppByIDViewFail()
        {
            throw new NotImplementedException();
        }

        public bool TestDetailsReadViewSuccess(List<TestDetails> tests)
        {
            throw new NotImplementedException();
        }

        public bool TestDetailsReadViewFail()
        {
            throw new NotImplementedException();
        }

        public bool TestReadViewSucces()
        {
            throw new NotImplementedException();
        }

        public bool TestReadViewFail()
        {
            throw new NotImplementedException();
        }

        public bool LastTestViewSuccess(TestDetails detail)
        {
            throw new NotImplementedException();
        }

        public bool LastTestViewFail()
        {
            throw new NotImplementedException();
        }

        public bool DataReadSuccess(List<DoctorInHospitalDetails> d)
        {
            throw new NotImplementedException();
        }

        public bool CheckAppointmentViewSuccess(int count)
        {
            throw new NotImplementedException();
        }

        public bool CheckAppointmentViewFail()
        {
            throw new NotImplementedException();
        }

        public bool DataReadFromAPISuccess(RootObject r)
        {
            throw new NotImplementedException();
        }
    }
}

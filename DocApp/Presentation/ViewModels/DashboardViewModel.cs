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
    public class DashboardViewModel: DoctorDetailsAbstract, IRecentDoctorViewCallback,IMostBookedDocViewCallback,
        IAppDisplayViewCallback, ILastHospitalViewCallback,ICancelAppViewCallback,IUpdateAppViewCallback,
        IAppViewCallback
    {
       
        public ObservableCollection<Doctor> recent_docs;
        public ObservableCollection<Doctor> most_booked_docs;
        public ObservableCollection<AppointmentDetails> appointments;
        public Appointment app_vals;
        public ObservableCollection<HospitalInDoctorDetails> hospsinbox;
        public Hospital LastBookedHosp;
        public delegate void LastHospBookedEventHandler(object souce, EventArgs args);
        public event LastHospBookedEventHandler LastHospBooked;
        public delegate void AppointmentEventHandler(object source, EventArgs args);
        public event AppointmentEventHandler AppointmentUpdated;
        public event AppointmentReadEventHandler AppRead;
        public delegate void NoRecordEventHandler(object source, EventArgs args);
        public event NoRecordEventHandler NoRecord;
        public event NoRecordEventHandler NoDoctor;
        bool flag;
        DateTimeOffset d;
        public DateTimeOffset date
        {
            get { return d; }
            set
            {
                d = value;
                RaisePropertyChanged("date");
            }
        }

        public DashboardViewModel()
        {
            this.recent_docs = new ObservableCollection<Doctor>();
            this.most_booked_docs = new ObservableCollection<Doctor>();
            this.appointments = new ObservableCollection<AppointmentDetails>();

            hospitals = new ObservableCollection<HospitalInDoctorDetails>();
            timeslots = new ObservableCollection<Roster>();
            tests = new ObservableCollection<TestDetails>();
            hosps = new ObservableCollection<Hospital>();
            hospsmain = new ObservableCollection<Hospital>();
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            hospsinbox = new ObservableCollection<HospitalInDoctorDetails>();
        }

        public void onAppRead()
        {
            if (AppRead != null)
                AppRead(this, EventArgs.Empty);

        }


        public void onNoRecord()
        {
            if (NoRecord != null)
                NoRecord(this, EventArgs.Empty);
        }

        public void onNoDoctor()
        {
            if (NoDoctor != null)
                NoDoctor(this, EventArgs.Empty);
        }

        public void onAppointmentUpdated()
        {
            if (AppointmentUpdated != null)
                AppointmentUpdated(this, EventArgs.Empty);
        }

        public void onLastHospBooked()
        {
            if (LastHospBooked != null)
                LastHospBooked(this, EventArgs.Empty);
        }

        public async Task GetLastHospital(int p_id, int doc_id)
        {
            try
            {
                UseCaseBase lastHosp = new GetLastHospitalUseCase(p_id, doc_id);
                lastHosp.SetCallBack(this);
                await lastHosp.Execute();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Last hospital view exception=" + e.Message);
            }
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
        public async Task GetApp(int x)
        {
            UseCaseBase getApp = new GetAppUseCase(x);
            getApp.SetCallBack(this);
            await getApp.Execute();
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

        public async Task CancelApp(int id)
        {
            UseCaseBase cancelApp = new CancelAppointmentUseCase(id);
            cancelApp.SetCallBack(this);
            await cancelApp.Execute();
        }

        public async Task UpdateApp(int x, string a, string t)
        {
            UseCaseBase updateApp = new UpdateAppUseCase(x, a, t);
            var item = appointments.FirstOrDefault(a1 => a1.id == app_vals.ID);
            var index = appointments.IndexOf(item);
            if (item != null)
            {
                item.app_date = a;
                item.Timeslot = t;
            }
            appointments[index] = item;
            //apps.OrderBy(a1 => a1.app_date).ThenBy(a1 => a1.Timeslot);
            updateApp.SetCallBack(this);
            await updateApp.Execute();
        }

        public bool MostBookedDocViewFail()
        {
            return false;
        }

        public bool MostBookedDocViewSuccess(List<Doctor> d)
        {
            most_booked_docs.Clear();
            foreach (var x in d.Take(4))
                most_booked_docs.Add(x);
            if (d.Count() == 0)
                onNoDoctor();
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
            if (apps.Count() == 0)
                onNoRecord();
            return true;
        }

        public bool GetAppsReadViewFail()
        {
            return false;
        }

        public bool LastHospitalViewSuccess(Hospital hospital)
        {
            this.LastBookedHosp = hospital;
            onLastHospBooked();
            return true;
        }

        public bool LastHospitalViewFail()
        {
            return false;
        }

        public bool CancelAppViewSuccess(bool f)
        {
            this.flag = f;
            return true;
        }

        public bool CancelAppViewFail()
        {
            return false;
        }

        public bool UpdateAppViewSuccess(bool flag)
        {
            var templist = new List<AppointmentDetails>(appointments.OrderBy(x => x.app_date).ThenBy(x => x.Timeslot));
            appointments.Clear();
            foreach (var x in templist)
                appointments.Add(x);
            onAppointmentUpdated();
            return true;
        }

        public bool UpdateAppViewFail()
        {
            return false;
        }

        public bool AppViewSuccess(Appointment a)
        {
            this.app_vals = a;
            this.date = DateTimeOffset.Parse(a.APP_DATE);
            onAppRead();
            return true;
        }

        public bool AppViewFail()
        {
            return false;
        }
    }
}

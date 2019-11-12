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
    public class SelectedDoctorViewModel : DoctorDetailsAbstract, ICancelAppViewCallback, IUpdateAppViewCallback,
        IAppViewCallback
    {
        public Hospital hospital;
        public Appointment app_vals;
        public ObservableCollection<AppointmentDetails> appointments;
        public delegate void AppointmentEventHandler(object source, EventArgs args);
        public event AppointmentEventHandler AppointmentUpdated;
        public event AppointmentReadEventHandler AppRead;
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

        public SelectedDoctorViewModel()
        {
            deptnames = new ObservableCollection<string>();
            docsmain = new ObservableCollection<Doctor>();
            docs = new ObservableCollection<Doctor>();
            hospitals = new ObservableCollection<HospitalInDoctorDetails>();
            timeslots = new ObservableCollection<Roster>();
            tests = new ObservableCollection<TestDetails>();
            hosps = new ObservableCollection<Hospital>();
            hospsmain = new ObservableCollection<Hospital>();
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            details = new ObservableCollection<AppointmentDetails>();
            appointments = new ObservableCollection<AppointmentDetails>();
            hospital = new Hospital();
        }

        public async Task GetApp(int x)
        {
            UseCaseBase getApp = new GetAppUseCase(x);
            getApp.SetCallBack(this);
            await getApp.Execute();
        }

        public void onAppRead()
        {
            if (AppRead != null)
                AppRead(this, EventArgs.Empty);

        }


        
        public void onAppointmentUpdated()
        {
            if (AppointmentUpdated != null)
                AppointmentUpdated(this, EventArgs.Empty);
        }

        public bool AppViewFail()
        {
            throw new NotImplementedException();
        }

        public bool AppViewSuccess(Appointment a)
        {
            this.app_vals = a;
            this.date = DateTimeOffset.Parse(a.APP_DATE);
            onAppRead();
            return true;
        }

        public async Task CancelApp(int id)
        {
            UseCaseBase cancelApp = new CancelAppointmentUseCase(id);
            cancelApp.SetCallBack(this);
            await cancelApp.Execute();
        }

        public bool CancelAppViewFail()
        {
            return false;
        }

        public bool CancelAppViewSuccess(bool flag)
        {
            var templist = new List<AppointmentDetails>(appointments.OrderBy(x => x.app_date).ThenBy(x => x.Timeslot));
            appointments.Clear();
            foreach (var x in templist)
                appointments.Add(x);
            //onAppointmentUpdated();
            return true;
        }

        public async Task UpdateApp(int x, string a, string t)
        {
            UseCaseBase updateApp = new UpdateAppUseCase(x, a, t);
            var item = details.FirstOrDefault(a1 => a1.id == app_vals.ID);
            var index = details.IndexOf(item);
            if (item != null)
            {
                item.app_date = a;
                item.Timeslot = t;
            }
            details[index] = item;
            //apps.OrderBy(a1 => a1.app_date).ThenBy(a1 => a1.Timeslot);
            updateApp.SetCallBack(this);
            await updateApp.Execute();
        }

        public bool UpdateAppViewFail()
        {
            return false;
        }

        public bool UpdateAppViewSuccess(bool flag)
        {
            onAppointmentUpdated();
            return true;
        }
    }
}

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
    public class AppointmentDisplayViewModel : IAppDisplayViewCallback,IUpcomingAppViewCallback,ICancelAppViewCallback,
        IRosterViewCallback, ICheckAppointmentViewCallback,IAppViewCallback,IUpdateAppViewCallback,INotifyPropertyChanged
    {
        UseCaseBase getApps;
        bool flag;
        DateTimeOffset d;
        
        public delegate void AppointmentReadEventHandler(object source, EventArgs e);
        public event AppointmentReadEventHandler AppointmentCheckSuccess;
        public event AppointmentReadEventHandler AppointmentReadSuccess;
        public event AppointmentReadEventHandler AppointmentUpdateSuccess;
        public int ct = -1;
        public ObservableCollection<AppointmentDetails> apps;
        public ObservableCollection<Roster> timeslots;
        public Appointment app;
        public DateTimeOffset date {
            get { return d; }
            set
            {
                d = value;
                RaisePropertyChanged("date");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));


            }
        }


        public AppointmentDisplayViewModel()
        {
            apps = new ObservableCollection<AppointmentDetails>();
            timeslots = new ObservableCollection<Roster>();
        }

        public void onAppCheckSuccess()
        {
            if (AppointmentCheckSuccess != null)
                AppointmentCheckSuccess(this, EventArgs.Empty);
        }

        public void onAppReadSuccess()
        {
            if (AppointmentReadSuccess != null)
                AppointmentReadSuccess(this, EventArgs.Empty);
        }

        public void onAppUpdateSuccess()
        {
            if (AppointmentUpdateSuccess != null)
                AppointmentUpdateSuccess(this, EventArgs.Empty);
        }

        public async Task GetAppointments(int id)
        {
            getApps = new GetAppointmentsUseCase(id);
            apps = new ObservableCollection<AppointmentDetails>();
            getApps.SetCallBack(this);
            await getApps.Execute();
        }

        public async Task GetTimeSlots(int doc_id, int hosp_id, string app_date)
        {
            UseCaseBase getTimeSlots = new GetRosterUseCase(doc_id, hosp_id, app_date);
            getTimeSlots.SetCallBack(this);
            await getTimeSlots.Execute();

        }

        public async Task GetUpcomingApps(int id)
        {
            UseCaseBase getApps = new GetUpcomingAppsUseCase(id);
            apps = new ObservableCollection<AppointmentDetails>();
            getApps.SetCallBack(this);
            await getApps.Execute();
        }

        public async Task CancelApp(int id)
        {
            UseCaseBase cancelApp = new CancelAppointmentUseCase(id);
            cancelApp.SetCallBack(this);
            await cancelApp.Execute();
        }

        public async Task CheckApp(int p_id, string app_date, string time)
        {
            UseCaseBase checkApp = new CheckAppointmentUseCase(p_id, app_date, time);
            checkApp.SetCallBack(this);
            await checkApp.Execute();
        }

        public async Task GetApp(int x)
        {
            UseCaseBase getApp = new GetAppUseCase(x);
            getApp.SetCallBack(this);
            await getApp.Execute();
        }

        public async Task UpdateApp(int x,string a, string t)
        {
            UseCaseBase updateApp = new UpdateAppUseCase(x,a,t);
            var item = apps.FirstOrDefault(a1 => a1.id == app.ID);
            var index = apps.IndexOf(item);
            if(item!=null)
            {
                item.app_date = a;
                item.Timeslot = t;
            }
            apps[index] = item;
            //apps.OrderBy(a1 => a1.app_date).ThenBy(a1 => a1.Timeslot);
            updateApp.SetCallBack(this);
            await updateApp.Execute();
        }

        public bool GetAppsReadViewFail()
        {
            return false;
        }

        public bool GetAppsReadViewSuccess(List<AppointmentDetails> appointments)
        {
            apps.Clear();
            foreach (var x in appointments)
                apps.Add(x);
            return true;
        }

        public bool UpcomingAppViewSuccess(List<AppointmentDetails> appointments)
        {
            apps.Clear();
            foreach (var x in appointments)
                apps.Add(x);
            return true;
        }

        public bool UpcomingAppViewFail()
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

        public bool AppViewSuccess(Appointment a)
        {
            this.app = a;
            this.date = DateTimeOffset.Parse(a.APP_DATE);
            onAppReadSuccess();
            return true;
        }

        public bool AppViewFail()
        {
            return false;
        }

        public bool UpdateAppViewSuccess(bool flag)
        {
            var templist = new List<AppointmentDetails>(apps.OrderBy(x=>x.app_date).ThenBy(x=>x.Timeslot));
            apps.Clear();
            foreach (var x in templist)
                apps.Add(x);
            onAppUpdateSuccess();
            return true;
        }

        public bool UpdateAppViewFail()
        {
            return false;
        }
    }
}

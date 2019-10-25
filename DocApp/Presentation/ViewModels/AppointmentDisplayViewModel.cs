using DocApp.Domain.UseCase;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.ViewModels
{
    public class AppointmentDisplayViewModel : IAppDisplayViewCallback,IUpcomingAppViewCallback,ICancelAppViewCallback,
        IRosterViewCallback, ICheckAppointmentViewCallback,IAppViewCallback
    {
        UseCaseBase getApps;
        bool flag;
        public delegate void AppointmentReadEventHandler(object source, EventArgs e);
        public event AppointmentReadEventHandler AppointmentCheckSuccess;
        public event AppointmentReadEventHandler AppointmentReadSuccess;
        public int ct = -1;
        public ObservableCollection<AppointmentDetails> apps;
        public ObservableCollection<Roster> timeslots;
        public Appointment app;
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


        public async Task GetAppointments(int id)
        {
            getApps = new GetAppointmentsUseCase(id);
            apps = new ObservableCollection<AppointmentDetails>();
            getApps.SetCallBack(this);
            await getApps.Execute();
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

        public async Task GetTimeSlots(int doc_id, int hosp_id, string app_date)
        {
            UseCaseBase getTimeSlots = new GetRosterUseCase(doc_id, hosp_id, app_date);
            getTimeSlots.SetCallBack(this);
            await getTimeSlots.Execute();

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
            onAppReadSuccess();
            return true;
        }

        public bool AppViewFail()
        {
            return false;
        }
    }
}

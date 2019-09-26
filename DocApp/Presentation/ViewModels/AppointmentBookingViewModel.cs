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
    public class AppointmentBookingViewModel: IAppBookingViewCallback, IDoctorDetailViewCallBack, IHospitalDoctorViewCallBack,
        IRosterViewCallback, INotifyPropertyChanged
    {
        UseCaseBase bookApp;
        UseCaseBase getDoctor;
        UseCaseBase getHosps;
        UseCaseBase getTimeSlots;
        public delegate void InsertSuccessEventHandler(object source, EventArgs e);
        public event InsertSuccessEventHandler InsertSuccess;
        public delegate void InsertFailEventHandler(object source, EventArgs e);
        public event InsertFailEventHandler InsertFail;
        public ObservableCollection<HospitalInDoctorDetails> hosps;
        public ObservableCollection<Roster> timeslots;
        public bool flag = false;
        int doc;
        string d;
        public string doc_name
        {
            get
            {
                return this.d;
            }
            set
            {
                this.d = value;
                RaisePropertyChanged("doc_name");
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

        public void onInsertSuccess()
        {
            if (InsertSuccess != null)
                InsertSuccess(this, EventArgs.Empty);
        }
        public void onInsertFail()
        {
            if (InsertFail != null)
                InsertFail(this, EventArgs.Empty);
        }

        public AppointmentBookingViewModel(int n)
        {
            this.timeslots = new ObservableCollection<Roster>();
            this.hosps = new ObservableCollection<HospitalInDoctorDetails>();
            this.doc = n
;        }

       

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

        public async Task GetDoctor(int id)
        {
            getDoctor = new GetDoctorUseCase(id);
            getDoctor.SetCallBack(this);
            await getDoctor.Execute();
        }

        public async Task GetHospitals(int id)
        {
            getHosps = new GetHospitalByDoctorUseCase(id);
            getHosps.SetCallBack(this);
            await getHosps.Execute();
        }

        public async Task GetTimeSlots(int doc_id, int hosp_id)
        {
            getTimeSlots = new GetRosterUseCase(doc_id, hosp_id);
            getTimeSlots.SetCallBack(this);
            await getTimeSlots.Execute();

        }

        public bool AppViewReadFail()
        {
            onInsertFail();
            flag = false;
            System.Diagnostics.Debug.WriteLine("Appointment insert view fail");
            return false;
        }

        public bool AppViewReadSuccess(Appointment appointment)
        {
            onInsertSuccess();
            flag = true;
            System.Diagnostics.Debug.WriteLine("Appointment insert view success");
            return true;
        }

        public bool DataReadSuccess(Doctor d)
        {
            this.doc_name = d.Name;
            System.Diagnostics.Debug.WriteLine("Name=" + doc_name);
            return true;
        }

        public bool DataReadFail()
        {
            return false;
        }

        public bool DataReadSuccess(List<HospitalInDoctorDetails> h)
        {
            hosps.Clear();
            foreach (var x in h)
                hosps.Add(x);
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
    }
}

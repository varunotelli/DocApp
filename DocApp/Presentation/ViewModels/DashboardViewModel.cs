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
        IAppDisplayViewCallback, ILastHospitalViewCallback
    {
       
        public ObservableCollection<Doctor> recent_docs;
        public ObservableCollection<Doctor> most_booked_docs;
        public ObservableCollection<AppointmentDetails> appointments;

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

        public bool LastHospitalViewSuccess(Hospital hospital)
        {
            throw new NotImplementedException();
        }

        public bool LastHospitalViewFail()
        {
            throw new NotImplementedException();
        }
    }
}

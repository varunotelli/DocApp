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
    public class SelectedDoctorViewModel : DoctorDetailsAbstract
    {
        public Hospital hospital;
        
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
            
            hospital = new Hospital();
        }

        

        
    }
}

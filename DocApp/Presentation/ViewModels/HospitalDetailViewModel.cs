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
    public class HospitalDetailViewModel: HospitalDetailCallBack, DoctorHospitalDetailViewCallback,DoctorDetailViewCallBack,
        INotifyPropertyChanged
    {
        
        protected string name1 = "";
        public string rating = "";
        public string num_rating = "";
        public Hospital hospital;
        public ObservableCollection<DoctorInHospitalDetails> Doctors;
        //public ObservableCollection<DoctorInHospitalDetails> doc= new ObservableCollection<DoctorInHospitalDetails>();
        public ObservableCollection<string> doctormain;
        private Doctor d;
        public Doctor doc
        {
            get { return d; }
            set
            {
                d = value;
                RaisePropertyChanged("doc");

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

        public HospitalDetailViewModel(string n)
        {
            this.name1 = n;
            doc = new Doctor();
            //this.Doctors = new ObservableCollection<Doctor>();
           
        }

        UseCaseBase getHospital;
        UseCaseBase getDoctorByHospital;
        UseCaseBase getDoctorProfile;
        public async Task GetHospitals()
        {
           
            getHospital = new GetHospitalDetail(name1);
            getDoctorByHospital = new GetDoctorByHospitalUseCase(name1);
            doctormain = new ObservableCollection<string>();
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
           
            //getHospital = new GetHospitalListUseCase();
            //hospitals = new ObservableCollection<Hospital>();
            getHospital.SetCallBack<HospitalDetailCallBack>(this);
            getDoctorByHospital.SetCallBack<DoctorHospitalDetailViewCallback>(this);

            try
            {

                await getHospital.Execute();
                await getDoctorByHospital.Execute();
                //System.Diagnostics.Debug.WriteLine("view model count=" + hospital.Location);
                //SetHospital();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }




        }

        public async Task GetDoctorDetails(string docname)
        {
            //doc = new Doctor();
            getDoctorProfile = new GetDoctorDetailUseCase(docname);
            getDoctorProfile.SetCallBack<DoctorDetailViewCallBack>(this);
            await getDoctorProfile.Execute();
        }

        public bool DataReadSuccess(ref Hospital h)
        {
            //this.hospital = new Hospital();
            this.hospital = h;


            System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
            System.Diagnostics.Debug.WriteLine("location=" + this.hospital);
            return true;
        }

        public bool DataReadSuccess(List<DoctorInHospitalDetails> d)
        {
            //Doctors = new ObservableCollection<Doctor>(d);
            //this.Doctors = d;
            foreach (var x in d)
            {
                Doctors.Add(x);
                doctormain.Add(x.Name);

            }
            //System.Diagnostics.Debug.WriteLine("Doctor "+Doctors[0].Name);
            System.Diagnostics.Debug.WriteLine("Doctor SUCCESS!!!");
            System.Diagnostics.Debug.WriteLine("Doctors size="+Doctors.Count);
            //doc.Add(Doctors[0]);
           
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor FAILED!!!");
            return false;
        }

        bool DoctorDetailViewCallBack.DataReadSuccess(Doctor d)
        {
            this.doc = d;
           
            System.Diagnostics.Debug.WriteLine("Doctor info success");
            
            return true;
        }

        bool DoctorDetailViewCallBack.DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor info fail");
            return false;
        }
    }
}

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
    public class HospitalDetailViewModel: IHospitalDetailCallBack, IDoctorHospitalDetailViewCallback,IDoctorDetailViewCallBack,
        INotifyPropertyChanged
    {

        int id;
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

        public HospitalDetailViewModel(int n)
        {
            this.id = n;
           
        }

        UseCaseBase getHospital;
        UseCaseBase getDoctorByHospital;
        UseCaseBase getDoctorProfile;
        public async Task GetHospital()
        {
           
            //getHospital = new GetHospitalUseCase(id,this);
            
           
            //getHospital = new GetHospitalListUseCase();
            //hospitals = new ObservableCollection<Hospital>();
            getHospital.SetCallBack<IHospitalDetailCallBack>(this);
            getDoctorByHospital.SetCallBack<IDoctorHospitalDetailViewCallback>(this);

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
            getDoctorProfile = new GetDoctorByNameUseCase(docname);
            getDoctorProfile.SetCallBack<IDoctorDetailViewCallBack>(this);
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

        bool IDoctorDetailViewCallBack.DataReadSuccess(Doctor d)
        {
            this.doc = d;
           
            System.Diagnostics.Debug.WriteLine("Doctor info success");
            
            return true;
        }

        bool IDoctorDetailViewCallBack.DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor info fail");
            return false;
        }

        public bool DoctorDetailViewSuccess(Doctor d)
        {
            this.doc = d;
            return true;
        }

        public bool DoctorDetailViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor update fail");
            return false;
        }

        public bool DataReadSuccess(List<Hospital> h)
        {
            throw new NotImplementedException();
        }
    }
}

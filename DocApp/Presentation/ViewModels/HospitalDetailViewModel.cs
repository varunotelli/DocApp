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
    public class HospitalDetailViewModel: HospitalDetailCallBack, DoctorHospitalDetailViewCallback
    {
        
        protected string name1 = "";
        public Hospital hospital;
        public ObservableCollection<DoctorInHospitalDetails> Doctors;
        //public ObservableCollection<DoctorInHospitalDetails> doc= new ObservableCollection<DoctorInHospitalDetails>();
        public ObservableCollection<string> doctormain;
        
        public HospitalDetailViewModel(string n)
        {
            this.name1 = n;
            //this.Doctors = new ObservableCollection<Doctor>();
           
        }

        UseCaseBase getHospital;
        UseCaseBase getDoctorByHospital;
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
    }
}

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
    public class HospitalDetailViewModel: IHospitalDetailViewCallBack, IDoctorHospitalDetailViewCallback,
        INotifyPropertyChanged
    {

        //int id;
        public string rating = "";
        public string num_rating = "";
        
        public ObservableCollection<DoctorInHospitalDetails> Doctors;
        //public ObservableCollection<DoctorInHospitalDetails> doc= new ObservableCollection<DoctorInHospitalDetails>();
        public ObservableCollection<string> doctormain;
        private Hospital h;
        public Hospital hospital
        {
            get { return h; }
            set
            {
                h = value;
                RaisePropertyChanged("hospital");

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

        public HospitalDetailViewModel()
        {
            //this.id = n;
           
            hospital = new Hospital();
        }

        UseCaseBase getHospital;
        UseCaseBase getDoctorByHospital;

        public async Task GetHospital(int id)
        {
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            getHospital = new GetHospitalUseCase(id);
            getDoctorByHospital = new GetDoctorByHospitalUseCase(id);
            getHospital.SetCallBack<IHospitalDetailViewCallBack>(this);
            getDoctorByHospital.SetCallBack<IDoctorHospitalDetailViewCallback>(this);
            try
            {
                await getHospital.Execute();
                await getDoctorByHospital.Execute();               
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }
        }

        public bool DataReadSuccess(Hospital h)
        {
            this.hospital = h;
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("HospitalDetailview fail");
            return false;
        }

        public bool DataReadSuccess(List<DoctorInHospitalDetails> d)
        {
            //Doctors.Clear();
            foreach (var x in d)
                Doctors.Add(x);
            return true;
        }
    }
}

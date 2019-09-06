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
    class DoctorDetailViewModel: DoctorDetailViewCallBack, HospitalDoctorViewCallBack,INotifyPropertyChanged
    {


        protected string name1 = "";
        private Doctor doc;
        public Doctor doctor
        {
            get { return doc; }
            set
            {
                doc = value;
                RaisePropertyChanged("doctor");

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
        public ObservableCollection<HospitalInDoctorDetails> hospitals;
        
        public DoctorDetailViewModel(string n)
        {
            this.name1 = n;
            //this.Doctors = new ObservableCollection<Doctor>();

        }
        


        UseCaseBase getDoc;
        UseCaseBase getHosp;
        UseCaseBase updateDoc;
        public async Task GetDoctor()
        {
            hospitals = new ObservableCollection<HospitalInDoctorDetails>();
            getDoc = new GetDoctorDetailUseCase(name1);
            getHosp = new GetHospitalByDoctorUseCase(name1);
            
            getDoc.SetCallBack<DoctorDetailViewCallBack>(this);
            getHosp.SetCallBack<HospitalDoctorViewCallBack>(this);
            try
            {

                await getDoc.Execute();
                await getHosp.Execute();
                

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }




        }

        public async Task UpdateDoctor(string name, double rating)
        {
            updateDoc = new UpdateDoctorRatingUseCase(name, rating);
            updateDoc.SetCallBack(this);
            try
            {
                await updateDoc.Execute();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("UPDATE VIEWMODEL EXCEPTION=" + e.Message);
            }
        }
        
        public bool DataReadSuccess(Doctor d)
        {
            this.doctor = d;
            

            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor FAILED!!!");
            return false;
        }

        bool HospitalDoctorViewCallBack.DataReadSuccess(List<HospitalInDoctorDetails> h)
        {
            
            foreach( var x in h)
            {
                hospitals.Add(x);
            }
            System.Diagnostics.Debug.WriteLine("Doc details viewmodel count=" + hospitals.Count());
            System.Diagnostics.Debug.WriteLine("Doctor Detail View Model Success!!");
            return true;
        }

        bool HospitalDoctorViewCallBack.DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor Detail View Model fail!!");
            return false;
        }

        public bool DoctorDetailViewSuccess(Doctor d)
        {
            this.doctor = d;
            System.Diagnostics.Debug.WriteLine("number value=" + d.Number_of_Rating);
            return true;
        }

        public bool DoctorDetailViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Doctor Update FAILED!!!");
            return false;
        }
    }
}



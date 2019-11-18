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
    public class SelectedHospViewModel : DoctorDetailsAbstract, IHospitalDetailViewCallBack,IDoctorHospitalDetailViewCallback,IHospitalRatingUpdateViewCallback,
        INotifyPropertyChanged,IRosterViewCallback
    {
        
        Hospital hosp;
        public Hospital hospital
        {
            get
            {
                return hosp;
            }
            set
            {
                hosp = value;
                RaisePropertyChanged("hospital");
            }
        }

        
       

        public SelectedHospViewModel()
        {
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            timeslots = new ObservableCollection<Roster>();
        }

        public async Task GetHospital(int id)
        {
            Doctors = new ObservableCollection<DoctorInHospitalDetails>();
            UseCaseBase getHospital = new GetHospitalUseCase(id);
            UseCaseBase getDoctorByHospital = new GetDoctorByHospitalUseCase(id);
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


        public async Task UpdateHospitalRating(int id, double rating)
        {
            UseCaseBase updateHospital = new UpdateHospitalRatingUseCase(id, rating);
            updateHospital.SetCallBack(this);
            await updateHospital.Execute();
        }

        
        public bool DataReadSuccess(Hospital h)
        {
            this.hospital = h;
            return true;
        }

        public bool DataReadSuccess(List<DoctorInHospitalDetails> d)
        {
            //Doctors.Clear();
            //foreach (var x in d)
            //    Doctors.Add(x);
            Doctors = new ObservableCollection<DoctorInHospitalDetails>(d);
            return true;
        }

        public void onHospitalRatingUpdateSuccess()
        {
            if (HospitalRatingUpdateSuccess != null)
                HospitalRatingUpdateSuccess(this, EventArgs.Empty);
        }


        public bool HospitalUpdateSuccess(Hospital h)
        {
            this.hospital = h;
            onHospitalRatingUpdateSuccess();
            return true;
        }

        public bool HospitalUpdateFail()
        {
            return false;
        }

        

        
    }
}

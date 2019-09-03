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
    class DoctorDetailViewModel: DoctorDetailViewCallBack
    {


        protected string name1 = "";
        public Doctor doctor;
        
        public DoctorDetailViewModel(string n)
        {
            this.name1 = n;
            //this.Doctors = new ObservableCollection<Doctor>();

        }


        UseCaseBase getDoc;// = new GetDoctorDetailUseCase();
        public async Task GetDoctor()
        {

            getDoc = new GetDoctorDetailUseCase(name1);
            getDoc.SetCallBack<DoctorDetailViewCallBack>(this);

            try
            {

                await getDoc.Execute();
                

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
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
    }
}



using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Presentation.Callbacks;
using DocApp.Domain.UseCase;
using DocApp.Domain.Usecase;

namespace DocApp.Presentation.ViewModels
{
    public class DoctorListViewModel : DoctorViewCallback
    {
        public ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
        public ObservableCollection<string> doctormain = new ObservableCollection<string>();
        public UseCaseBase getDoctor = new GetDoctorListUseCase();
        public async Task GetDoctors()
        {
            doctormain = new ObservableCollection<string>();
           
            getDoctor.SetCallBack<DoctorViewCallback>(this);

            try
            {

                //await getDoctor.Execute();
                System.Diagnostics.Debug.WriteLine("view model count=" + doctors.Count());
                

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }




        }
       

        public bool DataReadSuccess(List<Doctor> d)
        {

            this.doctors = new ObservableCollection<Doctor>(d);
            foreach (var x in this.doctors)
                this.doctormain.Add(x.Name);

            System.Diagnostics.Debug.WriteLine("Doclist SUCCESS!!!");
            System.Diagnostics.Debug.WriteLine("Doclist success" + doctormain.Count());
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doclist FAILED!!!");
            return false;
        }

        

        public bool DataReadSuccess<P>(P p) where P : class
        {
            throw new NotImplementedException();
        }
    }
}

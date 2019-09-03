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
    public class HospitalListViewModel: HospitalViewCallback
    {
        public ObservableCollection<Hospital> hospitals=new ObservableCollection<Hospital>();
        public ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
        public ObservableCollection<string> hospitalmain = new ObservableCollection<string>();
        public UseCaseBase getHospital= new GetHospitalListUseCase();
        
        public async Task GetHospitals()
        {
            hospitalmain = new ObservableCollection<string>();
            //getHospital = new GetHospitalListUseCase();
            //hospitals = new ObservableCollection<Hospital>();
            getHospital.SetCallBack<HospitalViewCallback>(this);

            try
            {
                
                //await getHospital.Execute();
                System.Diagnostics.Debug.WriteLine("view model count=" + hospitals.Count());
                //SetHospital();
                
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION="+e.Message);
            }
            
            


        }
        


        public bool DataReadSuccess(ref List<Hospital> h)
        {

            this.hospitals = new ObservableCollection<Hospital>(h);
            foreach (var x in this.hospitals)
                this.hospitalmain.Add(x.Name);

            System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
            System.Diagnostics.Debug.WriteLine("success"+hospitalmain.Count());
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("FAILED!!!");
            return false;
        }

        public ObservableCollection<Hospital> GetData()
        {
            return this.hospitals;
        }

    }
}

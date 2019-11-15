using DocApp.Data;
using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.UseCase
{
    public class SearchUseCase : UseCaseBase,IDoctorCallback,IHospitalListCallback
    {

        string name, location;
        //List<Doctor> doctors;
        //List<Hospital> hospitals;
        ISearchViewCallback viewCallback;
        SearchData data;
        public SearchUseCase(string s, string l)
        {
            data = new SearchData();
            this.name = s;
            this.location = l;
            data.doctors = new ObservableCollection<Doctor>();
            data.hospitals = new ObservableCollection<Hospital>();

            //doctors = new List<Doctor>();
            //hospitals = new List<Hospital>();
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ISearchViewCallback)p;
            
        }

        internal override async Task Action()
        {
            IDoctorList DoctorList = new DoctorListDAO();

            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await DoctorList.GetDoctorByNameAsync(name, location, this);

            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("get doctor by name use case DB EXCEPTION" + e.Message);
            }


            if (data.doctors != null && data.doctors.Count>0)
            {
                System.Diagnostics.Debug.WriteLine("get doctor by name use case success");
                
                viewCallback.SearchReadSuccess(data);
            }
            else
            {
                viewCallback.SearchReadFail();
                System.Diagnostics.Debug.WriteLine("get doctor by name use case fail");
            }



            IHospitalList HospitalList = new HospitalListDAO();

            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await HospitalList.GetHospitalByNameAsync(name, location, this);
                //System.Diagnostics.Debug.WriteLine("hosp val="+hospital.Number_Of_Rating);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("Get Hospital by name use case DB EXCEPTION" + e.Message);
            }


            if (data.hospitals != null && data.hospitals.Count>0)
            {
                viewCallback.SearchReadSuccess(data);
                //System.Diagnostics.Debug.WriteLine(hospital.Location);
            }
            else viewCallback.SearchReadFail();
            // + hospitals.Count());

        }

        bool IDoctorCallback.ReadFail()
        {
            data.docflag = false;
            return false;
        }

        bool IHospitalListCallback.ReadFail()
        {
            data.hospflag = false;
            return false;
        }

        bool IDoctorCallback.ReadSuccess(List<Doctor> docs)
        {
            //data.doctors = new ObservableCollection<Doctor>(docs);
            data.docflag = true;
            data.doctors.Clear();
            foreach (var x in docs)
                data.doctors.Add(x);
            return true;
        }

        bool IHospitalListCallback.ReadSuccess(List<Hospital> h)
        {
            data.doctors.Clear();
            data.hospflag = true;
            foreach (var x in h)
                data.hospitals.Add(x);

            
            return true;
        }
    }
}

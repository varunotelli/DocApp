using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocApp.Domain.Usecase;
using DocApp.Domain.UseCase;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using Windows.Devices.Geolocation;

/*
 * @todo Complete search by name for hospital
 */

namespace DocApp.Presentation.ViewModels
{
    


    class HospitalDoctorViewModel: IHospitalLocationPresenterCallBack, IDoctorViewCallBack, IHospitalViewCallback
        , IDoctorLocationPresenterCallBack, IHospitalByDeptViewCallback, IDoctorDeptLocationViewCallback,
        IDoc_SearchInsertViewCallback,ISearchViewCallback
    {
        //public double latitude;
        //public double longitude;
        public delegate void SearchCompletedEventHandler(object source, EventArgs args);
        public event SearchCompletedEventHandler SearchCompleted;


        public ObservableCollection<Doctor> doctors;// = new ObservableCollection<Doctor>();
        public ObservableCollection<Hospital> hospitals;// = new ObservableCollection<Hospital>();

        public SearchData searchData;
        public UseCaseBase getHosps;
        public UseCaseBase getDocs;
        public bool docflag = true;
        public bool hospflag=true;
        int temp=-1;
        
        public HospitalDoctorViewModel()
        {
            doctors = new ObservableCollection<Doctor>();
            hospitals = new ObservableCollection<Hospital>();
     
        }

        public void onSearchCompleted()
        {
            if (SearchCompleted != null)
                SearchCompleted(this, EventArgs.Empty);
        }

        public async Task GetSearchResults(string name, string location)
        {
            UseCaseBase search = new SearchUseCase(name, location);
            search.SetCallBack(this);
            await search.Execute();
        }


        public async Task AddDocSearchResult(Doc_Search d)
        {
            UseCaseBase addDoc = new AddSearchResultUseCase(d);
            addDoc.SetCallBack(this);
            await addDoc.Execute();
        }

        public async Task GetHospitalByLocation(string loc)
        {
            getHosps = new GetHospitalByLocationUseCase(loc.ToUpper());
            getHosps.SetCallBack<IHospitalLocationPresenterCallBack>(this);
            await getHosps.Execute();
        }
        public async Task GetDoctorByLocation(string loc)
        {
            getDocs = new GetDoctorByLocationUseCase(loc.ToUpper());
            getDocs.SetCallBack<IDoctorLocationPresenterCallBack>(this);
            await getDocs.Execute();
        }

        public async Task GetDoctorsByDept(string location, int dept)
        {
            getDocs = new GetDoctorByDeptLocationUseCase(location, dept);
            getDocs.SetCallBack<IDoctorDeptLocationViewCallback>(this);
            await getDocs.Execute();
        }
        public async Task GetHospitalByDept(string location, int dept,int rating=-1)
        {
            getHosps = new GetHospitalByDeptUseCase(location.ToUpper(), dept,rating);
            getHosps.SetCallBack<IHospitalByDeptViewCallback>(this);
            await getHosps.Execute();
        }
        public async Task GetDoctorsByName(string name, string location)
        {
            getDocs = new GetDoctorByNameUseCase(name.ToUpper(), location);
            getDocs.SetCallBack<IDoctorViewCallBack>(this);
            await getDocs.Execute();
        }

        public async Task GetHospitalByName(string name, string location)
        {
            getHosps = new GetHospitalByNameUseCase(name.ToUpper(), location);
            getHosps.SetCallBack<IHospitalViewCallback>(this);
            await getHosps.Execute();
        }


        public bool DataReadSuccess(List<Hospital> h)
        {
            this.hospitals.Clear();
            if(!hospitals.SequenceEqual(h))
            foreach (var x in h)
                this.hospitals.Add(x);
            System.Diagnostics.Debug.WriteLine("SUCCESS!!!");
            
            return true;
        }
       

        public bool HospitalLocationReadSuccess(List<Hospital> h)
        {
            System.Diagnostics.Debug.WriteLine("Hosp Location viewmodel success");
            this.hospitals.Clear();
            foreach (var x in h)
                this.hospitals.Add(x);
            return true;
        }

        public bool HospitalLocationReadFail()
        {
            System.Diagnostics.Debug.WriteLine(" Hosp Location viewmodel fail");
            return false;
        }

        public bool DoctorLocationReadSuccess(List<Doctor> d)
        {
            System.Diagnostics.Debug.WriteLine(" Doc Location viewmodel success");
            this.doctors.Clear();
            foreach (var x in d)
                this.doctors.Add(x);
            return true;
        }

        public bool DoctorLocationReadFail()
        {
            System.Diagnostics.Debug.WriteLine(" Doc Location viewmodel fail");
            return false;
        }

        public bool ReadViewSuccess(List<Hospital> h)
        {
            this.hospitals.Clear();
            foreach (var x in h)
                this.hospitals.Add(x);
            return true;
        }

        public bool ReadViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Hosp Dept fail");
            return false;
        }

        public bool DeptLocationViewSuccess(List<Doctor> d)
        {
            this.doctors.Clear();
            foreach (var x in d)
                this.doctors.Add(x);
            return true;
        }

        public bool DeptLocationViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Doc Dept fail");
            return false;
        }

        public bool DataReadSuccess(List<Doctor> d)
        {
            this.doctors.Clear();
            foreach (var x in d)
                this.doctors.Add(x);
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doc Search fail");
            return false;
        }

        public bool Doc_SearchInsertViewSuccess(int x)
        {
            this.temp = x;
            return true;
        }

        public bool Doc_SearchInsertViewFail()
        {
            System.Diagnostics.Debug.WriteLine("doc search viewmodel fail");
            return false;
        }

        public bool SearchReadSuccess(SearchData data)
        {
            //searchData = data;
            docflag = data.docflag;
            hospflag = data.hospflag;
            //doctors.Clear();
            //hospitals.Clear();
            if(docflag)
                foreach (var x in data.doctors)
                    doctors.Add(x);
            if(hospflag)
                foreach (var x in data.hospitals)
                    hospitals.Add(x);
            onSearchCompleted();
            return true;
        }

        public bool SearchReadFail()
        {
            docflag = false;
            hospflag = false;
            onSearchCompleted();
            return false;
        }
    }
}

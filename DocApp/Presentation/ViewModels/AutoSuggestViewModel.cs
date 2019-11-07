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
using Windows.Devices.Geolocation;

namespace DocApp.Presentation.ViewModels
{
    public class LocationEventArgs : EventArgs
    {
        public string address { get; set; }
    }

    public class LoginSuccessEventArgs:EventArgs
    {
        public int ct { get; set; }
    }

    public class AutoSuggestViewModel: IGetLocationPresenterCallback, IGetAddressPresenterCallback, IDepartmentViewCallback,
        INotifyPropertyChanged, IKeywordViewCallback,ILoginViewCallback,IRecentDoctorViewCallback
    {
        public double latitude;
        public double longitude;
        public ObservableCollection<string> localities;
        public ObservableCollection<string> depts;
        public ObservableCollection<string> keywords;
        public ObservableCollection<Doctor> docs;
        public UseCaseBase getLocalityList;
        public UseCaseBase getDepts;
        public UseCaseBase getKeyWords;
        private string location = "Chennai";
        public UseCaseBase getAddress;
        public delegate void LocationChangedEventHandler(object source, LocationEventArgs e);
        public event LocationChangedEventHandler LocationChanged;
        public delegate void LoginSuccessEventHandler(object source, LoginSuccessEventArgs args);
        public event LoginSuccessEventHandler LoginEventSuccess;
        public int count;
        public string loc
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                RaisePropertyChanged("loc");
            }
        }
        public string locbox = "";
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));


            }
        }

        public AutoSuggestViewModel()
        {
            depts = new ObservableCollection<string>();
            keywords = new ObservableCollection<string>();
            docs = new ObservableCollection<Doctor>();
        }

        public void onLocationChanged()
        {
            if (LocationChanged != null)
                LocationChanged(this, new LocationEventArgs { address = loc });
        }

        public void onLoginSuccess(int x)
        {
            if (LoginEventSuccess != null)
                LoginEventSuccess(this, new LoginSuccessEventArgs() { ct = x });
        }

        public async Task GetLocalities(string s)
        {
            localities = new ObservableCollection<string>();
            getLocalityList = new GetLocationsUseCase(s);
            getLocalityList.SetCallBack<IGetLocationPresenterCallback>(this);
            await getLocalityList.Execute();
        }

        

       
        public async Task GetCurrentAddress()
        {
            //var temp = await GetPosition();
            //latitude = temp.Coordinate.Latitude;
            //longitude = temp.Coordinate.Longitude;

            getAddress = new GetAddressUseCase();
            

            //getDoctor.SetCallBack<DoctorViewCallback>(this);

            getAddress.SetCallBack<IGetAddressPresenterCallback>(this);

            try
            {


                await getAddress.Execute();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION=" + e.Message);
            }

        }

        public async Task GetDepartments()
        {
            getDepts = new GetDeptsUseCase();
            getDepts.SetCallBack<IDepartmentViewCallback>(this);
            await getDepts.Execute();
        }

        public async Task GetKeyWords()
        {
            getKeyWords = new GetKeyWordUseCase();
            getKeyWords.SetCallBack<IKeywordViewCallback>(this);
            await getKeyWords.Execute();
        }

        public async Task UserLogin()
        {
            Logins l = new Logins() { user_id = 1, login_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm") };
            UseCaseBase loginUseCase = new LoginUseCase(l);
            loginUseCase.SetCallBack(this);
            await loginUseCase.Execute();
        }

        public async Task GetRecentSearchDoctors(int id)
        {

            try
            {
                UseCaseBase getRecentDoctors = new GetRecentDoctorsUseCase(id);
                getRecentDoctors.SetCallBack(this);
                await getRecentDoctors.Execute();
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Recent doctor view fail " + e.Message);

            }
        }

        public bool DataFromPractoSuccess(RootLocationObject r)
        {
            System.Diagnostics.Debug.WriteLine("Viewmodel location success");
            localities.Clear();
            foreach (var x in r.results.@default.matches)
                localities.Add(x.suggestion);
            return true;
        }

        public bool DataFromPractoFail()
        {
            System.Diagnostics.Debug.WriteLine("Viewmodel location fail");
            return false;
        }



        public bool DataReadFromAPISuccess(RootObject r)
        {

            loc = r.address.neighbourhood;
            System.Diagnostics.Debug.WriteLine("location=" + location);
            onLocationChanged();
            return true;
        }

        public bool DataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("API Viewmodel fail");
            return false;
        }

        public bool DepartmentDataReadSuccess(List<Department> d)
        {
            
            foreach (var x in d)
                depts.Add(x.name);
            return true;
        }

        public bool DepartmentDataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Main page depts fail");
            return false;
        }

        public bool KeywordReadViewSuccess(List<KeyWord> k)
        {
            keywords.Clear();
            foreach (var x in k)
                keywords.Add(x.common_name);
            return true;
        }

        public bool KeywordReadViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Keyword view fail");
            return false;
        }

        public bool LoginViewSuccess(int val)
        {
            this.count = val;
            onLoginSuccess(val);
            return false;
        }

        public bool LoginViewFail()
        {
            return false;
        }

        public bool SearchDocViewSuccess(List<Doctor> doctors)
        {
            docs.Clear();
            foreach (var x in doctors.Take(4))
                docs.Add(x);
            return true;
        }

        public bool SearchDocViewFail()
        {
            return false;
        }
    }
}

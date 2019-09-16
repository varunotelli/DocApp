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
    public class DoctorSearchViewModel : IDepartmentViewCallback,IDoctorDeptLocationViewCallback,IDoctorLocationPresenterCallBack,
        INotifyPropertyChanged
    {
        public UseCaseBase getDepts;
        public UseCaseBase getDocs;
        
        public ObservableCollection<string> deptnames;
        ObservableCollection<Doctor> d;
        public ObservableCollection<Doctor> docs;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));


            }
        }

        public DoctorSearchViewModel()
        {
            deptnames = new ObservableCollection<string>();
            docs= new ObservableCollection<Doctor>();
        }

        public async Task GetDepartments()
        {
            getDepts = new GetDeptsUseCase();
            getDepts.SetCallBack<IDepartmentViewCallback>(this);
            await getDepts.Execute();
        }

        public async Task GetDoctors(string location)
        {
            getDocs = new GetDoctorByLocationUseCase(location);
            getDocs.SetCallBack<IDoctorLocationPresenterCallBack>(this);
            await getDocs.Execute();
        }

        public async Task GetDoctorsByDept(string location, string dept)
        {
            getDocs = new GetDoctorByDeptLocationUseCase(location, dept);
            getDocs.SetCallBack<IDoctorDeptLocationViewCallback>(this);
            await getDocs.Execute();
        }

       

        public bool DepartmentDataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Department view fail");
            return false;
        }

        public bool DepartmentDataReadSuccess(List<Department> d)
        {
            
            foreach (var x in d)
                deptnames.Add(x.name);
            return true;
        }

        public bool DeptLocationViewSuccess(List<Doctor> doctors)
        {
            docs.Clear();
            foreach (var x in doctors)
                docs.Add(x);
            return true;
        }

        public bool DeptLocationViewFail()
        {
            System.Diagnostics.Debug.WriteLine("Dept location view fail");
            return false;
        }

        public bool DoctorLocationReadSuccess(List<Doctor> doctors)
        {
            //docs = new ObservableCollection<Doctor>();

            docs.Clear();
            foreach (var x in doctors)
                docs.Add(x);
            return true;
        }

        public bool DoctorLocationReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Doc location view fail");
            return false;
        }
    }
}

using DocApp.Domain.UseCase;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * @todo complete view of hospital
 */
namespace DocApp.Presentation.ViewModels
{
    public class HospitalSearchViewModel: IHospitalByDeptViewCallback, IHospitalLocationPresenterCallBack,
        IDepartmentViewCallback
    {
        public ObservableCollection<string> deptnames;
        public ObservableCollection<Hospital> hospitals;
        UseCaseBase getHosp;
        public UseCaseBase getDepts;
        public HospitalSearchViewModel()
        {
            deptnames = new ObservableCollection<string>();
            hospitals = new ObservableCollection<Hospital>();
        }
        public async Task GetDepartments()
        {
            getDepts = new GetDeptsUseCase();
            getDepts.SetCallBack<IDepartmentViewCallback>(this);
            await getDepts.Execute();
        }
        public async Task GetHospitals(string location)
        {
            getHosp = new GetHospitalByLocationUseCase(location);
            getHosp.SetCallBack<IHospitalLocationPresenterCallBack>(this);
            await getHosp.Execute();
        }

        public async Task GetHospitalByDept(string location, int dept)
        {
            getHosp = new GetHospitalByDeptUseCase(location.ToUpper(), dept+1);
            getHosp.SetCallBack<IHospitalByDeptViewCallback>(this);
            await getHosp.Execute();
        }

        public bool HospitalLocationReadFail()
        {
            return false;
        }

        public bool HospitalLocationReadSuccess(List<Hospital> h)
        {
            hospitals.Clear();
            foreach (var x in h)
                hospitals.Add(x);
            return true;
        }

        public bool ReadViewFail()
        {
            return false;
        }

        public bool ReadViewSuccess(List<Hospital> h)
        {
            hospitals.Clear();
            foreach (var x in h)
                hospitals.Add(x);
            return true;
        }

        public bool DepartmentDataReadSuccess(List<Department> d)
        {
            foreach (var x in d)
                deptnames.Add(x.name);
            return true;
        }

        public bool DepartmentDataReadFail()
        {
            return false;
        }
    }
}

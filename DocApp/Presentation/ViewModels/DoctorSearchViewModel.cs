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
    public class DoctorSearchViewModel : IDepartmentViewCallback
    {
        public UseCaseBase getDepts;
        public ObservableCollection<string> deptnames;

        public DoctorSearchViewModel()
        {
            deptnames = new ObservableCollection<string>();

        }

        public async Task GetDepartments()
        {
            getDepts = new GetDeptsUseCase();
            getDepts.SetCallBack<IDepartmentViewCallback>(this);
            await getDepts.Execute();
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
    }
}

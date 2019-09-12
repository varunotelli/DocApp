using DocApp.Data;
using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.UseCase
{
    public class GetDeptsUseCase : UseCaseBase, IDepartmentListCallBack
    {
        IDepartmentViewCallback viewCallback;
        List<Department> departments;

        public GetDeptsUseCase()
        {
            this.departments = new List<Department>();
        }

        public bool DepartmentDataReadFail()
        {
            System.Diagnostics.Debug.WriteLine("dept usercase Fail");
        
            return false;
        }

        public bool DepartmentDataReadSuccess(List<Department> l)
        {
            this.departments = l;
            return true;
            
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IDepartmentViewCallback)p;
        }

        internal override async Task Action()
        {
            IDepartmentList dept = new DepartmentDAO();
            departments = new List<Department>();
            try
            {
                await dept.GetDepartmentList(this);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("dept usercase exception "+e.Message);
            }
            if (departments != null && departments.Count > 0)
                viewCallback.DepartmentDataReadSuccess(departments);
            else viewCallback.DepartmentDataReadFail();


        }
    }
}

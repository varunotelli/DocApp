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
    public class DashboardViewModel: IRecentDoctorViewCallback
    {
        public ObservableCollection<Doctor> recent_docs;
        public DashboardViewModel()
        {
            this.recent_docs = new ObservableCollection<Doctor>();
        }
        public async Task GetRecentSearchDoctors(int id)
        {

            try
            {
                UseCaseBase getRecentDoctors = new GetRecentDoctorsUseCase(id);
                getRecentDoctors.SetCallBack(this);
                await getRecentDoctors.Execute();
            }

            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Recent doctor view fail " + e.Message);

            }
        }

        public bool SearchDocViewFail()
        {
            return false;
        }

        public bool SearchDocViewSuccess(List<Doctor> doctors)
        {
            recent_docs.Clear();
            foreach (var x in doctors)
                recent_docs.Add(x);
            return true;
        }
    }
}

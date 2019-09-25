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
    public class AppointmentDisplayViewModel : IAppDisplayViewCallback
    {
        UseCaseBase getApps;
        public ObservableCollection<AppointmentDetails> apps;

        public async Task GetAppointments(int id)
        {
            getApps = new GetAppointmentsUseCase(id);
            apps = new ObservableCollection<AppointmentDetails>();
            getApps.SetCallBack(this);
            await getApps.Execute();
        }


        public bool GetAppsReadViewFail()
        {
            return false;
        }

        public bool GetAppsReadViewSuccess(List<AppointmentDetails> appointments)
        {
            apps.Clear();
            foreach (var x in appointments)
                apps.Add(x);
            return true;
        }
    }
}

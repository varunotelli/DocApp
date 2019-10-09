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
    public class GetRosterUseCase : UseCaseBase, IRosterCallback
    {
        IRosterViewCallback viewCallback;
        List<Roster> rosters;
        int doc_id;
        int hosp_id;
        string app_date;
        public GetRosterUseCase(int d, int h,string s)
        {
            this.doc_id = d;
            this.hosp_id = h;
            this.app_date = s;

        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IRosterViewCallback)p;
        }

        internal override async Task Action()
        {
            IRoster roster = new RosterDAO();
            try
            {
                await roster.GetTimeSlots(doc_id, hosp_id, app_date,this);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("roster usecase fail");
            }
            if (rosters != null)
                viewCallback.RosterViewReadSuccess(rosters);
            else
                viewCallback.RosterViewReadFail();

        }

        public bool RosterReadFail()
        {
            return false;
        }

        public bool RosterReadSuccess(List<Roster> r)
        {
            this.rosters = r;
            return true;
        }
    }
}

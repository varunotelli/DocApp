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
    public class GetLocationsUseCase : UseCaseBase, ILocationCallBack
    {
        string val = "";
        RootLocationObject locs;
        IGetLocationPresenterCallback callback;

        public GetLocationsUseCase(string s)
        {

            this.val = s;
        }

        public override void SetCallBack<P>(P p)
        {
            this.callback = (IGetLocationPresenterCallback)p;
        }

        internal override async Task Action()
        {
            ILocationList location = new LocationProxy();

            try
            {
                System.Diagnostics.Debug.WriteLine("In locations use case");
                System.Diagnostics.Debug.WriteLine("In location val="+val);
                await location.GetLocationsAsync(val, this);


            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine("LOCATION EXCEPTION" + e.Message);
                callback.DataFromPractoFail();
            }

            if (locs != null)
                callback.DataFromPractoSuccess(locs);
            else
                callback.DataFromPractoFail();

        }
        public bool PractoReadFail()
        {
            System.Diagnostics.Debug.WriteLine("LOCATION FAIL!");
            return false;
        }

        public bool PractoReadSuccess(RootLocationObject root)
        {
            System.Diagnostics.Debug.WriteLine("LOCATION success!");
            this.locs = root;
            return true;
        }
    }
}

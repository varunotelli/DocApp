using DocApp.Data;
using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.UseCase
{
    public class AddTestimonialUseCase : UseCaseBase,ITestCallback
    {
        ITestViewCallback viewCallback;
        int patient, doc;
        string message, time;

        public AddTestimonialUseCase(int p, int d, string m, string t)
        {
            this.patient = p;
            this.doc = d;
            this.message = m;
            this.time = t;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ITestViewCallback)p;
        }

        public bool TestReadFail()
        {
            return false;
        }

        public bool TestReadSuccess()
        {
            return true;
        }

        internal override async Task Action()
        {
            ITest test = new TestDAO();
            try
            {
                await test.AddTestimonial(patient, doc, message, time, this);
                viewCallback.TestReadViewSucces();
            }
            catch(Exception e)
            {
                viewCallback.TestReadViewFail();
            }

        }
    }
}

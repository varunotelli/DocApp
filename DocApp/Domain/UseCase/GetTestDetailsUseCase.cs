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
    public class GetTestDetailsUseCase : UseCaseBase,ITestDetailsCallback
    {
        int doc_id;
        ITestDetailsViewCallback viewCallback;
        List<TestDetails> dets;
        public GetTestDetailsUseCase(int id)
        {
            dets = new List<TestDetails>();
            this.doc_id = id;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ITestDetailsViewCallback)p;
        }

        internal override async Task Action()
        {
            ITestDetails testDetails = new TestDetailsDAO();
            try
            {
                await testDetails.GetTestDetails(doc_id,this);               
            }

            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Testimonial use case exception=" + e.Message);
            }
            if (dets != null)
                viewCallback.TestDetailsReadViewSuccess(dets);
            else viewCallback.TestDetailsReadViewFail();

            
        }

        public bool TestDetailsReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Testimonial use case Fail");
            return false;
        }

        public bool TestDetailsReadSuccess(List<TestDetails> details)
        {
            this.dets = details;
            return true;
        }
    }
}

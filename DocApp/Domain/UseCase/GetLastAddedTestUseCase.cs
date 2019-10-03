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
    public class GetLastAddedTestUseCase : UseCaseBase,ILastTestDetailCallback
    {

        ILastTestViewCallback viewCallback;
        TestDetails test;
        int id;

        public GetLastAddedTestUseCase(int x)
        {
            this.id = x;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (ILastTestViewCallback)p;
        }

        internal override async Task Action()
        {
            ITestDetails testDetails = new TestDetailsDAO();
            try
            {
                await testDetails.GetLastTestDetail(id,this);
            }
            catch(Exception e)
            {

            }
            if (test != null)
                viewCallback.LastTestViewSuccess(test);
            else viewCallback.LastTestViewFail();
        }
        public bool LastTestReadFail()
        {
            return false;
        }

        public bool LastTestReadSuccess(TestDetails details)
        {
            this.test = details;
            return true;
        }
    }
}

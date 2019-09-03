using DocApp.Domain.Callbacks;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DocApp.Domain.UseCase
{
    public abstract class UseCaseBase
    {
        internal abstract Task Action();

        public bool GetIfAvailableInCache()
        {
            return false;
        }



        public Windows.Foundation.IAsyncAction Execute()
        {

            //if (GetIfAvailableInCache()) return;

            return Windows.ApplicationModel.Core.CoreApplication.
                GetCurrentView().CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
             {
                 try
                 {
                     await Action();
                     System.Diagnostics.Debug.WriteLine("In Use Case base");
                    //return true;

                }
                 catch (Exception e)
                 {
                     System.Diagnostics.Debug.WriteLine("Use case base exception=" + e.Message);
                    //return false;
                }
             }
                );
        }

        public abstract void SetCallBack<P>(P p) where P : class;

    }
}

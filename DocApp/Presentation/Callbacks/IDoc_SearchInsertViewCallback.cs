using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Callbacks
{
    public interface IDoc_SearchInsertViewCallback
    {
        bool Doc_SearchInsertViewSuccess(int x);
        bool Doc_SearchInsertViewFail();
    }
}

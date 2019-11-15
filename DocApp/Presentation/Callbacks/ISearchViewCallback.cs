using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Callbacks
{
    public interface ISearchViewCallback
    {
        bool SearchReadSuccess(SearchData data);
        bool SearchReadFail();
    }
}

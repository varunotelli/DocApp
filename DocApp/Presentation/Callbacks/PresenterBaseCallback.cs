using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Callbacks
{
    public interface PresenterBaseCallback
    {
        bool DataReadSuccess<P>(P p) where P:class;
        bool DataReadFail();
    }
}

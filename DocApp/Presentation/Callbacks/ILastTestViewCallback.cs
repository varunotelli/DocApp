using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Presentation.Callbacks
{
    interface ILastTestViewCallback
    {
        bool LastTestViewSuccess(TestDetails detail);
        bool LastTestViewFail();
    }
}

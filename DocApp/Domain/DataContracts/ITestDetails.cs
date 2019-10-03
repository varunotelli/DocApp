using DocApp.Domain.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface ITestDetails
    {
        Task GetTestDetails(int doc_id, ITestDetailsCallback callback);
        Task GetLastTestDetail(int doc_id, ILastTestDetailCallback callback);
    }
}

using DocApp.Domain.Callbacks;
using DocApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.DataContracts
{
    public interface IDoc_Search
    {
        Task AddDocSearch(Doc_Search d, IDoc_SearchCallback callback);
    }
}

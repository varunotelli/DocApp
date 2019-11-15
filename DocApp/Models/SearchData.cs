using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class SearchData
    {
        public ObservableCollection<Doctor> doctors { get; set; }
        public ObservableCollection<Hospital> hospitals { get; set; }
        public bool docflag { get; set; }
        public bool hospflag { get; set; }

    }
}

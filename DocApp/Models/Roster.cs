using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class Roster
    {
        public int doc_id { get; set; }
        public int hosp_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public int fee { get; set; }
        public int max_patients { get; set; }
        public int val { get; set; }
    }
}

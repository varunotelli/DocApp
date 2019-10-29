using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
   public class AppointmentDetails
   {
        public int id { get; set; }
        public string doc_name { get; set; }
        public string img { get; set; }
        public string hosp_name { get; set; }
        public string location { get; set; }
        public string app_date { get; set; }
        public string Timeslot { get; set; }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class DoctorInHospitalDetails
    {
        public int doc_id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Experience { get; set; }
        public string Designation { get; set; }
        public float Rating { get; set; }
        public int Number_of_Rating { get; set; }
        public int fees { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public int max_patients { get; set; }
    }
}

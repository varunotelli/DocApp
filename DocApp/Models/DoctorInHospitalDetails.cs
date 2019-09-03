using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class DoctorInHospitalDetails
    {
        public string Name { get; set; }
        protected string Image { get; set; }
        public string Experience { get; set; }
        public string Designation { get; set; }
        public float Rating = 0;
        public int Number_of_Rating = 0;
        public int fees { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public int max_patients { get; set; }
    }
}

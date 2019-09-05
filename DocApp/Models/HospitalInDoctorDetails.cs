using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class HospitalInDoctorDetails
    {
        public string Name { get; set; }
        protected string Image { get; set; }
        public string Location { get; set; }
        public float Rating { get; set; }
        public int Number_of_Rating { get; set; }
        public int Fees { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public int Max_Patients { get; set; }
    }
}

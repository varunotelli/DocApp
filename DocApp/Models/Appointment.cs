using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class Appointment
    {
        [PrimaryKey]
        public int ID { get; set; }
        public int DOC_ID { get; set; }
        public int HOS_ID { get; set; }
        public int PATIENT_ID { get; set; }
        public string APP_DATE { get; set; }
        public string start_time { get; set; }
        
    }
}

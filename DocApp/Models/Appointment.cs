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
        public DateTime APP_DATE { get; set; }
        public DateTime start_time { get; set; }
        
    }
}

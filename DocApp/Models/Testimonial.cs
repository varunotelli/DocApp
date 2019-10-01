using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class Testimonial
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int Patient_ID { get; set; }
        public int Doc_ID { get; set; }
        public string message { get; set; }
        public string posted_time { get; set; }

    }
}

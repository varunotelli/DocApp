using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace DocApp.Models
{
    public class Hospital: NotifyChangeBase
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public float Rating { get; set; }
        public int Number_Of_Rating { get; set; }
        public string Image { get; set; }
        public string Start_Time { get; set; }
        public string Close_Time { get; set; }
        public string Description { get; set; }
        public bool Clinic { get; set; }    
        public string Speciality { get; set; }
    }
}

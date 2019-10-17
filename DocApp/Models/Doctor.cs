using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace DocApp.Models
{
    public class Doctor
    {
        [PrimaryKey]
        public int ID { get; set; }
        [NotNull]
        public string Name { get; set; }
        public string Image { get; set; }
        public int Experience { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public float Rating { get; set; }
        public int Number_of_Rating { get; set; }
    
    }
}

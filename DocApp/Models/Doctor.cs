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
        protected string Image { get; set; }
        public string Experience { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        protected float Rating = 0;
        protected int Number_of_Rating = 0;
    
    }
}

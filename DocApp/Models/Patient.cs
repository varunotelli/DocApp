using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class Patient
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string name { get; set;}
        public int age { get; set; }
    }
}

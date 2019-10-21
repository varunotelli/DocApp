using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class Doc_Search
    {
        public int user_id { get; set; }
        public int doc_id { get; set; }
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
    }
}

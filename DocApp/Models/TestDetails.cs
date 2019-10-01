using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class TestDetails
    {
        public int test_id { get; set; }
        public int p_id { get; set; }
        public int doc_id { get; set; }
        public string patient_name { get; set; }
        public string message { get; set; }
        public string posted_time { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    public class RootObject
    {
        public int place_id { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public int osm_id { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
        public Address address { get; set; }
        public List<string> boundingbox { get; set; }
    }
}

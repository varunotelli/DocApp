using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    [DataContract]
    public class Match
    {
        [DataMember]
        public string suggestion { get; set; }
        [DataMember]
        public int weight { get; set; }
        [DataMember]
        public string category { get; set; }
        [DataMember]
        public string display_name { get; set; }
        [DataMember]
        public string display_name_original { get; set; }
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string original { get; set; }
        [DataMember]
        public string city_live { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string city_slug { get; set; }
        [DataMember]
        public string country_slug { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string locality { get; set; }
        [DataMember]
        public string locality_slug { get; set; }
    }
}

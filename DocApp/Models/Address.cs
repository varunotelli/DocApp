using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string address29 { get; set; }
        [DataMember]
        public string road { get; set; }
        [DataMember]
        public string neighbourhood { get; set; }
        [DataMember]
        public string suburb { get; set; }
        [DataMember]
        public string village { get; set; }
        [DataMember]
        public string state_district { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string postcode { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string country_code { get; set; }
    }
}

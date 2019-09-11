using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Models
{
    [DataContract]
    public class Default
    {
        [DataMember]
        public List<Match> matches { get; set; }
    }
    [DataContract]
    public class Results
    {
        [DataMember]
        public Default @default { get; set; }
        
    }
    
    public class RootLocationObject
    {
       
        public Results results { get; set; }
    }
}

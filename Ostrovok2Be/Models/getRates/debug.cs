using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ostrovok2Be.Models.getRates
{
   public class debug
    {
        public int? limit { get; set; }
        public int? apiauthkey_id { get; set; }
        public string include { get; set; }
        public int? status { get; set; }
        public string lang { get; set; }
        public string format { get; set; }
        public int? adults { get; set; }
        public string checkout { get; set; }
        public string exclude { get; set; }
        public List<string> children { get; set; }
        public string currency { get; set; }
        public List<string> ids { get; set; }
        public string contract_slug { get; set; }
        public string checkin { get; set; } 
        
    }
}

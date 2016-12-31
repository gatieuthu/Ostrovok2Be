using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ostrovok2Be.Models.getRates
{
   public class Result
    {
        public string NextPage { get; set; }
        public List<Hotels> hotels { get; set; }
       public int? total_pages { get; set; }
       public int? total_hotels { get; set; }
    }
}

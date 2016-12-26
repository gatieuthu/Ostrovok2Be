using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ostrovok2Be.Models.getFromOstrovok
{
    public class AuthoRegion
    {
        public int ? hotels_count { get; set; }
        public string locative_where_ru { get; set; }
        public string  locative_where_en { get; set; }
        public string type { get; set; }
        public Center Center { get; set; }
       
    }
}

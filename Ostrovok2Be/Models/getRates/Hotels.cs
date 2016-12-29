using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ostrovok2Be.Models.getRates
{
  public   class Hotels
    {
      public string available_rates { get; set; }
      public string rate_name_min { get; set; }
      public string sort_score { get; set; }
      public string rate_price_min { get; set; }
      public string id { get; set; }
      public List<Rates> rates { get; set; }
    }
}

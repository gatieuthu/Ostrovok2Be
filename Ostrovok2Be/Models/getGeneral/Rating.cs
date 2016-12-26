using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ostrovok2Be.Models.getFromOstrovok;
using Ostrovok2Be.Models.getGeneral;

namespace Ostrovok2Be.Models
{
  public  class Rating
    {
        public rating_detail detailed { get; set; }
        public bool exists { get; set; }
        public int? count { get; set; }
        public string total_verbose { get; set; }
        public string our_reviews_count { get; set; }
        public ReviewBest review_best { get; set; }
        public int? other_reviews_count { get; set; }
        
    }
}

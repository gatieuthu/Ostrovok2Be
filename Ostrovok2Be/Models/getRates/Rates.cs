using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ostrovok2Be.Models.getRates
{
    public class Rates
    {
       public List<policies> policies { get; set; }
        public List<float> daily_prices { get; set; }
        public List<string> room_amenities { get; set; }
        public List<string>  rate_currency { get; set; }
       public  PompetitorPrices competitor_prices { get; set; }
        public BedPlace bed_places { get; set; }
        public List<string> taxes { get; set; }
        public PaymentOption payment_options { get; set; }
        public string room_size { get; set; }
        public string available_rooms { get; set; }
        public string non_refundable { get; set; }
        public int? room_group_id { get; set; }
        public List<ValueAdd> value_adds { get; set; }
        public string smoking_policies { get; set; }
        public string rate_price { get; set; }
        public List<string> serp_filters { get; set; }
        public string book_hash { get; set; }
        
    }
}

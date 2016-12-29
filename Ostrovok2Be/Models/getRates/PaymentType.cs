using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ostrovok2Be.Models.getRates
{
    public class PaymentType
    {
        public string currency_code { get; set; }
        public string vat_included { get; set; }
        public string by { get; set; }
        public float amount { get; set; }
        public string is_need_cvc { get; set; }
        public string type { get; set; }
        public string is_need_credit_card_data { get; set; }
    } 
}

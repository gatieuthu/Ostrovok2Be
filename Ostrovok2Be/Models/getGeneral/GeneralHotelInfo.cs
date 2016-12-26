using System.Collections.Generic;
using Ostrovok2Be.Models.getFromOstrovok;
using Ostrovok2Be.Models.getGeneral;

namespace Ostrovok2Be.Models
{
    public class GeneralHotelInfo
    {
        public string is_bookable_in_api { get; set; }
        public Rating rating  { get; set; }
        public string thumbnail { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public string latitude { get; set; }
        public string country_code { get; set; }
        public string check_in_time { get; set; }
        public string longitude { get; set; }
        public string check_out_time { get; set; }
        public string hotelpage { get; set; }
        public string address { get; set; }
        public string id { get; set; }
        public string clean_address { get; set; }
        public string policy_description { get; set; }
        public List<string> serp_filters { get; set; }
        public string star_rating { get; set; }
        public string country { get; set; }
        public List<images> images { get; set; }
        public string city { get; set; }
        public string kind { get; set; }
        public string region_category { get; set; }
        public Matching matching { get; set; }
        public List<amenity> amenities { get; set; }
        public List<RoomGroup> room_groups { get; set; }
        public string description_short { get; set; }
        public string name { get; set; }
        public string region_id { get; set; }
        public string phone { get; set; }
    }
}
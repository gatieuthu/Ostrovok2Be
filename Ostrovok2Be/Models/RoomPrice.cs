namespace Ostrovok2Be.Models
{
    public class RoomPrice
    {
        public string Ids { get; set; } 
        public string fromdate { get; set; } 
        public string todate { get; set; }
        public int? room_group_id { get; set; }
        public float? Price { get; set; }
        public string Currency { get; set; }

    }
}
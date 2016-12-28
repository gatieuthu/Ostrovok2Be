using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ostrovok2Be.Models.getGeneral;

namespace Ostrovok2Be.Models.getFromOstrovok
{
   public class RoomGroup
    {
    /* public  amenity amenities { get; set; }*/
     public List<image_roomgroup> image_list_tmpl { get; set; }
     public int? room_group_id { get; set; }
     public string name { get; set; }
     public string size { get; set; }
     public string thumbnail_tmpl { get; set; }
    }
}

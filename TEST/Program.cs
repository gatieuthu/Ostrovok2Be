using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Ostrovok2Be.Function;
using Ostrovok2Be.Models.getRates;
using Ostrovok2Be.RequestType;


namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> tempListIds = new List<string>(new string[]{"dong_khanh_hotel"});

            var allgroupIds = list2objbuilder.ListCreator(tempListIds,10);
            var allGroup = new str2objbuilder(allgroupIds.FirstOrDefault()).listIds2Object();
           var checkInDate = "2017-05-31";
          var  checkOutDate = "2017-06-01";

            /*var tempresult = GetRate.getRateHotelInforByIds(allGroup, checkInDate, checkOutDate, "VND");*/


         /*   var tempRateObj = JsonConvert.DeserializeObject<RatesPackage>(tempresult);*/
                //getRatesObject.Add(tempRateObj);
           


            Console.WriteLine(tempListIds.Count());
            Console.ReadLine();

        }
    }
}

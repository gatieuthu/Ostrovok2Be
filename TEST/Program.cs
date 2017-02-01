using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Ostrovok2Be.Models;
using Ostrovok2Be.Models.getFromOstrovok;
using Ostrovok2Be.Models.getRates;

namespace DivList
{
    class Program
    {
        static void Main(string[] args)
        {
            Rates a = new Rates()
            {
                daily_prices = new List<float>(
                new float[] { 1,1,1,1 }
               )
            };
            string fromdate = "2017-02-05";
            string todate = "2017-02-20";
            string ids = "obama";
            int roomGroup = 1;
          string  currency = "USD";
            var test = divedListRoomPrice(a,ids,roomGroup,currency,fromdate,todate);
       }

        public static List<RoomPrice> divedListRoomPrice(Rates a,string ids,int roomGroup,string currency,string fromdate, string todate)
        {
            /* List<int> a=new List<int>(new int[] {1,2,3,3,3,3,3,4,4,5,5,5,3,3,3});*/
            List<RoomPrice> result = new List<RoomPrice>();
            List<Rates> all = new List<Rates>();
            Rates temp = a;
            temp=System.ObjectExtensions.Copy(a);
            temp.daily_prices.Clear();
            int i = 0;
            foreach (var item in a.daily_prices)
            {
                i++;
                if (i == 1)
                {
                    temp.daily_prices.Add(item);
                }
               
                else
                {
                    if (temp.daily_prices.Count > 0)
                    {
                        if (temp.daily_prices.LastOrDefault() != item)
                        {
                            all.Add(temp);
                            temp. daily_prices.Clear();//reset
                            
                            temp.daily_prices.Add(item);
                        }
                        else
                        {
                            temp.daily_prices.Add(item);
                        }
                    }
                }
                if (i == a.daily_prices.Count())
                {
                    all.Add(temp);
                }

            }
            //----------Div Day
            DateTime fromDate=Convert.ToDateTime(fromdate);
            DateTime toDate=Convert.ToDateTime(todate);
            DateTime tempDate=fromDate;
            foreach (var item in all)
            {
               
                result.Add(new RoomPrice()
                {Ids=ids,
                fromdate = tempDate.ToString(),
                todate = tempDate.AddDays(item.daily_prices.Count()).ToString(),
                    room_group_id=roomGroup,
                    Price=item.daily_prices.Average(),
                    Currency=currency


                });
                tempDate = tempDate.AddDays(item.daily_prices.Count()+1);
            }

            return result;
        }



    }
}

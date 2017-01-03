using System.Diagnostics;
using System.IO;
using System.Net;
using Ostrovok2Be.Models;

namespace Ostrovok2Be.RequestType
{
    public class GetRate:Form1
    {
        public static ReturnObject getRateHotelInforByIds(string listIds, string checkin = "", string checkout = "", string currency = "USD")
        {

            try
            {
                Debug.WriteLine(" GET RATE: " + listIds + " on Lang: " + currency + " CheckIn  " + checkin + "  " + checkout);
                if (!string.IsNullOrEmpty(listIds))
                {
                    System.Threading.Thread.Sleep(1000);
                    string api_getAllHotelByLocationText =
                        @"https://partner.ostrovok.ru/api/b2b/v2/hotel/rates?data={""ids"":[""" + listIds + @"""],""checkin"":""" + checkin + @""",""checkout"":""" + checkout + @""",""currency"":""" + currency + @""",""lang"":""en""}";
                    CookieContainer myContainer = new CookieContainer();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_getAllHotelByLocationText);
                    request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
                    request.CookieContainer = myContainer;
                    request.PreAuthenticate = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string result = reader.ReadLine();
                    if ((int)response.StatusCode == 429)//server denied --> too much request today.
                        pause = true;
                    return new ReturnObject()
                    {
                        Code = (int)(int)response.StatusCode,
                        Result = result
                    };


                }
                else
                {
                    return null;
                }
            }
            catch
            {
               
                return null;

            }
          

        }
        //---- check rate cũng tương tự: 
    }
}
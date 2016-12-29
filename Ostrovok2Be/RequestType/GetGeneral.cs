using System.Diagnostics;
using System.IO;
using System.Net;

namespace Ostrovok2Be.RequestType
{
    public class GetGeneral:Form1
    {
        //--------------------
        public static string getGeneralHotelInforByIds(string listIds, string lang = "en")
        {
            Debug.WriteLine(" Downloading: " + listIds + " on Lang: " + lang);
            if (!string.IsNullOrEmpty(listIds))
            {
                System.Threading.Thread.Sleep(Form1.timeIdle);
                string api_getAllHotelByLocationText =
                    @"https://partner.ostrovok.ru/api/b2b/v2/hotel/list?data={""ids"":[""" + listIds +@"""],""lang"":""" + lang + @"""}";
                CookieContainer myContainer = new CookieContainer();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_getAllHotelByLocationText);
                request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
                request.CookieContainer = myContainer;
                request.PreAuthenticate = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadLine();
                return result;
            }
            else
            {
                return null;
            }

        }
        //---- check rate cũng tương tự: 
    }
}
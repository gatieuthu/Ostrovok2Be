using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ostrovok2Be.Function;
using Ostrovok2Be.Models;

namespace Ostrovok2Be
{
    public partial class Form1 : Form
    {
        private static int timeIdle;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // MessageBox.Show("Form loaded","warning !");
            //--- set the time between 2 connections:
            timeIdle = Int32.Parse(idletime.Text);
             string locationlist = File.ReadAllText("../../Object/countrylist.txt");
            
           using (WebClient wc = new WebClient())
           {
               /* string urlImageList = baseurl + "api/SupplierPhotoAPI/GetListImageDetail?supplierId=" + supplierId;*/
               wc.Encoding = System.Text.Encoding.UTF8;
               var country = JsonConvert.DeserializeObject<List<country>>(locationlist);
               //--- clear checkbox list
               countrylist.Items.Clear();
               //----------
               var ctrlist = countrylist.Items;
               foreach (var item in country)
               {
                   ctrlist.Add(item.value);
               }
           }
            
           
            
       
           

        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {

        }

        public void TaskGetHotelGeneral(string value, int type=0,int runtype=0)
        {
            process_lb.Text = "TASK: Get Hotel Info...";
            //type =0 get by region_id
            List<string> listHotelInRegion = new List<string>();
            if (type == 0)
            {
                listHotelInRegion = getAllHotelByRegionId(value); // get all hotel in quynhon
            }
            else
            {//type =1 get by ids
                listHotelInRegion = getAllHotelByText(value);
            }
            //--- STARTING TASK
            //---1. GET VALUE FROM API
            var divedList = list2objbuilder.ListCreater(listHotelInRegion, 10);
            var listObj = new List<JToken>();
            var value_Track = 0;
            foreach (var tasklist in divedList)
            {
                value_Track++;
                var tempStr2Obj = new str2objbuilder(tasklist);
                var allHotelInStr = tempStr2Obj.listIds2Object();
                var result = getGeneralHotelInforByIds(allHotelInStr);
                listObj.Add(result);
               backgroundWorkerUpdate(value_Track, divedList.Count());


            }
            //----------------

            //----2.CREATE OBJECT

            //-----3.SAVE OBJECT TO EXCEL

            //----4. SAVE LOG

            process_lb.Text = "TASK: Done";
         Debug.WriteLine(" hehehe");
             
        }

        public  void backgroundWorkerUpdate( int currentValue, int Count)
        {
        
            bgWorker.ReportProgress(currentValue * 100 / Count);

        }
        public List<string> getAllHotelByText(string textValue)
        {   
            System.Threading.Thread.Sleep(timeIdle);
            var totalHotels = new List<string>();
            string api_getAllHotelByLocationText = @"https://partner.ostrovok.ru/api/b2b/v2/region/hotel/list?data={""text"":""" + textValue + @"" + @""",""format"":""json""}"; 
            api_getAllHotelByLocationText.Replace(@"\","");
            CookieContainer myContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_getAllHotelByLocationText);
            request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
            request.CookieContainer = myContainer;
            request.PreAuthenticate = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadLine();
            JObject hotelListobj = JObject.Parse(result);
            string content_country = hotelListobj["result"]["ids"].ToString();
            List<string> allHotels = JsonConvert.DeserializeObject<List<string>>(content_country);
            return allHotels;
            
        }   
        public List<string>getAllHotelByRegionId(string textValue)
        {//------------------https://1356:f5df4f22-1277-44a7-a7fc-56b5b2de93da@partner.ostrovok.ru/api/b2b/v2/region/hotel/list?data={"region_id":"3620","format":"json"}
            System.Threading.Thread.Sleep(timeIdle);
            var totalHotels = new List<string>();
            string api_getAllHotelByLocationText = @"https://partner.ostrovok.ru/api/b2b/v2/region/hotel/list?data={""region_id"":""" + textValue + @"" + @""",""format"":""json""}"; 
            api_getAllHotelByLocationText.Replace(@"\","");
            CookieContainer myContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_getAllHotelByLocationText);
            request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
            request.CookieContainer = myContainer;
            request.PreAuthenticate = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadLine();
            JObject hotelListobj = JObject.Parse(result);
            string content_country = hotelListobj["result"]["ids"].ToString();
            List<string> allHotels = JsonConvert.DeserializeObject<List<string>>(content_country);
            return allHotels;
            
        }
        //--------------------
        public JToken getGeneralHotelInforByIds( string listIds)
        {// link demo: https://1356:f5df4f22-1277-44a7-a7fc-56b5b2de93da@partner.ostrovok.ru/api/b2b/v2/hotel/list?data={"ids":["dong_khanh_hotel"],"lang":"en"}
            System.Threading.Thread.Sleep(timeIdle);
            string api_getAllHotelByLocationText = @"https://partner.ostrovok.ru/api/b2b/v2/hotel/list?data={""ids"":[""" + listIds + @"""],""lang"":""en""}";
            CookieContainer myContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_getAllHotelByLocationText);
            request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
            request.CookieContainer = myContainer;
            request.PreAuthenticate = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadLine();
         
            //return JObject.Parse(result)["result"];
            return result;
        }
        //---- check rate cũng tương tự:

        public JToken getRatesByIds(string listIds)
        {// link demo: https://1356:f5df4f22-1277-44a7-a7fc-56b5b2de93da@partner.ostrovok.ru/api/b2b/v2/hotel/list?data={"ids":["dong_khanh_hotel"],"lang":"en"}
            System.Threading.Thread.Sleep(timeIdle);
            string api_getAllHotelByLocationText = @"https://partner.ostrovok.ru/api/b2b/v2/hotel/list?data={""ids"":[""" + listIds + @"""],""lang"":""en""}";
            CookieContainer myContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_getAllHotelByLocationText);
            request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
            request.CookieContainer = myContainer;
            request.PreAuthenticate = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadLine();

            return JObject.Parse(result)["result"];
        }
        private void startbtn_Click(object sender, EventArgs e)
        {
            /*var temp=  getAllHotelByText("vietnam");
            Begodi.CreateExcelFile.CreateExcelDocument(temp,"../../Result/data.xls");*/
            TaskGetHotelGeneral("3620", 0, 0);
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            pBar.Value = Math.Min(e.ProgressPercentage, 100);
            pBar.Update();
        }

    }
    
}

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
using Ostrovok2Be.Models.getFromOstrovok;
using Ostrovok2Be.Models.MidleObject;

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

        public void TaskGetHotelGeneral(string value, int type=0,int runtype=0,string lang="en")
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
            var totalPackage = new List<GeneralPackage>();
            foreach (var tasklist in divedList)
            {
                value_Track++;
                var tempStr2Obj = new str2objbuilder(tasklist);
                var allHotelInStr = tempStr2Obj.listIds2Object();
                var result = getGeneralHotelInforByIds(allHotelInStr,"en");
                listObj.Add(result);
               backgroundWorkerUpdate(value_Track, divedList.Count());
               var temGenPackage = JsonConvert.DeserializeObject<GeneralPackage>(result);
               totalPackage.Add(temGenPackage);


            }
            //----------------

            //----2.CREATE OBJECT
            var ListMiddleObject = new List<SupplierMidleObject>();
            foreach (var package in totalPackage)
            {

                foreach (var item in package.result)
                {
                     var temMidleObject = new SupplierMidleObject()
                    {
                        SupplierOstIds=item.id,
                        Country_code=item.country_code,
                        Images=item.images.ToString(),
                        Thumbnail=item.thumbnail,
                        Adress_clean=item.clean_address,
                        Lat=item.latitude,
                        Long=item.longitude,
                        Kind=item.kind,
                        Phone=item.phone, 
                        Email=item.email,
                        RegionId=item.region_id,
                        AmenitiesEn=lang=="en"?item.amenities.ToString():"",
                        AmenitiesRu=lang=="ru"?item.amenities.ToString():"",
                        DescriptionEn=lang=="en"?item.description.ToString():"",
                        DescriptionRu = lang == "ru" ? item.description.ToString() : "",
                        CityEn=lang=="en"?item.city:"", 
                        CityRu=lang=="ru"?item.city:"",
                        CountryEn = lang == "en" ? item.country : "",
                        CountryRu = lang == "ru" ? item.country : "",
                        Policy_descriptionRu=lang=="ru"?item.policy_description:"",
                        Policy_descriptionEn=lang=="en"?item.policy_description:"",
                        SupplierOstNameEn=lang=="en"?item.name:"",
                        SupplierOstNameRu=lang=="ru"?item.name:"",
                       AddressEn=lang=="en"?item.address:"",
                       AddressRu=lang=="ru"?item.address:""
                    };
                    ListMiddleObject.Add(temMidleObject);
                }
               
            }

            //-----3.SAVE OBJECT TO EXCEL

            process_lb.Text = "TASK: Save Exelfile";
            Begodi.CreateExcelFile.CreateExcelDocument(ListMiddleObject, @"../../Raw/rawdata.xls");
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
        public string getGeneralHotelInforByIds( string listIds,string lang="en")
        {// link demo: https://1356:f5df4f22-1277-44a7-a7fc-56b5b2de93da@partner.ostrovok.ru/api/b2b/v2/hotel/list?data={"ids":["dong_khanh_hotel"],"lang":"en"}
            System.Threading.Thread.Sleep(timeIdle);
            string api_getAllHotelByLocationText = @"https://partner.ostrovok.ru/api/b2b/v2/hotel/list?data={""ids"":[""" + listIds + @"""],""lang"":"""+lang+@"""}";
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
            TaskGetHotelGeneral("3620", 0, 0,"en");
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            pBar.Value = Math.Min(e.ProgressPercentage, 100);
            pBar.Update();
        }

    }
    
}

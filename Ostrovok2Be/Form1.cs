using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ostrovok2Be.Function;
using Ostrovok2Be.Models;
using Ostrovok2Be.Models.getFromOstrovok;
using Ostrovok2Be.Models.LogObject;
using Ostrovok2Be.Models.MidleObject;

namespace Ostrovok2Be
{
    public partial class Form1 : Form
    {
        //-------------DECLARE AREA--------------
        private static int timeIdle;
        private static int runmode;
        public static List<string> List_Ids = new List<string>();
        public static string  pathLog = @"../../Result/Log/Log.xls";
        public  string currentIds = "";
        public static bool pause = false;
        public ConcurrentBag<Task> AllTanks = new ConcurrentBag<Task>();
        public ConcurrentBag<string> allIdsDone = new ConcurrentBag<string>();
        public ConcurrentBag<LogObject> AllLogs = new ConcurrentBag<LogObject>();

        //--------- COMPONENT EVENT---------------------------------------------------

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //--- set the time between 2 connections:
            timeIdle = Int32.Parse(idletime.Text);
            runmode = 0;
            string locationlist = File.ReadAllText("../../Object/countrylist.txt");
            //------------------------
            if (File.Exists(pathLog))
            {
                  FileStream stream = File.Open(pathLog, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result1 = excelReader.AsDataSet();
            IEnumerable<LogObject> allItemLog = from row in result1.Tables["Table1"].AsEnumerable()
                                                select new LogObject()
                                                {
                                                    Ids = Convert.ToString(row["Ids"]),
                                                    Done = Convert.ToString(row["Done"]),
                                                    DateCreated = Convert.ToString(row["DateCreated"]),
                                                    DateUpdated = Convert.ToString(row["DateUpdated"])
                                                };
                if (allItemLog.Count() > 0)
                {
                    foreach (var item in allItemLog)
                    {
                        AllLogs.Add(item);
                    }
                    
                }
            }
          
            //-----------------------
            using (WebClient wc = new WebClient())
            {
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
            ListIdsCreator();

        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {


        }
        private  void startbtn_Click(object sender, EventArgs e)
        {
           
            var allLangSelected = getAllLangSelected();
            var allIds = ListIdsCreator();
            foreach (var lang in allLangSelected)
            {
           
                Task taskA = Task.Factory.StartNew(() =>  TaskGetHotelGeneral(allIds, 0, runmode, lang.ToLower()));
                AllTanks.Add(taskA);

            }
            
        }


        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
          /*  pBar.Value = Math.Min(e.ProgressPercentage, 100);
            pBar.Update();*/
        }
        private void ExitProgram(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_continue_Click(object sender, EventArgs e)
        {
            ListIdsCreator();
        }
        private void btn_Pause_Click(object sender, EventArgs e)
        {
            //check file Log exist

                pause = true;
            Task.WaitAll(AllTanks.ToArray());
            Debug.WriteLine(" Currentids in Pause Function: "+currentIds);
            if (File.Exists(pathLog))
            {
                //--- Hoi update hay tao moi

                string message = "Log file was exited";
                string caption = "Please confirm";
                MessageBoxButtons buttons = MessageBoxButtons.AbortRetryIgnore;
                MessageBoxManager.Ignore = "Cancel";
                MessageBoxManager.Retry = "Create New";
                MessageBoxManager.Abort = "Update";
                MessageBoxManager.Register();
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.Retry)
                {
                    // clear all content, and create a new Log
                    createNewLog(currentIds);

                }
                if (result == DialogResult.Abort)
                {
                    //UpdateLog
                    UpdateLog();
                }

            }
            else
            {
                createNewLog(currentIds);
            }
            //--------------
        }

        //------------METHODS-------------------------------------------------------------------------------------------

        private void UpdateLog()
        {
            if (allIdsDone.Count() > 0)
            {
                foreach (var doneItem in allIdsDone)
                {
                    AllLogs.Where(c => c.Ids == doneItem).FirstOrDefault().Done = "1";
                }
               
            }
           //------- Create a New Log
            Begodi.CreateExcelFile.CreateExcelDocument(AllLogs.ToList(), pathLog);

        }
        private List<string> getAllLangSelected()
        {
            var allSelectedLang = new List<string>();
            if (cb_En.Checked)
            {
                var tem = "en";
                allSelectedLang.Add(tem);
            } if (cb_Ru.Checked)
            {
                var tem = "ru";
                allSelectedLang.Add(tem);
            } if (cb_De.Checked)
            {
                var tem = "de";
                allSelectedLang.Add(tem);
            }
            return allSelectedLang;
        }


        public void TaskGetHotelGeneral(List<string> allIds, int type = 0, int runtype = 0, string lang = "en")
        {
           /* process_lb.Text = "TASK: Get Hotel Info...";*/

           
            //type =0 get by region_id
         
          
            //--- STARTING TASK        
            //---1. GET VALUE FROM API
            var divedList = list2objbuilder.ListCreater(allIds, 10);
            var listObj = new List<JToken>();
            var value_Track = 0;
            var totalPackage = new List<GeneralPackage>();
            foreach (var tasklist in divedList)
            {
                 if (pause)
                        break;
                value_Track++;
                var tempStr2Obj = new str2objbuilder(tasklist);
                var allHotelInStr = tempStr2Obj.listIds2Object();
                var result = getGeneralHotelInforByIds(allHotelInStr, "en");
                listObj.Add(result);
                backgroundWorkerUpdate(value_Track, divedList.Count());
                var temGenPackage = JsonConvert.DeserializeObject<GeneralPackage>(result);
                totalPackage.Add(temGenPackage);


            }
            //----- The last Ids
            if (totalPackage.Count()>0)
            currentIds = totalPackage.LastOrDefault().result.LastOrDefault().id;
            Debug.WriteLine(" The last ids: "+currentIds +"-------------------------------------");
           //----2.CREATE OBJECT
            var ListMiddleObject = new List<SupplierMidleObject>();
            foreach (var package in totalPackage)
            {
               

                foreach (var item in package.result)
                {
                    

                    var tem = item.amenities.Select(c => c.amenities);
                    var tem1 = tem.Select(c => c.FirstOrDefault());

                    var tem3 = new List<string>();
                    foreach (var ameniti in tem)
                    {
                        tem3.AddRange(ameniti);
                    }
                    var tem2 = string.Join(",", tem3.ToArray());

                    var temMidleObject = new SupplierMidleObject()
                    {
                        SupplierOstIds = item.id,
                        Country_code = item.country_code,
                        Images = JsonConvert.SerializeObject(item.images),
                        Thumbnail = item.thumbnail,
                        Adress_clean = item.clean_address,
                        Lat = item.latitude,
                        Long = item.longitude,
                        Kind = item.kind,
                        Phone = item.phone,
                        Email = item.email,
                        RegionId = item.region_id,
                        AmenitiesEn = lang == "en" ? tem2 : "",
                        AmenitiesRu = lang == "ru" ? tem2 : "",
                        DescriptionEn = lang == "en" ? item.description.ToString() : "",
                        DescriptionRu = lang == "ru" ? item.description.ToString() : "",
                        CityEn = lang == "en" ? item.city : "",
                        CityRu = lang == "ru" ? item.city : "",
                        CountryEn = lang == "en" ? item.country : "",
                        CountryRu = lang == "ru" ? item.country : "",
                        Policy_descriptionRu = lang == "ru" ? item.policy_description : "",
                        Policy_descriptionEn = lang == "en" ? item.policy_description : "",
                        SupplierOstNameEn = lang == "en" ? item.name : "",
                        SupplierOstNameRu = lang == "ru" ? item.name : "",
                        AddressEn = lang == "en" ? item.address : "",
                        AddressRu = lang == "ru" ? item.address : ""
                    };
                    ListMiddleObject.Add(temMidleObject);
                }

            }

            //-----3.SAVE OBJECT TO EXCEL

            /*process_lb.Text = "TASK: Save Exelfile";*/
            Begodi.CreateExcelFile.CreateExcelDocument(ListMiddleObject, @"../../Result/Raw/raw_GeneralData.xls");
            //----4. SAVE LOG
            
            /*process_lb.Text = "TASK: Done";*/
           

        }

        public void backgroundWorkerUpdate(int currentValue, int Count)
        {

            bgWorker.ReportProgress(currentValue * 100 / Count);

        }

        public bool createNewLog(string ids)
        {
            try
            {
                bool result = false;
                //----------------Build list of ObjectLog
                var allItemInLog = new List<LogObject>();
                var hasit = false;
                hasit = List_Ids.Contains(ids);
                bool reach = false;
                foreach (var item in List_Ids)
                {
                   
                    if (reach == false&&hasit==true)
                    {
                        allItemInLog.Add(new LogObject()
                        {

                            Ids = item,
                            Done = "1",
                            DateCreated = DateTime.Now.ToString()
                        });
                    }
                    else
                    {
                        allItemInLog.Add(new LogObject()
                        {

                            Ids = item,
                            Done = "0",
                            DateCreated = DateTime.Now.ToString()
                        });
                        
                    }

                    if (item == ids)
                        reach = true;

                }
                if (allItemInLog.Count() > 0)
                {
                    Begodi.CreateExcelFile.CreateExcelDocument(allItemInLog, pathLog);
                    result = true;
                }
                //----------
                return result;
            }
            catch
            {
                return false;
            }

           
        }
        
        #region Method for requesting..
        public List<string> getAllHotelByText(string textValue)
        {
             if (string.IsNullOrEmpty(textValue))
                return null;
               try
            {
            System.Threading.Thread.Sleep(timeIdle);
            var totalHotels = new List<string>();
            string api_getAllHotelByLocationText = @"https://partner.ostrovok.ru/api/b2b/v2/region/hotel/list?data={""text"":""" + textValue + @"" + @""",""format"":""json""}";
            api_getAllHotelByLocationText.Replace(@"\", "");
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
               catch
               {
                   return null;
               }

        }
        public List<string> getAllHotelByRegionId(string textValue)
        {
            if (string.IsNullOrEmpty(textValue))
                return null;
               try
            {
                System.Threading.Thread.Sleep(timeIdle);
                var totalHotels = new List<string>();
                string api_getAllHotelByLocationText =
                    @"https://partner.ostrovok.ru/api/b2b/v2/region/hotel/list?data={""region_id"":""" + textValue + @"" +
                    @""",""format"":""json""}";
                api_getAllHotelByLocationText.Replace(@"\", "");
                CookieContainer myContainer = new CookieContainer();
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(api_getAllHotelByLocationText);
                request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
                request.CookieContainer = myContainer;
                request.PreAuthenticate = true;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadLine();
                JObject hotelListobj = JObject.Parse(result);
                string content_country = hotelListobj["result"]["ids"].ToString();
                List<string> allHotels = JsonConvert.DeserializeObject<List<string>>(content_country);
                return allHotels;
            }
            catch
            {
                return null;
            }
           

        }
        //--------------------
        public string getGeneralHotelInforByIds(string listIds, string lang = "en")
        {// link demo: https://1356:f5df4f22-1277-44a7-a7fc-56b5b2de93da@partner.ostrovok.ru/api/b2b/v2/hotel/list?data={"ids":["dong_khanh_hotel"],"lang":"en"}
            if (!string.IsNullOrEmpty(listIds))
            {
                System.Threading.Thread.Sleep(timeIdle);
                string api_getAllHotelByLocationText =
                    @"https://partner.ostrovok.ru/api/b2b/v2/hotel/list?data={""ids"":[""" + listIds +
                    @"""],""lang"":""" + lang + @"""}";
                CookieContainer myContainer = new CookieContainer();
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(api_getAllHotelByLocationText);
                request.Credentials = new NetworkCredential("1356", "f5df4f22-1277-44a7-a7fc-56b5b2de93da");
                request.CookieContainer = myContainer;
                request.PreAuthenticate = true;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadLine();
                if (listIds.Contains(","))
                {
                    foreach (var item in listIds.Split(','))
                    {
                        allIdsDone.Add(item);
                    }
                }
                else
                {
                    allIdsDone.Add(listIds);
                }
                return result;
            }
            else
            {
                return null;
            }
          
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

        public List<string> ListIdsCreator()
        {
            var result = new List<string>();
            //---1 -Convert from COUNTRY LIST
           
            var listCountrySelected=countrylist.CheckedItems.OfType<string>().ToList();
            //-------------
            foreach (var task in listCountrySelected)
            {
                var temListIds = getAllHotelByText(task);
                if (temListIds != null)
                {
                    result.AddRange(temListIds);
                }
              

            }

            //----Convert from region id
            var listRegionId = new List<string>();
            if (tb_listIds.Text != null)
            {
                if (tb_listIds.Text.Contains(","))
                {
                    listRegionId.AddRange(tb_listIds.Text.Split(','));

                }
                else
                {
                    listRegionId.Add(tb_listIds.Text);
                }
            }
           
            if (listRegionId.Count() > 0)
            {
                 foreach (var regionId in listRegionId)
                 {
                     var temListIds = getAllHotelByRegionId(regionId);
                     if (temListIds != null)
                     {
                         result.AddRange(temListIds);
                     }
               
            }

            }


            List_Ids = result;
            return result;
        }
        #endregion
        #region Event phu cho component
        private void btn_continue_MouseHover(object sender, EventArgs e)
        {
            lb_Info.Text = "Continue from Log";
        }

        private void btn_continue_MouseLeave(object sender, EventArgs e)
        {
            lb_Info.Text = "";
        }
        private void btn_Pause_MouseHover(object sender, EventArgs e)
        {
            lb_Info.Text = "Save log->Stop";
        }

        private void btn_Pause_MouseLeave(object sender, EventArgs e)
        {
            lb_Info.Text = "";
        }
       

        private void rdAuto_MouseHover(object sender, EventArgs e)
        {
            lb_Info.Text = "Start form new ";
        }

        private void rdAuto_MouseLeave(object sender, EventArgs e)
        {
            lb_Info.Text = "";
        }
 

        private void radioButton2_MouseHover(object sender, EventArgs e)
        {
            lb_Info.Text = "Start from the log !";
        }

        private void radioButton2_MouseLeave(object sender, EventArgs e)
        {
            lb_Info.Text="";
        }

        private void rd_Check_MouseHover(object sender, EventArgs e)
        {
            lb_Info.Text = "only check, and update status";
        }

        private void rd_Check_MouseLeave(object sender, EventArgs e)
        {
            lb_Info.Text = "";
        }

        private void rd_Check_Update_MouseHover(object sender, EventArgs e)
        {
            lb_Info.Text = "Update if data was updated from ostrovok";
        }

        private void rd_Check_Update_MouseLeave(object sender, EventArgs e)
        {
            lb_Info.Text = "";
        }
        #endregion

        private void countrylist_MouseClick(object sender, MouseEventArgs e)
        {
            var selecteditem=countrylist.SelectedItem;
            
            /*MessageBox.Show(selecteditem.ToString());*/
            /*countrylist.SetItemChecked(countrylist.SetItemChecked);*/
        }

      

      

      
    }
    
}

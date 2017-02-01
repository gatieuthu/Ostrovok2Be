using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
using Omu.ValueInjecter;
using Ostrovok2Be.Function;
using Ostrovok2Be.Models;
using Ostrovok2Be.Models.getFromOstrovok;
using Ostrovok2Be.Models.getGeneral;
using Ostrovok2Be.Models.getRates;
using Ostrovok2Be.Models.LogObject;
using Ostrovok2Be.Models.MidleObject;
using Ostrovok2Be.RequestType;

namespace Ostrovok2Be
{
    public partial class Form1 : Form
    {
        //-------------DECLARE AREA--------------
        public static int timeIdle=1000;
        public static int runmode=0;
        public static List<string> List_Ids = new List<string>();
        public static string  pathLogGetPrice = @"../../Result/Log/Log-Price.xlsx";
        public static string  pathLogGetGeneralInfo = @"../../Result/Log/Log-Info.xlsx";
        public static string pathRoomPrice = @"../../Result/RoomPrice/RoomPrice.xlsx";
        public static string pathGeneralHotelInfo = @"../../Result/GeneralHotelInfo/GeneralHotelInfoData.xlsx";
        public static string pathCountryList = @"../../Object/countrylist.txt";
        public  string currentIds = "";
        public static bool pause = false;
        public ConcurrentBag<Task> AllTasks = new ConcurrentBag<Task>();
        public ConcurrentBag<string> allIdsDone = new ConcurrentBag<string>();
        public ConcurrentBag<LogObject> AllLogs = new ConcurrentBag<LogObject>();
        public static int idsPerUnit = 10;
        public static List<string> AllCurrencyType =new List<string>();
        public static List<string> AllLangSelected =new List<string>();
        public static List<RoomPrice> AllRoomPrice = new List<RoomPrice>();
        public static string checkInDate;
        public static string checkOutDate;
        public static string connectStr = "";
        public static int choosen_SaveType=0;
        public static bool startNew = true;

        //--------- COMPONENT EVENT---------------------------------------------------

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                dt_Fromdate.Value =Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
           //--- set the time between 2 connections:
            timeIdle = Int32.Parse(idletime.Text);
            runmode = 0;
            string locationlist = File.ReadAllText(pathCountryList);
            var pathLog = runmode == 1 ? pathLogGetGeneralInfo : pathLogGetPrice;
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
            
            Console.WriteLine(" ids count:    " + List_Ids.Count());

        }

      
        private  void startbtn_Click(object sender, EventArgs e)
        {
            startbtn.Text = "Started";
            startbtn.Enabled = false;
            getState();
            startNew = true;
            var allIds = ListIdsCreator(1);
            //-- TASK: GET-GENERALINFO
            if (runmode == 1)
            { 
                     if (allIds.Count > 0)
                        {
                            
                       
                                        Task task = new Task(() => GetHotelGeneral(allIds, 0, runmode));
                                        AllTasks.Add(task);
                                        task.Start();
                        
           
                      
                         }
                        else
                        {
                            MessageBox.Show(" Plz select Ids");
                        }
            }
           
            //--TASK GET-HOTELPRICE
            if (runmode == 2)
            {
                if (allIds.Count > 0)
                        {
                             
                                Task task = new Task(() => GetPrice());
                                AllTasks.Add(task);
                                task.Start();
                        }
                        else
                        {
                            MessageBox.Show(" Plz select Ids");
                        }
            }
            
           // Task.WaitAll(AllTasks.ToArray());
            startbtn.Text = "Start";
            startbtn.Enabled = true;
            
        }


        public void getState()
        {
            //-----Get State Currency
            if (cb_Rub.Checked)
            {
                AllCurrencyType.Add("RUB");
            }  
            if (cb_Usd.Checked)
            {
                AllCurrencyType.Add("USD");
            } 
            if (cb_Vnd.Checked)
            {
                AllCurrencyType.Add("VND");
            } 
            if (cb_Eur.Checked)
            {
                AllCurrencyType.Add("EUR");
            }  
            //--------------GetSelectLangquage
            if (cb_En.Checked)
            {
                AllLangSelected.Add("en");
            }
            if (cb_Ru.Checked)
            {
                AllLangSelected.Add("ru");
            } /*if (cb_De.Checked)
            {
                AllLangSelected.Add("de");
            } */
            //----- Get Fromdate and ToDAte
            checkInDate = dt_Fromdate.Value.ToString("yyyy-MM-dd");
            checkOutDate = dt_Todate.Value.ToString("yyyy-MM-dd");
            //get Run Mode
            if (rd_GetGeneralInfo.Checked)
                runmode = 1;

            if (rd_getPrice.Checked)
                runmode = 2; 
            if (rd_Auto.Checked)
                runmode = 0;
            //---get Savetype in Default mode
            if (rd_SaveGen.Checked)
            {
                choosen_SaveType = 1;
            }
            if (rd_SavePrice.Checked)
            {
                choosen_SaveType = 2;
            }


        }
        public void GetPrice()
        {
           // ListIDS, LIST Lang, RunMode, List Currency
            List<RatesPackage> temp = new List<RatesPackage>();
            var allGroup = list2objbuilder.ListCreator(List_Ids, idsPerUnit);
            foreach (var itemGroup in allGroup)//---------> Loop in allUnit
                   {
               
                                    List<Task> allTaskInGroupIds = new List<Task>();
                                    var HotelInStr = new str2objbuilder(itemGroup).listIds2Object();
                                    List<string> result_getRates = new List<string>();
                                    List<RatesPackage> getRatesObject = new List<RatesPackage>();

                                    object lockObj = new object();          
                                    foreach (var currencyItem in AllCurrencyType)
                                    {
                                        //Request cac loai tien te.
                                        Task<ReturnObject> getRatePackage = new Task<ReturnObject>(() => GetRate.getRateHotelInforByListOfIds(HotelInStr, checkInDate, checkOutDate, currencyItem));
                                        getRatePackage.Start();
                                       
                                        //----------------
                                        lock (lockObj)
                                        {
                                            if (getRatePackage.Result != null)
                                            {
                                                if (getRatePackage.Result.Code == 200)
                                                {
                                                    allTaskInGroupIds.Add(getRatePackage);
                                                    result_getRates.Add(getRatePackage.Result.Result);
                                                }
                                                else
                                                    pause = true;
                                            }
                                            

                                        }
                                    }
                                    var allTaskInArray = allTaskInGroupIds.ToArray();
                                    if (allTaskInArray.Length>0)
                                        Task.WaitAll(allTaskInArray);
                                    //---------------Map Object
                                 foreach (var item in result_getRates) //Currency
                                 {
                                     
                                        var tempRateObj = JsonConvert.DeserializeObject<RatesPackage>(item);
                                        getRatesObject.Add(tempRateObj);
                                        if (tempRateObj.result.hotels.Count() > 0)
                                        {
                                            List<RoomPrice> tempListRoomPrice = new List<RoomPrice>();
                                            foreach (var rates in tempRateObj.result.hotels)//Hotels
                                            {
                                                foreach (var perRoom in rates.rates)//Room
                                                {
                                                     var tempRoomPrice = new RoomPrice()
                                                    {
                                                        Ids=rates.id,
                                                        fromdate=checkInDate,
                                                        todate=checkOutDate,
                                                        Currency = perRoom.rate_currency,
                                                        Price = perRoom.daily_prices.Average(),
                                                        room_group_id = perRoom.room_group_id
                                                    };
                                                     tempListRoomPrice.Add(tempRoomPrice);

                                                }
                                                var tempMinRoomPrice = tempListRoomPrice.Where(c=>c.Price==tempListRoomPrice.Select(p=>p.Price).Min()).FirstOrDefault();
                                                if (tempMinRoomPrice != null)
                                                {
                                                    AllRoomPrice.Add(tempMinRoomPrice);
                                                }
                                               
                                            }
                                        }
                                        
                                    }
                                  
               

                                    //-------------Save to Excel
                                
                       if (pause)
                       {
                           break;
                       }
                           


                   }
            if (startNew)
                Begodi.CreateExcelFile.CreateExcelDocument(AllRoomPrice, pathRoomPrice);
            else
            {//append 
                UpdatePrice(AllRoomPrice);
            }

        }

        private void UpdatePrice(List<RoomPrice> AllRoomPrice)
        {
            // Get all room Price in Excel and bind to object
            FileStream stream = File.Open(pathRoomPrice, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            IEnumerable<RoomPrice> allRoomPrice = from row in result.Tables["Table1"].AsEnumerable()
                                                  select new RoomPrice()
                                                  {
                                                      Ids = Convert.ToString(row["Ids"]),
                                                      fromdate = Convert.ToString(row["fromdate"]),
                                                      todate = Convert.ToString(row["todate"]),
                                                      room_group_id = Convert.ToString(row["room_group_id"]) != null ? int.Parse(Convert.ToString(row["room_group_id"])) : 0,
                                                      Price = Convert.ToString(row["Price"]) != null ? float.Parse(Convert.ToString(row["Price"])) : 0,
                                                      Currency = Convert.ToString(row["Currency"])

                                                  };
            AllRoomPrice.AddRange(allRoomPrice.ToList());
            ListIdsCreator(2);
            Begodi.CreateExcelFile.CreateExcelDocument(AllRoomPrice, pathRoomPrice);


        }
        
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
         /*   btn_continue.Text = "Started";
            btn_continue.Enabled = false;*/

            /*  pBar.Value = Math.Min(e.ProgressPercentage, 100);
            pBar.Update();*/
        }
        private void ExitProgram(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_continue_Click(object sender, EventArgs e)
        {
            startNew = false;
            getState();
          var allIds=  ListIdsCreator(2);
            var pathLog = runmode == 1 ? pathLogGetGeneralInfo : pathLogGetPrice;
            var pathSouce = runmode == 1 ? pathGeneralHotelInfo : pathRoomPrice;
            if (runmode == 1)
            {
                if (allIds.Count > 0)
                {


                    Task task = new Task(() => GetHotelGeneral(allIds, 0, runmode));
                    AllTasks.Add(task);
                    task.Start();
                }
                else
                {
                    MessageBox.Show(" Plz select Ids");
                }
            }

            //--TASK GET-HOTELPRICE
            if (runmode == 2)
            {
                if (allIds.Count > 0)
                {

                    Task task = new Task(() => GetPrice());
                    AllTasks.Add(task);
                    task.Start();
                }
                else
                {
                    MessageBox.Show(" Plz select Ids");
                }
            }
            
           
        }
    
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            btn_Connect.Text = "Connecting";
            try
            {
                //---- creating string.....

                List<string> numTables = new List<string>();

                string connStr = @"Data Source =" + Ip_txt.Text + "; Initial Catalog = " + db_Name_txt.Text + "; User ID =" + tb_User_txt.Text + "; password = " + dt_Password_txt.Text;
                //MessageBox.Show(connStr);
                string sql = "SELECT * FROM information_schema.tables";


                SqlConnection conn = new SqlConnection(connStr);
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[2].ToString() != "sysdiagrams")
                        {
                            numTables.Add(reader[2].ToString());

                        }
                    }

                    //MessageBox.Show(numTables.Count().ToString());

                    reader.Close();//Đóng SqlDataReader
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
                finally
                {
                    conn.Close();
                }
                if (numTables.Count() > 0)
                {

                    btn_Connect.Text = "Connected";
                    btn_Connect.Enabled = false;
                    connectStr = connStr;
                    // MessageBox.Show(connectStr);

                }
                else
                {
                    MessageBox.Show("Try again!");

                }

            }
            catch
            {
                btn_Connect.Text = "Connect";
            }

        }

        private void SaveDb(object sender, EventArgs e)
        {
            try
            {
                //----Read Excel
                // int choosen = 1;
                if (connectStr == "")
                {
                    MessageBox.Show(" Correct the connect string then press connect button !");
                    return;
                }
                if (choosen_SaveType == 1)
                {
                    //Save General Hotel Information
                    //--read data from excel file
                    if (!File.Exists(pathGeneralHotelInfo))
                    {
                        MessageBox.Show(" Check the Path again !");
                        return;
                    }
                    FileStream stream = File.Open(pathGeneralHotelInfo, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();

                    IEnumerable<SupplierMidleObject> allOstrovokSupplier = from row in result.Tables["Table1"].AsEnumerable()
                                                                           select new SupplierMidleObject()
                                                                           {
                                                                               SupplierOstIds = Convert.ToString(row["SupplierOstIds"]),
                                                                               SupplierOstNameEn = Convert.ToString(row["SupplierOstNameEn"]),
                                                                               SupplierOstNameRu = Convert.ToString(row["SupplierOstNameRu"]),
                                                                               Policy_descriptionRu = Convert.ToString(row["Policy_descriptionRu"]),
                                                                               Policy_descriptionEn = Convert.ToString(row["Policy_descriptionEn"]),
                                                                               DescriptionEn = Convert.ToString(row["DescriptionEn"]),
                                                                               DescriptionRu = Convert.ToString(row["DescriptionRu"]),
                                                                               AmenitiesEn = Convert.ToString(row["AmenitiesEn"]),
                                                                               AmenitiesRu = Convert.ToString(row["AmenitiesRu"]),
                                                                               CountryEn = Convert.ToString(row["CountryEn"]),
                                                                               CountryRu = Convert.ToString(row["CountryRu"]),
                                                                               CityRu = Convert.ToString(row["CityRu"]),
                                                                               CityEn = Convert.ToString(row["CityEn"]),
                                                                               RegionId = Convert.ToString(row["RegionId"]),
                                                                               AddressEn = Convert.ToString(row["AddressEn"]),
                                                                               AddressRu = Convert.ToString(row["AddressRu"]),
                                                                               Adress_clean = Convert.ToString(row["Adress_clean"]),
                                                                               Thumbnail = Convert.ToString(row["Thumbnail"]),
                                                                               Lat = Convert.ToString(row["Lat"]),
                                                                               Long = Convert.ToString(row["Long"]),
                                                                               Country_code = Convert.ToString(row["Country_code"]),
                                                                               Kind = Convert.ToString(row["Kind"]),
                                                                               Phone = Convert.ToString(row["Phone"]),
                                                                               Email = Convert.ToString(row["Email"]),
                                                                               Images = Convert.ToString(row["Images"]),
                                                                               Contract_slug = Convert.ToString(row["Contract_slug"])

                                                                           };
                    // Call store and insert database
                    foreach (var item in allOstrovokSupplier)
                    {
                        var Conn = new SqlConnection(connectStr);
                        var P = new SqlParameter[26];
                        P[0] = new SqlParameter("@SupplierOstIds", SqlDbType.NVarChar);
                        P[0].Value = item.SupplierOstIds;
                        P[1] = new SqlParameter("@SupplierOstNameEn", SqlDbType.NVarChar);
                        P[1].Value = item.SupplierOstNameEn;
                        P[2] = new SqlParameter("@SupplierOstNameRu", SqlDbType.NVarChar);
                        P[2].Value = item.SupplierOstNameRu;
                        P[3] = new SqlParameter("@Policy_descriptionRu", SqlDbType.NVarChar);
                        P[3].Value = item.Policy_descriptionRu;
                        P[4] = new SqlParameter("@Policy_descriptionEn", SqlDbType.NVarChar);
                        P[4].Value = item.Policy_descriptionEn;
                        P[5] = new SqlParameter("@DescriptionEn", SqlDbType.NVarChar);
                        P[5].Value = item.DescriptionEn;
                        P[6] = new SqlParameter("@DescriptionRu", SqlDbType.NVarChar);
                        P[6].Value = item.DescriptionRu;
                        P[7] = new SqlParameter("@AmenitiesEn", SqlDbType.NVarChar);
                        P[7].Value = item.AmenitiesEn;
                        P[8] = new SqlParameter("@AmenitiesRu", SqlDbType.NVarChar);
                        P[8].Value = item.AmenitiesRu;
                        P[9] = new SqlParameter("@CountryEn", SqlDbType.NVarChar);
                        P[9].Value = item.CountryEn;
                        P[10] = new SqlParameter("@CountryRu", SqlDbType.NVarChar);
                        P[10].Value = item.CountryRu;
                        P[11] = new SqlParameter("@CityRu", SqlDbType.NVarChar);
                        P[11].Value = item.CityRu;
                        P[12] = new SqlParameter("@CityEn", SqlDbType.NVarChar);
                        P[12].Value = item.CityEn;
                        P[13] = new SqlParameter("@RegionId", SqlDbType.NVarChar);
                        P[13].Value = item.RegionId;
                        P[14] = new SqlParameter("@AddressEn", SqlDbType.NVarChar);
                        P[14].Value = item.AddressEn;
                        P[15] = new SqlParameter("@AddressRu", SqlDbType.NVarChar);
                        P[15].Value = item.AddressRu;
                        P[16] = new SqlParameter("@Adress_clean", SqlDbType.NVarChar);
                        P[16].Value = item.Adress_clean;
                        P[17] = new SqlParameter("@Thumbnail", SqlDbType.NVarChar);
                        P[17].Value = item.Thumbnail;
                        P[18] = new SqlParameter("@Lat", SqlDbType.NVarChar);
                        P[18].Value = item.Lat;
                        P[19] = new SqlParameter("@Long", SqlDbType.NVarChar);
                        P[19].Value = item.Long;
                        P[20] = new SqlParameter("@Country_code", SqlDbType.NVarChar);
                        P[20].Value = item.Country_code;
                        P[21] = new SqlParameter("@Kind", SqlDbType.NVarChar);
                        P[21].Value = item.Kind;
                        P[22] = new SqlParameter("@Phone", SqlDbType.NVarChar);
                        P[22].Value = item.Phone;
                        P[23] = new SqlParameter("@Email", SqlDbType.NVarChar);
                        P[23].Value = item.Email;
                        P[24] = new SqlParameter("@Images", SqlDbType.NVarChar);
                        P[24].Value = item.Images;
                        P[25] = new SqlParameter("@Contract_slug", SqlDbType.NVarChar);
                        P[25].Value = item.Contract_slug;
                        var dt = AE.SqlHelper.ExecuteDataTable(Conn, CommandType.StoredProcedure, "InsertOstrovokSupplier", P);
                    }
                }
                if (choosen_SaveType == 2)
                {
                    if (!File.Exists(pathRoomPrice))
                    {
                        MessageBox.Show(" Check the Path again !");
                        return;
                    }
                    FileStream stream = File.Open(pathRoomPrice, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();

                    IEnumerable<RoomPrice> allRoomPrice = from row in result.Tables["Table1"].AsEnumerable()
                                                          select new RoomPrice()
                                                          {
                                                              Ids = Convert.ToString(row["Ids"]),
                                                              fromdate = Convert.ToString(row["fromdate"]),
                                                              todate = Convert.ToString(row["todate"]),
                                                              room_group_id = Convert.ToString(row["room_group_id"]) != null ? int.Parse(Convert.ToString(row["room_group_id"])) : 0,
                                                              Price = Convert.ToString(row["Price"]) != null ? float.Parse(Convert.ToString(row["Price"])) : 0,
                                                              Currency = Convert.ToString(row["Currency"])

                                                          };


                    foreach (var item in allRoomPrice)
                    {
                        var Conn = new SqlConnection(connectStr);
                        var P = new SqlParameter[6];
                        P[0] = new SqlParameter("@Ids", SqlDbType.NVarChar);
                        P[0].Value = item.Ids;
                        P[1] = new SqlParameter("@fromdate", SqlDbType.NVarChar);
                        P[1].Value = item.fromdate;
                        P[2] = new SqlParameter("@todate", SqlDbType.NVarChar);
                        P[2].Value = item.todate;
                        P[3] = new SqlParameter("@room_group_id", SqlDbType.NVarChar);
                        P[3].Value = item.room_group_id;
                        P[4] = new SqlParameter("@Price", SqlDbType.NVarChar);
                        P[4].Value = item.Price;
                        P[5] = new SqlParameter("@Currency", SqlDbType.NVarChar);
                        P[5].Value = item.Currency;
                        var dt = AE.SqlHelper.ExecuteDataTable(Conn, CommandType.StoredProcedure, "InsertRoomPriceOst", P);
                    }


                }
            }
            catch
            {
                MessageBox.Show(" Have an Error !");
            }


        }

        private void rd_SaveGen_CheckedChanged(object sender, EventArgs e)
        {
            if (rd_SaveGen.Checked)
            {
                choosen_SaveType = 1;
            }
        }

      
        private void btn_Pause_Click(object sender, EventArgs e)
        {
            

            startbtn.Enabled = true;
            btn_continue.Enabled = true;
            pause = true;
           Task.WaitAll(AllTasks.ToArray());
            createNewLog();
           
            //--------------
        }

        //------------METHODS-------------------------------------------------------------------------------------------
     
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
            } /*if (cb_De.Checked)
            {
                var tem = "de";
                allSelectedLang.Add(tem);
            }*/
            return allSelectedLang;
        }


        public void GetHotelGeneral(List<string> allIds, int type = 0, int runtype = 0)
        {
           /* process_lb.Text = "TASK: Get Hotel Info...";*/
            //type =0 get by region_id
         
          
            //--- STARTING TASK        
            //---1. GET VALUE FROM API
            var divedList = list2objbuilder.ListCreator(allIds, 10);
            var listObj = new List<JToken>();
            var value_Track = 0;
            var totalPackage = new List<GeneralPackage>(); 
            List<GeneraPackageObjByLang> collectGeneralPackage = new List<GeneraPackageObjByLang>();
            foreach (var tasklist in divedList)
            {
                 if (pause)
                        break;
                value_Track++;
                var tempStr2Obj = new str2objbuilder(tasklist);
                var allHotelInStr = tempStr2Obj.listIds2Object();
              
                object obj = new object();
              
                Task taskGetGeneralInfo =new Task(() =>
                {
                    foreach (var lang in AllLangSelected)
                    {
                      var tempdataPackage=GetGeneral.getGeneralHotelInforByIds(allHotelInStr, lang);
                        lock (obj)
                        {
                            try
                            {
                                collectGeneralPackage.Add(
                                    new GeneraPackageObjByLang()
                                    {
                                        GeneralPackage =
                                            JsonConvert.DeserializeObject<GeneralPackage>(tempdataPackage.Result),
                                        lang = lang
                                    });
                            }
                            catch (Exception m)
                            {
                                Debug.WriteLine("Errorrrr: "+m.Message);
                            }
                           
                        }
                      
                    }
                });
                taskGetGeneralInfo.Start();
                taskGetGeneralInfo.Wait();
              
            }
       
           //----2.CREATE OBJECT
            var ListMiddleObject = new List<SupplierMidleObject>();
            foreach (var packageByLang in collectGeneralPackage)
            {

                foreach (var item in packageByLang.GeneralPackage.result)
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
                        AmenitiesEn = packageByLang.lang == "en" ? tem2 : null,
                        AmenitiesRu = packageByLang.lang == "ru" ? tem2 : null,
                        DescriptionEn = packageByLang.lang == "en" ? item.description.ToString() : null,
                        DescriptionRu = packageByLang.lang == "ru" ? item.description.ToString() : null,
                        CityEn = packageByLang.lang == "en" ? item.city : null,
                        CityRu = packageByLang.lang == "ru" ? item.city : null,
                        CountryEn = packageByLang.lang == "en" ? item.country : null,
                        CountryRu = packageByLang.lang == "ru" ? item.country : null,
                        Policy_descriptionRu = packageByLang.lang == "ru" ? item.policy_description : null,
                        Policy_descriptionEn = packageByLang.lang == "en" ? item.policy_description : null,
                        SupplierOstNameEn = packageByLang.lang == "en" ? item.name : null,
                        SupplierOstNameRu = packageByLang.lang == "ru" ? item.name : null,
                        AddressEn = packageByLang.lang == "en" ? item.address : null,
                        AddressRu = packageByLang.lang == "ru" ? item.address : null,
                    };
                    ListMiddleObject.Add(temMidleObject);
                }

            }
            //----------2. MAP OBJECT
            var ListMiddleObject_Mapped = new List<SupplierMidleObject>();
           // Map in ListMiddleObject
                List<string> tempAllIds = ListMiddleObject.Select(ids => ids.SupplierOstIds).Distinct().ToList();
            foreach (var itemIds in tempAllIds)
            {
                var temp_mapped = new SupplierMidleObject();
               
                var allItemEquaIds = ListMiddleObject.Where(ids => ids.SupplierOstIds == itemIds).ToList();
                foreach (var itemMiddleObj in allItemEquaIds)
                {
                    temp_mapped.InjectFrom <IgnoreNulls>(itemMiddleObj);
                }
                if (temp_mapped != null)
                {
                    ListMiddleObject_Mapped.Add(temp_mapped);
                    
                }
            }

            
            
            //-----3.SAVE OBJECT TO EXCEL
            /*process_lb.Text = "TASK: Save Exelfile";*/
            if (startNew)
            {
                 Begodi.CreateExcelFile.CreateExcelDocument(ListMiddleObject_Mapped, pathGeneralHotelInfo);
            
            }
            else
            {// append data
                UpdateGeneraInfo(ListMiddleObject_Mapped);
            }
            //----4. SAVE LOG
            
            /*process_lb.Text = "TASK: Done";*/
           

        }

        private void UpdateGeneraInfo(List<SupplierMidleObject> ListMiddleObject_Mapped)
        {
            //Read From excel and bind to Object
            FileStream stream = File.Open(pathGeneralHotelInfo, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            List<SupplierMidleObject> allObjectDone = new List<SupplierMidleObject>();
          
                DataSet temp_result = excelReader.AsDataSet();
                IEnumerable<SupplierMidleObject> ObjInfo = from row in temp_result.Tables["Table1"].AsEnumerable()
                                                  select new SupplierMidleObject()
                                                  {

                                                      SupplierOstIds = Convert.ToString(row["SupplierOstIds"]),
                                                      SupplierOstNameEn = Convert.ToString(row["SupplierOstNameEn"]),
                                                      SupplierOstNameRu = Convert.ToString(row["SupplierOstNameRu"]),
                                                      Policy_descriptionRu = Convert.ToString(row["Policy_descriptionRu"]),
                                                      Policy_descriptionEn = Convert.ToString(row["Policy_descriptionEn"]),
                                                      DescriptionEn = Convert.ToString(row["DescriptionEn"]),
                                                      DescriptionRu = Convert.ToString(row["DescriptionRu"]),
                                                      AmenitiesEn = Convert.ToString(row["AmenitiesEn"]),
                                                      AmenitiesRu = Convert.ToString(row["AmenitiesRu"]),
                                                      CountryEn = Convert.ToString(row["CountryEn"]),
                                                      CountryRu = Convert.ToString(row["CountryRu"]),
                                                      CityRu = Convert.ToString(row["CityRu"]),
                                                      CityEn = Convert.ToString(row["CityEn"]),
                                                      RegionId = Convert.ToString(row["RegionId"]),
                                                      AddressEn = Convert.ToString(row["AddressEn"]),
                                                      AddressRu = Convert.ToString(row["AddressRu"]),
                                                      Adress_clean = Convert.ToString(row["Adress_clean"]),
                                                      Thumbnail = Convert.ToString(row["Thumbnail"]),
                                                      Lat = Convert.ToString(row["Lat"]),
                                                      Long = Convert.ToString(row["Long"]),
                                                      Country_code = Convert.ToString(row["Country_code"]),
                                                      Kind = Convert.ToString(row["Kind"]),
                                                      Phone = Convert.ToString(row["Phone"]),
                                                      Email = Convert.ToString(row["Email"]),
                                                      Images = Convert.ToString(row["Images"]),
                                                      Contract_slug = Convert.ToString(row["Contract_slug"])

                                                  };
                allObjectDone = ObjInfo.ToList();
            
            ListMiddleObject_Mapped.AddRange(allObjectDone);
            //----------- Save again
            ListIdsCreator(2);
            // Get back again List_id from excel
            Begodi.CreateExcelFile.CreateExcelDocument(ListMiddleObject_Mapped, pathGeneralHotelInfo);


        }

        public void backgroundWorkerUpdate(int currentValue, int Count)
        {

            bgWorker.ReportProgress(currentValue * 100 / Count);

        }

        public bool createNewLog()
        {
            bool result = false;
            try
            { 
                var pathLog = runmode == 1 ? pathLogGetGeneralInfo : pathLogGetPrice;
            var pathSouce = runmode == 1 ? pathGeneralHotelInfo : pathRoomPrice;
            //---------------Get List Ids was done on result file.

            FileStream stream = File.Open(pathSouce, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            List<LogObject> allObjectDone = new List<LogObject>();
            if (runmode == 1)
            {
                DataSet temp_result = excelReader.AsDataSet();
                IEnumerable<LogObject> Log_done = from row in temp_result.Tables["Table1"].AsEnumerable()
                                                  select new LogObject()
                                                      {
                                                          Ids = Convert.ToString(row["SupplierOstIds"]),
                                                          Done ="1",
                                                          DateCreated = DateTime.Now.ToString()
                                                      };
                allObjectDone = Log_done.ToList();
            }
            else 
                if (runmode == 2)
                {
                    DataSet temp_result = excelReader.AsDataSet();
                    IEnumerable<LogObject> Log_done = from row in temp_result.Tables["Table1"].AsEnumerable()
                                                      select new LogObject()
                                                      {
                                                          Ids = Convert.ToString(row["Ids"]),
                                                          Done = "1",
                                                          DateCreated = DateTime.Now.ToString()
                                                      };
                    allObjectDone = Log_done.ToList();
                }
                // --Get List Objlog from OLDlog
                if (!startNew)
                {
                    FileStream stream2 = File.Open(pathLog, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader2 = ExcelReaderFactory.CreateOpenXmlReader(stream2);
                    excelReader2.IsFirstRowAsColumnNames = true;
                    List<LogObject> OldLogObj= new List<LogObject>();
                    DataSet olDataSet = excelReader2.AsDataSet();
                    IEnumerable<LogObject> oldLogs = from row in olDataSet.Tables["Table1"].AsEnumerable()
                                                      select new LogObject()
                                                      {
                                                          Ids = Convert.ToString(row["Ids"]),
                                                          Done = Convert.ToString(row["Done"]),
                                                          DateCreated = Convert.ToString(row["DateCreated"])
                                                      };
                    var temOldLog = oldLogs.Where(d => d.Done == "1");
                    allObjectDone.AddRange(temOldLog);

                }
            
            //---------------------Create new Emtpy Log base allids
                List<LogObject> allEmptyLog = new List<LogObject>();
                MessageBox.Show(List_Ids.Count().ToString());
                foreach (var item in List_Ids)
                {
                    allEmptyLog.Add(new LogObject()
                    {
                        Ids=item
                    });
                }

                var test = allObjectDone.Count();
                var testvalue = allObjectDone;
                List<LogObject> allEmptyLog_Mapped = new List<LogObject>();
             //--collect all ids in objecdone:
                var allIdsinObjecDone = allObjectDone.Select(c=>c.Ids);

                foreach (var itemEmptyLog in allEmptyLog)
                {
                    var temp_ids = itemEmptyLog.Ids;
                    if (allIdsinObjecDone.Where(m => m == temp_ids).Count() > 0)
                    {
                        
                        var tempMatchedObject = allObjectDone.Where(c => c.Ids == temp_ids).FirstOrDefault();
                        if (tempMatchedObject != null)
                        {
                            itemEmptyLog.InjectFrom<IgnoreNulls>(tempMatchedObject);
                            allEmptyLog_Mapped.Add(itemEmptyLog);
                        }
                        
                    }
                    else
                    {
                        allEmptyLog_Mapped.Add(itemEmptyLog);
                    }
                }
                //----------------Build list of ObjectLog

                if (allEmptyLog_Mapped.Count() > 0)
                {
                    Begodi.CreateExcelFile.CreateExcelDocument(allEmptyLog_Mapped, pathLog);
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
            Trace.WriteLine(" Downloading: " + textValue);
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

            Trace.WriteLine(" Downloading: "+textValue);
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

        public List<string> ListIdsCreator(int type)
        {
            var result = new List<string>();
            //---1 -Convert from COUNTRY LIST
            if (type == 1)
            {
                //Get Ids from Queries ~ startNew==true
                var listCountrySelected = countrylist.CheckedItems.OfType<string>().ToList();
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

            }
            else
            {
                var pathLog = runmode == 1 ? pathLogGetGeneralInfo : pathLogGetPrice;
               // var pathSouce = runmode == 1 ? pathGeneralHotelInfo : pathRoomPrice;
                //---------------Get List Ids was done on result file.

                FileStream stream = File.Open(pathLog, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                List<LogObject> allLog = new List<LogObject>();
               
                    DataSet temp_result = excelReader.AsDataSet();
                    IEnumerable<LogObject> Log_done = from row in temp_result.Tables["Table1"].AsEnumerable()
                                                      select new LogObject()
                                                      {
                                                          Ids = Convert.ToString(row["Ids"]),
                                                          Done = Convert.ToString(row["Done"]),
                                                          DateCreated = Convert.ToString(row["DateCreated"])
                                                      };
                    allLog = Log_done.ToList();
                result = allLog.Where(m=>m.Done!="1").Select(c=>c.Ids).ToList();
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
      

        private void countrylist_MouseClick(object sender, MouseEventArgs e)
        {
            var selecteditem=countrylist.SelectedItem;
            
            /*MessageBox.Show(selecteditem.ToString());*/
            /*countrylist.SetItemChecked(countrylist.SetItemChecked);*/
        }
  

        private void dt_Fromdate_ValueChanged(object sender, EventArgs e)
        {
            dt_Todate.Value = dt_Todate.Value.AddDays(25);
            dt_Todate.Enabled = false;
        }
        private void rd_SavePrice_CheckedChanged(object sender, EventArgs e)
        {
            if (rd_SavePrice.Checked)
            {
                choosen_SaveType = 2;
            }
        }
        private void rd_GetGeneralInfo_CheckedChanged(object sender, EventArgs e)
            {
                if (rd_GetGeneralInfo.Checked)
                {
                    runmode = 1;
                }
            }

            private void rd_getPrice_CheckedChanged(object sender, EventArgs e)
            {
                if (rd_getPrice.Checked)
                {
                    runmode = 2;
                }
            }

            private void rd_Auto_CheckedChanged(object sender, EventArgs e)
            {
                if (rd_Auto.Checked)
                {
                    runmode = 0;
                }
            }

#endregion

    
    
       

      
    }
    
}

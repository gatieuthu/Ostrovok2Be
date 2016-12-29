using System;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;


using System.Collections.Generic;

using System.Reflection;
namespace Begodi
{
	public class IO
	{
		public static string ReadNewsletterTemplate(string path)
		{
			// Get a StreamReader class that can be used to read the file
            StreamReader objStreamReader = System.IO.File.OpenText(path);
			// Now, read the entire file into a string
			string contents = objStreamReader.ReadToEnd();

			objStreamReader.Close();

			return contents;
		}
       
		public static void XCopy(string SourceFolder, string DestinationFolder)
		{
			Process processCMD = new Process();
			processCMD.StartInfo.UseShellExecute = false;
			processCMD.StartInfo.RedirectStandardOutput = true;
			//processCMD.StartInfo.WorkingDirectory = Request.PhysicalApplicationPath + @"bin\";
			string strCommand = "xcopy \"" + SourceFolder + "\" \"" + DestinationFolder + "\"  /e /y /r /k /o /h /i";
			processCMD.StartInfo.FileName = strCommand;
			//processCMD.StartInfo.Arguments = "";
			processCMD.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processCMD.Start();

		}

		public static void CopyDirectory(string sourcePath, string destPath, bool overwrite)
		{
			System.IO.DirectoryInfo sourceDir = new System.IO.DirectoryInfo(sourcePath);
			System.IO.DirectoryInfo destDir = new System.IO.DirectoryInfo(destPath);
			if (sourceDir.Exists)
			{
				if (!destDir.Exists)
					destDir.Create();
				foreach (System.IO.FileInfo file in sourceDir.GetFiles())
				{
					if (overwrite)
						file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), true);
					else if ((System.IO.File.Exists(System.IO.Path.Combine(destDir.FullName, file.Name))) == false)
						file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), false);
				}
				foreach (System.IO.DirectoryInfo dir in sourceDir.GetDirectories())
				{
					CopyDirectory(dir.FullName, System.IO.Path.Combine(destDir.FullName, dir.Name), overwrite);
				}

			}
			else
			{

			}
		}

		public static void CopyFile(string SourceFile, string DestinationFile)
		{
			System.IO.File.Copy(SourceFile, DestinationFile, true);
		}

		public static void CopyFile(string SourceFile, string DestinationFile, bool OverWrite)
		{
			System.IO.File.Copy(SourceFile, DestinationFile, OverWrite);
		}
		public static string ReadFileText(string FileName)
		{
			string contents = "";
			try
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader(FileName))
				{
					String line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{

						contents = contents + line + ";";
					}
				}
			}
			catch { }
			return contents;
		}
		public static Hashtable ReadFileToHashtable(string FileName)
		{
			Hashtable hash = new Hashtable();
			try
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader(FileName))
				{
					String line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						hash.Add(line, line);
					}
				}
			}
			catch { }
			return hash;
		}

		public static DataTable ReadFileToDataTable(string FileName)
		{
			DataTable dt = new DataTable("Table1");
			// Create columns in the DataTable
			dt.Columns.Add("Column1", Type.GetType("System.String"));
			// Add rows of data to the DataTable
			DataRow dataRow;
			try
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader(FileName))
				{
					String line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						dataRow = dt.NewRow(); // Create a new row of data associateed with the DataTable.
						dataRow["Column1"] = line; // Set the columns of data within the DataTable.
						dt.Rows.Add(dataRow); // Add the row of data to the DataTable.
					}
				}
			}
			catch { }
			return dt;
		}

		public static String ReadFile(string strFile)
		{
			// Open the stream and read it back.
			String strRet;
			if (System.IO.File.Exists(strFile))
			{
				StreamReader sr = System.IO.File.OpenText(strFile);
				strRet = sr.ReadToEnd();
				sr.Close();
			}
			else
				strRet = String.Empty;

			return strRet;
		}

		public static void WriteFile(string strFile, string strContent)
		{
			StreamWriter sw = new StreamWriter(strFile);
			// Add some text to the file.
			sw.Write(strContent);
			sw.Close();
		}

		public static void AppendFile(string strFile, string strContent)
		{
			StreamWriter sw = new StreamWriter(strFile, true);
			// Add some text to the file.
			sw.Write(strContent);
			sw.Close();
		}

		public static OleDbDataReader ReadExcelToDataReader(string excelFile)
		{
			string sql = "select * from [Sheet1$]";
			string dataSource = excelFile;
			string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + dataSource + ";" + "Extended Properties=Excel 8.0;";
			OleDbConnection conn = new OleDbConnection(connectionString);
			conn.Open();
			//DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, null });
			OleDbCommand cmd = new OleDbCommand(sql, conn);
			OleDbDataReader dr = cmd.ExecuteReader();
			dr.Close();
			conn.Close();
			return dr;

		}

		public static DataTable ReadExcelToDataTable(string excelFile)
		{
			return ReadExcelToDataTable(excelFile, "Sheet1");
		}

		public static DataTable ReadExcelToDataTable(string excelFile, string sheetName)
		{
			return ReadExcelToDataTable(excelFile, sheetName, 0);
		}

		public static DataTable ReadExcelToDataTable(string excelFile, string sheetName, int SkippingLines)
		{
			string sql = "select * from [" + sheetName + "$]";
			string dataSource = excelFile;
			string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + dataSource + ";" + "Extended Properties=Excel 8.0;";

			OleDbConnection _oleConn = new OleDbConnection(connectionString);
			_oleConn.Open();
			OleDbCommand _oleCmdSelect = new OleDbCommand(sql, _oleConn);
			OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
			oleAdapter.SelectCommand = _oleCmdSelect;
			DataTable dt = new DataTable();
			oleAdapter.FillSchema(dt, SchemaType.Source);
			oleAdapter.Fill(dt);
			oleAdapter.Dispose();
			_oleConn.Close();
			_oleConn.Dispose();
			if (SkippingLines > 0 && dt.Rows.Count >= SkippingLines)
			{
				for (int i = 0; i < SkippingLines; i++)
				{
					dt.Rows[0].Delete();
				}
			}
			return dt;
		}



		/*
		protected string[] COLUMN_NAMES = {
											  "HotelID", "HotelName"
											  , "ValidFromDate", "ValidToDate"
											  , "HotelRoomTypeName"
											  , "SingleCost", "DoubleCost","ExtraBedCost"
											  , "Roh"
											  , "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"
											  , "SupplierCode", "TaxIncludedInCost", "BreakfastIncluded","NoteofPrice"
										  };

		protected string PATTERN = String.Empty; // (?<=,)\s*(?=,)|^(?=,)|[^\"]{2,}(?=\")|([^,\"]+(?=,|$))
		
		
		HotelID,HotelName,ValidFromDate,ValidToDate,HotelRoomTypeName,SingleCost,DoubleCost,ExtraBedCost,Roh,Sun,Mon,Tue,Wed,Thu,Fri,Sat,SupplierCode,TaxIncludedInCost,BreakfastIncluded
		675,"Hanoi Sofitel ,Metropole",1/11/2005,30/9/2006,Premium,132,147,30,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/11/2005,30/9/2006,Classic,162,177,,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/11/2005,30/9/2006,Classic Deluxe,197,212,30,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/11/2005,30/9/2006,Classic Suite,302,317,30,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/10/2006,31/10/2006,Premium,162,162,30,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/10/2006,31/10/2006,Classic,192,192,,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/10/2006,31/10/2006,Classic Deluxe,227,227,30,0,1,1,1,1,1,1,1,INDHAN,1,1
		675,Hanoi Sofitel Metropole,1/10/2006,31/10/2006,Classic Suite,332,332,30,0,1,1,1,1,1,1,1,INDHAN,1,1
		*/
		public static DataTable RegexToDataTable(string source, string pattern, string[] columnNames)
		{
			DataTable toReturn = new DataTable();
			foreach (string columnName in columnNames)
			{
				toReturn.Columns.Add(columnName);
			}
			Regex regex = new Regex(pattern);
			DataRow dr;
			foreach (string sourceLine in source.Split('\n'))
			{
				int i = 0;
				dr = toReturn.NewRow();
				foreach (Match match in regex.Matches(sourceLine))
				{
					dr[columnNames[i++]] = match.ToString();
				}
				toReturn.Rows.Add(dr);
			}
			return toReturn;
		}
	}
}


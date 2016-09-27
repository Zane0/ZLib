using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace ZLib.Util
{
	/// <summary>
	/// 读取 Excel 文件的方法，主要以 Ado 方式读取
	/// </summary>
	public static class ExcelHelper
	{
		public static DataTable GetDataSetFromExcel(string excelFileName)
		{
			string _connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFileName + ";Extended Properties = Excel 12.0";
			using (OleDbConnection _conn = new OleDbConnection(_connstr))
			{
				var firstTableName = GetFirstTableName(_conn);
				string _cmdText = "select * from [" + firstTableName + "] ";
				using (OleDbDataAdapter _adapter = new OleDbDataAdapter(_cmdText, _conn))
				{
					using (DataSet dt = new DataSet())
					{
						_adapter.Fill(dt, "csv");
						return dt.Tables[0];
					}
				}
			}
		}

		public static DataTable GetDataSetFromExcelOld(string excelFileName)
		{
			string _connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFileName + ";Extended Properties = Excel 8.0";
			using (OleDbConnection _conn = new OleDbConnection(_connstr))
			{
				var firstTableName = GetFirstTableName(_conn);
				string _cmdText = "select * from [" + firstTableName + "] ";
				using (OleDbDataAdapter _adapter = new OleDbDataAdapter(_cmdText, _conn))
				{
					using (DataSet dt = new DataSet())
					{
						_adapter.Fill(dt, "csv");
						return dt.Tables[0];
					}
				}
			}
		}

		public static DataTable GetDataSetFromCSV(string csvFileName)
		{
			try
			{
				using (OleDbConnection _conn = new OleDbConnection())
				{
					string pCsvpath = Path.GetDirectoryName(csvFileName);
					string pCsvname = Path.GetFileName(csvFileName);
					pCsvname = "A股用户2006";

					_conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pCsvpath + ";Extended Properties='Text;FMT=Delimited(,);HDR=YES;IMEX=1';";
					_conn.Open();
					string _cmdText = "select * from [" + pCsvname + "] where 1=1";
					using (OleDbCommand _cmd = new OleDbCommand(_cmdText, _conn))
					{
						using (OleDbDataAdapter _adapter = new OleDbDataAdapter(_cmd))
						{
							DataSet dt = new DataSet();
							_adapter.Fill(dt, "csv");
							return dt.Tables[0];
						}
					}
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string GetFirstTableName(OleDbConnection conn)
		{
			conn.Open();
			var tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { });
			conn.Close();

			if (tables.Rows.Count == 0)
			{ throw new Exception("Excel必须包含一个表"); }
			var firstTableName = tables.Rows[0]["TABLE_NAME"].ToString();
			return firstTableName;
		}
	}
}

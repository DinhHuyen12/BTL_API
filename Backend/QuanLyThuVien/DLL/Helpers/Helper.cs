using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BCrypt.Net;


namespace DAL.Helper
{
	public class DataHelper : IDataHelper
	{
		protected string connectString = @"Server=HYSHR;Database=QuanLyThuVien;Trusted_Connection=True;TrustServerCertificate=True;";
		SqlConnection connection;
		
		public DataHelper()
		{
			connection = new SqlConnection(connectString);
		}
		public DataHelper(string connectStr)
		{
			this.connectString = connectStr;
			connection = new SqlConnection(connectString);
		}

		public bool Open()
		{
			try
			{
				if (connection.State != ConnectionState.Open)
				{
					connection.Open();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Open connection error: " + ex.Message);
				return false;
			}
		}

		public void Close()
		{
			if (connection.State != ConnectionState.Closed)
			{
				connection.Close();
			}
		}

		public DataTable ExecuteReaderWithOutput(string procedureName, Dictionary<string, object> inputParams, List<SqlParameter> outputParams)
		{
			DataTable tb = new DataTable();
			try
			{
				using (SqlCommand cmd = new SqlCommand(procedureName, connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					Open();

					// Thêm input params
					if (inputParams != null)
					{
						foreach (var kv in inputParams)
						{
							cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
						}
					}

					// Thêm output params
					if (outputParams != null)
					{
						foreach (var p in outputParams)
						{
							p.Direction = ParameterDirection.Output;
							cmd.Parameters.Add(p);
						}
					}

					using (var reader = cmd.ExecuteReader())
					{
						tb.Load(reader);
					}

					// output params sẽ được lấy ở caller
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ExecuteReaderWithOutput error: " + ex.Message);
				tb = null;
			}
			finally
			{
				Close();
			}
			return tb;
		}


		// Đọc dữ liệu (SELECT)
		public DataTable ExcuteReader(string procedureName, params object[] param_list)
		{
			DataTable tb = new DataTable();
			try
			{
				using (SqlCommand cmd = new SqlCommand(procedureName, connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					Open();

					int parameterInput = param_list.Length / 2;
					for (int i = 0; i < parameterInput; i++)
					{
						string paramName = Convert.ToString(param_list[i]);
						object paramValue = param_list[i + parameterInput];

						cmd.Parameters.Add(new SqlParameter(paramName, paramValue ?? DBNull.Value));
					}

					using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
					{
						ad.Fill(tb);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ExecuteReader error: " + ex.Message);
				tb = null;
			}
			finally
			{
				Close();
			}
			return tb;
		}

		// Thực thi INSERT/UPDATE/DELETE, có hỗ trợ OUTPUT parameter
		public Dictionary<string, object> ExcuteNonQuery(
	  string procedureName,
	  Dictionary<string, object> inputParams,
	  List<SqlParameter> outputParams = null)
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			try
			{
				using (SqlCommand cmd = new SqlCommand(procedureName, connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					Open();

					// Input params
					if (inputParams != null)
					{
						foreach (var kv in inputParams)
						{
							cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
						}
					}

					// Output params
					if (outputParams != null)
					{
						foreach (var p in outputParams)
						{
							p.Direction = ParameterDirection.Output;
							cmd.Parameters.Add(p);
						}
					}

					// Thực thi và lấy số dòng bị ảnh hưởng
					int rowsAffected = cmd.ExecuteNonQuery();
					result["rowsAffected"] = rowsAffected;

					// Lấy giá trị output parameters
					if (outputParams != null)
					{
						foreach (var p in outputParams)
						{
							result[p.ParameterName] = p.Value;
						}
					}
				}
			}
			catch (Exception ex)
			{
				result["error"] = ex.Message;
			}
			finally
			{
				Close();
			}
			return result;
		}


	}
}


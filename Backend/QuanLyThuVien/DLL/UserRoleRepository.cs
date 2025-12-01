using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helper;
using Model;

namespace DAL
{
	public class UserRoleRepository : IUserRoleRepository
	{
		//private readonly IDataHelper _dHelper;

		private readonly IDatabaseHelper _dbHelper;

		//public UserRoleRepository(IDataHelper dHelper)

		//{
		//	_dHelper = dHelper;
		//}
		public UserRoleRepository(IDatabaseHelper dbHelper)

		{
			_dbHelper = dbHelper;
		}

		//public Dictionary<string, object> UpdateUserRole(UpdateUserRoleRequest request)
		//{
		//	var outputParams = new List<SqlParameter>
		//{
		//	new SqlParameter("@p_error_code", SqlDbType.Int) { Direction = ParameterDirection.Output },
		//	new SqlParameter("@p_error_message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
		//};

		//	var inputParams = new Dictionary<string, object>
		//{
		//	{ "@UserId", request.UserId },
		//	{ "@OldRoleId", request.OldRoleId },
		//	{ "@NewRoleId", request.NewRoleId }
		//};

		//	var result = _dbHelper.ExcuteNonQuery("Pro_UpdateUserRole", inputParams, outputParams);

		//	return new Dictionary<string, object>
		//{
		//	{ "Success", result["@p_error_code"].ToString() == "0" },
		//	{ "ErrorCode", result["@p_error_code"] },
		//	{ "Message", result["@p_error_message"] }
		//};
		//}
		public Dictionary<string, object> UpdateUserRole(UpdateUserRoleRequest request)
		{
			string msgError = "";

			var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
				out msgError,
				"Pro_UpdateUserRole",
				2,  // số lượng output
				"@UserId", request.UserId,
				"@OldRoleId", request.OldRoleId,
				"@NewRoleId", request.NewRoleId,
				"@p_error_code", null,
				"@p_error_message", null
			);

			if (!string.IsNullOrEmpty(msgError))
			{
				return new Dictionary<string, object>
		{
			{ "Success", false },
			{ "ErrorCode", -1 },
			{ "Message", msgError }
		};
			}

			return new Dictionary<string, object>
	{
		{ "Success", result[0].ToString() == "0" },
		{ "ErrorCode", result[0] },
		{ "Message", result[1] }
	};
		}

		public Dictionary<string, object> DeleteUserRole(DeleteUserRoleRequest request)
		{
			string msgError = "";

			var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
				out msgError,
				"Pro_DeleteUserRole",
				2, // số lượng OUTPUT
				"@UserId", request.UserId,
				"@RoleId", request.RoleId,
				"@p_error_code", 0,
				"@p_error_message", ""
			);

			if (!string.IsNullOrEmpty(msgError))
			{
				return new Dictionary<string, object>
		{
			{ "Success", false },
			{ "ErrorCode", -1 },
			{ "Message", msgError }
		};
			}

			return new Dictionary<string, object>
	{
		{ "Success", result[0].ToString() == "0" },
		{ "ErrorCode", result[0] },
		{ "Message", result[1] }
	};
		}
		public Dictionary<string, object> GetUserRoles(int userId)
		{
			string msgError = "";

			var table = _dbHelper.ExecuteSProcedureReturnDataTable(
				out msgError,
				"Pro_GetUserRoles",
				"@UserId", userId,
				"@p_error_code", 0,
				"@p_error_message", ""
			);

			if (!string.IsNullOrEmpty(msgError))
			{
				return new Dictionary<string, object>
		{
			{ "Success", false },
			{ "ErrorCode", -1 },
			{ "Message", msgError }
		};
			}

			if (table == null || table.Rows.Count == 0)
			{
				return new Dictionary<string, object>
		{
			{ "Success", false },
			{ "ErrorCode", 1 },
			{ "Message", "User không tồn tại hoặc không có quyền" }
		};
			}

			var row = table.Rows[0];

			return new Dictionary<string, object>
	{
		{ "Success", true },
		{ "ErrorCode", 0 },
		{ "UserId", row["UserId"] },
		{ "Username", row["Username"] },
		{ "RoleId", row["RoleId"] },
		{ "RoleName", row["RoleName"] },
		{ "Description", row["Description"] }
	};
		}
		public Dictionary<string, object> AssignUserRole(AssignUserRoleRequest request)
		{
			string msgError = "";

			var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
				out msgError,
				"Pro_AssignUserRole",
				2,                      // số lượng OUTPUT param
				"@UserId", request.UserId,
				"@RoleId", request.RoleId,
				"@p_error_code", null,
				"@p_error_message", null
			);

			if (!string.IsNullOrEmpty(msgError))
			{
				return new Dictionary<string, object>
			{
				{ "Success", false },
				{ "ErrorCode", -1 },
				{ "Message", msgError }
			};
			}

			return new Dictionary<string, object>
		{
			{ "Success", result[0].ToString() == "0" },
			{ "ErrorCode", result[0] },
			{ "Message", result[1] }
		};
		}




	}
}

using DAL.Helper;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Models.Requests;

namespace DAL
{
	public class RoleRepository : IRoleRepository
	{
		private readonly IDatabaseHelper _dbHelper;

		public RoleRepository(IDatabaseHelper dbHelper)
		{
			_dbHelper = dbHelper;
		}

		public Dictionary<string, object> AddRole(AddRoleRequest request)
		{
			string msgError = "";

			//var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
			//	out msgError,
			//	"Pro_AddRole",
			//	2, // số lượng OUTPUT param
			//	"@RoleName", request.RoleName,
			//	"@p_error_code", 0,
			//	"@p_error_message", ""
			//);
			var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
				out msgError,
				"Pro_AddRole",
				2,
				"@RoleName", request.RoleName,
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

		public Dictionary<string, object> UpdateRole(UpdateRoleRequest request)
		{
			string msgError = "";

			var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
				out msgError,
				"Pro_UpdateRole",
				2, // số lượng output param
				"@RoleId", request.RoleId,
				"@RoleName", request.RoleName,
				"@p_error_code", "",
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
		public Dictionary<string, object> DeleteRole(int roleId)
		{
			string msgError = "";

			var result = _dbHelper.ReturnValuesFromExecuteSProcedure(
				out msgError,
				"Pro_DeleteRole",
				2,
				"@RoleId", roleId,
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

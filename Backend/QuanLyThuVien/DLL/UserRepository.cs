using DAL.Interfaces;
using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
	public class UserRepository : IUserRepository
	{
		private readonly IDataHelper _dbHelper;

		public UserRepository(IDataHelper dbHelper)
		{
			_dbHelper = dbHelper;
		}

		public Users GetUserByEmail(string email)
		{
			var param = new Dictionary<string, object>
	{
		{ "@Email", email }
	};

			var dt = _dbHelper.ExcuteReader(
				"sp_GetUserByEmail",   // Gọi stored procedure
				"@Email",
				email
			);

			if (dt != null && dt.Rows.Count > 0)
			{
				return MapToUser(dt.Rows[0]);
			}
			return null;
		}



		public void SaveResetToken(string email, string token, DateTime expiry)
		{
			var param = new Dictionary<string, object>
			{
				{ "@Email", email },
				{ "@ResetToken", token },
				{ "@Expiry", expiry }
			};

			_dbHelper.ExcuteNonQuery(
				"sp_SaveResetToken",
				param,
				null
			);
		}

		
		public Users ValidateResetToken(string token)
		{
			var param = new Dictionary<string, object>
			
			{
		{ "@Token", token } // không cần @ vì sẽ được xử lý trong DAL
    };

			var dt = _dbHelper.ExcuteReader(
				"Pro_ValidateResetToken",
				"@Token", token
			);

			if (dt != null && dt.Rows.Count > 0)
			{
				return MapToUser(dt.Rows[0]);
			}
			return null;
		}




		public void UpdatePassword(string email, string passwordHash)
		{
			var param = new Dictionary<string, object>
			{
				{ "@Email", email },
				{ "@HashedPassword", passwordHash }
			};

			_dbHelper.ExcuteNonQuery(
				"sp_UpdatePassword",
				param,
				null
			);
		}

		// Helper: map DataRow -> Users model
		private Users MapToUser(DataRow row)
		{
			return new Users
			{
				UserId = Convert.ToInt32(row["UserId"]),
				Username = row["Username"].ToString(),
				PasswordHash = row["PasswordHash"].ToString(),
				FullName = row["FullName"]?.ToString(),
				Email = row["Email"].ToString(),
				PasswordResetToken = row["PasswordResetToken"]?.ToString(),
				TokenExpiry = row["TokenExpiry"] == DBNull.Value ? null : (DateTime?)row["TokenExpiry"],
				RoleId = Convert.ToInt32(row["RoleId"])
			};
		}
		public bool UpdateUserRole(int userId, string role)
		{
			var inputParams = new Dictionary<string, object>
	{
		{ "@UserId", userId },
		{ "@Role", role }
	};

			var result = _dbHelper.ExcuteNonQuery(
				"sp_update_user_role",    // 👈 phải là stored procedure
				inputParams
			);

			// Kiểm tra xem rowsAffected có tồn tại không
			if (result.ContainsKey("rowsAffected"))
			{
				return Convert.ToInt32(result["rowsAffected"]) > 0;
			}

			return false;
		}
	}
}

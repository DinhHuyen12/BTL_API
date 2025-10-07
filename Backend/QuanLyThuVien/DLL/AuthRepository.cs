using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public partial class AuthRepository : IAuthRepository
    {
        private readonly IDataHelper _dbHelper;

        public AuthRepository(IDataHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public Users Login(string username)
        {
            try
            {
                // input params
                var inputParams = new Dictionary<string, object>
                {
                    { "@username", username }
                };

                // Gọi procedure Pro_Login
                var dt = _dbHelper.ExcuteReader("Pro_Login", inputParams);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new Users
                    {
                        UserId = Convert.ToInt32(row["UserId"]),
                        Username = row["Username"].ToString(),
                        FullName = row["FullName"].ToString(),
                        Email = row["Email"].ToString(),
                        RoleId = Convert.ToInt32(row["RoleId"]),
                        RoleName = row["RoleName"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login error: " + ex.Message);
                return null;
            }
        }

        public Dictionary<string, object> CreateUser(Users user)
        {
            var response = new Dictionary<string, object>
            {
                { "Success", false },
                { "ErrorCode", 0 },
                { "Message", "" }
            };

            try
            {
                var inputParams = new Dictionary<string, object>
                {
                    { "@userName", user.Username },
                    { "@password", user.PasswordHash },
                    { "@fullName", user.FullName },
                    { "@email", user.Email },
                    { "@roleId", user.RoleId }
                };

                var outputParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_error_code", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@p_error_message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
                };

                var result = _dbHelper.ExcuteNonQuery("Pro_Register", inputParams, outputParams);
                foreach (var param in outputParams)
                {
                    Console.WriteLine($"{param.ParameterName} = {param.Value}");
                }

                // Hoặc log toàn bộ dictionary result
                Console.WriteLine("Result dictionary:");
                foreach (var kv in result)
                {
                    Console.WriteLine($"{kv.Key} = {kv.Value}");
                }
                if (result != null && result.ContainsKey("@p_error_code"))
                {
                    int errorCode = Convert.ToInt32(result["@p_error_code"]);
                    string errorMsg = result["@p_error_message"]?.ToString() ?? "";

                    response["ErrorCode"] = errorCode;
                    response["Message"] = errorMsg;

                    if (errorCode == 0)
                    {
                        response["Success"] = true;
                        response["Message"] = "Tạo user thành công";
                    }
                }
            }
            catch (Exception ex)
            {
                response["ErrorCode"] = -1;
                response["Message"] = $"Exception: {ex.Message}";
            }

            return response;
        }

        public Dictionary<string, object> UpdateUser(Users user)
        {
			var response = new Dictionary<string, object>
	        {
		        { "Success", false },
		        { "ErrorCode", 0 },
		        { "Message", "" }
	        };

			try
			{
				var inputParams = new Dictionary<string, object>
		        {
			        { "@userId", user.UserId },       // thêm userId để xác định record cần update
                    { "@userName", user.Username },
			        { "@password", user.PasswordHash },
			        { "@fullName", user.FullName },
			        { "@email", user.Email },
			        { "@roleId", user.RoleId }
		        };

				var outputParams = new List<SqlParameter>
		        {
			        new SqlParameter("@p_error_code", SqlDbType.Int) { Direction = ParameterDirection.Output },
			        new SqlParameter("@p_error_message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
		        };

				var result = _dbHelper.ExcuteNonQuery("Pro_UpdateUser", inputParams, outputParams);

				// Debug output
				foreach (var param in outputParams)
				{
					Console.WriteLine($"{param.ParameterName} = {param.Value}");
				}

				Console.WriteLine("Result dictionary:");
				foreach (var kv in result)
				{
					Console.WriteLine($"{kv.Key} = {kv.Value}");
				}

				if (result != null && result.ContainsKey("@p_error_code"))
				{
					int errorCode = Convert.ToInt32(result["@p_error_code"]);
					string errorMsg = result["@p_error_message"]?.ToString() ?? "";

					response["ErrorCode"] = errorCode;
					response["Message"] = errorMsg;

					if (errorCode == 0)
					{
						response["Success"] = true;
						response["Message"] = "Cập nhật user thành công";
					}
				}
			}
			catch (Exception ex)
			{
				response["ErrorCode"] = -1;
				response["Message"] = $"Exception: {ex.Message}";
			}

			return response;
		}

		public Dictionary<string, object> DeleteUser(int userId)
		{
			var response = new Dictionary<string, object>
			{
				{ "Success", false },
				{ "ErrorCode", 0 },
				{ "Message", "" }
			};

			try
			{
				// Tham số đầu vào
				var inputParams = new Dictionary<string, object>
				{
					{ "@userId", userId }
				};

				// Tham số đầu ra
				var outputParams = new List<SqlParameter>
				{
					new SqlParameter("@p_error_code", SqlDbType.Int) { Direction = ParameterDirection.Output },
					new SqlParameter("@p_error_message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
				};

				var result = _dbHelper.ExcuteNonQuery("Pro_DeleteUser", inputParams, outputParams);

				// Debug output
				foreach (var param in outputParams)
				{
					Console.WriteLine($"{param.ParameterName} = {param.Value}");
				}

				if (result != null && result.ContainsKey("@p_error_code"))
				{
					int errorCode = Convert.ToInt32(result["@p_error_code"]);
					string errorMsg = result["@p_error_message"]?.ToString() ?? "";

					response["ErrorCode"] = errorCode;
					response["Message"] = errorMsg;

					if (errorCode == 0)
					{
						response["Success"] = true;
						response["Message"] = "Xóa user thành công";
					}
				}
			}
			catch (Exception ex)
			{
				response["ErrorCode"] = -1;
				response["Message"] = $"Exception: {ex.Message}";
			}

			return response;
		}

		//public Users GetUserByEmail(string email)
		//{
		//	Users user = null;

		//	try
		//	{
		//		// Gọi stored procedure với tham số duy nhất
		//		var dt = _dbHelper.ExcuteReader("Pro_GetUserByEmail", email);

		//		if (dt != null && dt.Rows.Count > 0)
		//		{
		//			var row = dt.Rows[0];
		//			user = new Users
		//			{
		//				UserId = Convert.ToInt32(row["UserId"]),
		//				Username = row["Username"].ToString(),
		//				PasswordHash = row["PasswordHash"].ToString(),
		//				FullName = row["FullName"].ToString(),
		//				Email = row["Email"].ToString(),
		//				RoleId = Convert.ToInt32(row["RoleId"])
		//			};
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine("Lỗi GetUserByEmail: " + ex.Message);
		//	}

		//	return user;
		//}
		//// Lấy token reset mật khẩu
		//public PasswordResetToken GetResetToken(string token)
		//{
		//	try
		//	{
		//		var inputParams = new Dictionary<string, object>
		//	{
		//		{ "@Token", token }
		//	};

		//		var dt = _dbHelper.ExecuteQuery("Pro_GetResetToken", inputParams);

		//		if (dt != null && dt.Rows.Count > 0)
		//		{
		//			var row = dt.Rows[0];
		//			return new PasswordResetToken
		//			{
		//				TokenId = Guid.Parse(row["TokenId"].ToString()),
		//				UserId = Convert.ToInt32(row["UserId"]),
		//				Token = row["Token"].ToString(),
		//				ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]),
		//				IsUsed = Convert.ToBoolean(row["IsUsed"])
		//			};
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine("GetResetToken error: " + ex.Message);
		//	}

		//	return null;
		//}

		//// Forgot Password: sinh token + gửi email
		//public Dictionary<string, object> ForgotPassword(string email)
		//{
		//	var response = new Dictionary<string, object>
		//{
		//	{ "Success", false },
		//	{ "Message", "" }
		//};

		//	try
		//	{
		//		var user = GetUserByEmail(email);
		//		if (user == null)
		//		{
		//			response["Message"] = "Email không tồn tại";
		//			return response;
		//		}

		//		string token = Guid.NewGuid().ToString();
		//		DateTime expiry = DateTime.UtcNow.AddMinutes(15);

		//		var inputParams = new Dictionary<string, object>
		//	{
		//		{ "@UserId", user.UserId },
		//		{ "@Token", token },
		//		{ "@ExpiryDate", expiry }
		//	};

		//		_dbHelper.ExcuteNonQuery("Pro_InsertPasswordResetToken", inputParams, null);

		//		// TODO: gửi email thật qua SMTP
		//		string resetLink = $"https://huyendinh6122001@gmail.com/reset-password?token={{token}}";
		//		Console.WriteLine("Reset link: " + resetLink);

		//		response["Success"] = true;
		//		response["Message"] = "Đã gửi email reset mật khẩu";
		//	}
		//	catch (Exception ex)
		//	{
		//		response["Message"] = $"Exception: {ex.Message}";
		//	}

		//	return response;
		//}

		//// Reset Password
		//public Dictionary<string, object> ResetPassword(string token, string newPasswordHash)
		//{
		//	var response = new Dictionary<string, object>
		//{
		//	{ "Success", false },
		//	{ "Message", "" }
		//};

		//	try
		//	{
		//		var resetToken = GetResetToken(token);

		//		if (resetToken == null || resetToken.IsUsed || resetToken.ExpiryDate < DateTime.UtcNow)
		//		{
		//			response["Message"] = "Token không hợp lệ hoặc đã hết hạn";
		//			return response;
		//		}

		//		// Update mật khẩu user
		//		var inputParams = new Dictionary<string, object>
		//	{
		//		{ "@UserId", resetToken.UserId },
		//		{ "@PasswordHash", newPasswordHash }
		//	};
		//		_dbHelper.ExcuteNonQuery("Pro_UpdatePassword", inputParams, null);

		//		// Đánh dấu token đã dùng
		//		var updateParams = new Dictionary<string, object>
		//	{
		//		{ "@TokenId", resetToken.TokenId }
		//	};
		//		_dbHelper.ExcuteNonQuery("Pro_MarkTokenUsed", updateParams, null);

		//		response["Success"] = true;
		//		response["Message"] = "Đặt lại mật khẩu thành công";
		//	}
		//	catch (Exception ex)
		//	{
		//		response["Message"] = $"Exception: {ex.Message}";
		//	}

		//	return response;
		//}

	}
}

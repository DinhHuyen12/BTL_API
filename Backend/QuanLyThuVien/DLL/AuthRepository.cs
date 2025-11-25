using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MailKit.Net.Smtp;
namespace DAL
{
	public partial class AuthRepository : IAuthRepository
	{
		private readonly IDataHelper _dbHelper;

		private readonly string jwtSecret = "ThuVien2025_2025_SecretKey123!@#456-dfdfwer"; // đổi thành key bảo mật mạnh
		private readonly int jwtLifespan = 60; // token sống 60 phút

		public AuthRepository(IDataHelper dbHelper)
		{
			_dbHelper = dbHelper;
		}
		public string GenerateJwtToken(Users user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(jwtSecret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("UserId", user.UserId.ToString()),
					new Claim("Username", user.Username),
					new Claim("FullName", user.FullName)
				}),
				Expires = DateTime.UtcNow.AddMinutes(jwtLifespan),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public Users Login(string username)
		{
			try
			{
				var inputParams = new Dictionary<string, object>
				{
					["@username"] = username
				};

				var outputParams = new List<SqlParameter>
		{
			new SqlParameter("@p_error_code", SqlDbType.Int),
			new SqlParameter("@p_error_message", SqlDbType.NVarChar, 255)
		};

				var dt = _dbHelper.ExecuteReaderWithOutput("Pro_Login1", inputParams, outputParams);

				int errorCode = Convert.ToInt32(outputParams[0].Value ?? 0);
				string errorMessage = outputParams[1].Value?.ToString();

				Console.WriteLine($"ErrorCode: {errorCode}, Message: {errorMessage}");

				if (errorCode != 0)
				{
					Console.WriteLine("Login failed: " + errorMessage);
					return null;
				}



				if (dt != null && dt.Rows.Count > 0)
				{
					DataRow row = dt.Rows[0];
					// Tạo object user trước
					var user = new Users
					{
						UserId = Convert.ToInt32(row["UserId"]),
						Username = row["Username"].ToString(),
						PasswordHash = row["PasswordHash"].ToString(),
						FullName = row["FullName"].ToString(),
						Email = row["Email"].ToString(), // ✅ thêm dòng này

					};

					// Tạo JWT sau khi có user
					user.token = GenerateJwtToken(user);

					return user;
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


		public bool GenerateTwoFactorCode(string email)
		{
			try
			{
				string otp = new Random().Next(100000, 999999).ToString();
				DateTime expiry = DateTime.Now.AddMinutes(5);

				var inputParams = new Dictionary<string, object>
		{
			{ "@Email", email },
			{ "@Code", otp },
			{ "@Expiry", expiry }
		};

				_dbHelper.ExcuteNonQuery("Pro_SaveTwoFactorCode", inputParams, null);

				// Gửi mail OTP
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("Shop ThienThanNho", "contact.dev24h@gmail.com"));
				message.To.Add(new MailboxAddress("", email));
				message.Subject = "Mã xác minh đăng nhập (2FA)";

				string htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Segoe UI', sans-serif;
            background-color: #f7f8fa;
            color: #333;
            padding: 30px;
        }}
        .container {{
            max-width: 500px;
            margin: auto;
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
            padding: 30px;
            text-align: center;
        }}
        .logo {{
            font-size: 22px;
            font-weight: bold;
            color: #4a90e2;
        }}
        .otp-code {{
            font-size: 32px;
            font-weight: bold;
            color: #ff6600;
            letter-spacing: 3px;
            margin: 20px 0;
        }}
        .footer {{
            font-size: 13px;
            color: #888;
            margin-top: 30px;
        }}
        .button {{
            display: inline-block;
            background: #4a90e2;
            color: white;
            padding: 10px 20px;
            border-radius: 5px;
            text-decoration: none;
            margin-top: 15px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='logo'>📚 THƯ VIỆN ONLINE</div>
        <p>Xin chào,</p>
        <p>Bạn vừa yêu cầu đăng nhập vào hệ thống. Đây là mã OTP xác minh của bạn:</p>
        <div class='otp-code'>{otp}</div>
        <p>Mã này có hiệu lực trong <strong>5 phút</strong>.</p>
        <a class='button' href='#'>Xác nhận ngay</a>
        <div class='footer'>
            Nếu bạn không yêu cầu thao tác này, vui lòng bỏ qua email này.<br/>
            &copy; {DateTime.Now.Year} Thư viện Online. All rights reserved.
        </div>
    </div>
</body>
</html>
";

				message.Body = new TextPart("html") { Text = htmlBody };
				using var client = new SmtpClient();
				client.Connect("smtp.gmail.com", 587, false);
				client.Authenticate("contact.dev24h@gmail.com", "jcrx xzfh ahpe oisk");
				client.Send(message);
				client.Disconnect(true);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("2FA error: " + ex.Message);
				return false;
			}
		}
		//public Users GetUserByEmail(string email)
		//{
		//	try
		//	{
		//		var inputParams = new Dictionary<string, object>
		//{
		//	{ "@Email", email }
		//};

		//		// Gọi stored procedure
		//		var dt = _dbHelper.ExcuteReader("Pro_GetUserInfoByEmail", inputParams);

		//		if (dt != null && dt.Rows.Count > 0)
		//		{
		//			DataRow row = dt.Rows[0];
		//			return new Users
		//			{
		//				UserId = Convert.ToInt32(row["UserId"]),
		//				Username = row["Username"].ToString(),
		//				FullName = row["FullName"].ToString(),
		//				Email = row["Email"].ToString(),
		//				RoleId = Convert.ToInt32(row["RoleId"]),
		//				RoleName = row.Table.Columns.Contains("RoleName") ? row["RoleName"].ToString() : null
		//			};
		//		}

		//		return null;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine("GetUserByEmail error: " + ex.Message);
		//		return null;
		//	}
		//}
		public Users GetUserByEmail(string email)
		{
			try
			{
				var inputParams = new Dictionary<string, object>
		{
			{ "@Email", email }
		};

				var dt = _dbHelper.ExecuteReaderWithOutput(
					"Pro_GetUserInfoByEmail",
					inputParams,
					null
				);

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
						RoleName = row.Table.Columns.Contains("RoleName") ? row["RoleName"].ToString() : null
					};
				}

				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine("GetUserByEmail error: " + ex.Message);
				return null;
			}
		}



		public bool VerifyTwoFactorCode(string email, string code)
		{
			try
			{
				var inputParams = new Dictionary<string, object>
		{
			{ "@Email", email },
			{ "@Code", code }
		};

				// Không có output parameters nên truyền list rỗng
				var outputParams = new List<SqlParameter>();

				// Gọi stored procedure với đúng 3 tham số
				var dt = _dbHelper.ExecuteReaderWithOutput("Pro_VerifyTwoFactorCode", inputParams, outputParams);

				// Kiểm tra dữ liệu trả về
				if (dt != null && dt.Rows.Count > 0)
				{
					var expiry = Convert.ToDateTime(dt.Rows[0]["TwoFactorExpiry"]);
					return expiry > DateTime.Now; // OTP còn hạn thì true
				}

				return false; // Không có record => sai hoặc hết hạn
			}
			catch (Exception ex)
			{
				Console.WriteLine("VerifyTwoFactorCode error: " + ex.Message);
				return false;
			}
		}
	}
	
}

using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;

namespace QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBusiness _authBusiness;

        public AuthController(IAuthBusiness authBusiness)
        {
            _authBusiness = authBusiness;
        }

		///// <summary>
		///// Đăng nhập
		///// </summary>
		//[AllowAnonymous]
		//[HttpPost("login")]
		//public IActionResult Login([FromBody] Dictionary<string, string> parameters)
		//{
		//	if (parameters == null ||
		//		!parameters.ContainsKey("username") ||
		//		!parameters.ContainsKey("password") ||
		//		string.IsNullOrEmpty(parameters["username"]) ||
		//		string.IsNullOrEmpty(parameters["password"]))
		//	{
		//		return BadRequest(new { message = "Thiếu username hoặc password" });
		//	}

		//	string username = parameters["username"];
		//	string password = parameters["password"];

		//	// Trả về kiểu Users
		//	var user = _authBusiness.Login(username, password);

		//	if (user == null)
		//	{
		//		return Unauthorized(new { message = "Sai username hoặc password" });
		//	}

		//	return Ok(new
		//	{
		//		message = "Đăng nhập thành công",
		//		data = user
		//	});
		//}

		/// <summary>
		/// Đăng nhập (Bước 1) – Kiểm tra tài khoản và gửi mã OTP đến email
		/// </summary>
		[AllowAnonymous]
		[HttpPost("login")]
		public IActionResult Login([FromBody] Dictionary<string, string> parameters)
		{
			if (parameters == null ||
				!parameters.ContainsKey("username") ||
				!parameters.ContainsKey("password") ||
				string.IsNullOrEmpty(parameters["username"]) ||
				string.IsNullOrEmpty(parameters["password"]))
			{
				return BadRequest(new { message = "Thiếu username hoặc password" });
			}

			string username = parameters["username"];
			string password = parameters["password"];

			// Lấy user trong DB
			var user = _authBusiness.Login(username, password);

			if (user == null)
			{
				Console.WriteLine($"[LOGIN FAIL] Username={username}, Password={password}");
				return Unauthorized(new { message = "Sai username hoặc password" });
			}

			// ✅ Gửi mã OTP tới email user
			bool sent = _authBusiness.GenerateTwoFactorCode(user.Email);
			if (!sent)
				return StatusCode(500, new { success = false, message = "Không thể gửi mã OTP. Vui lòng thử lại." });

			Console.WriteLine($"[LOGIN SUCCESS] Gửi OTP tới: {user.Email}");

			return Ok(new
			{
				success = true,
				message = "Đăng nhập bước 1 thành công. OTP đã được gửi đến email.",
				email = user.Email
			});
		}


		/// <summary>
		/// Bước 2: Xác minh mã OTP
		/// </summary>
		[AllowAnonymous]
		[HttpPost("verify-otp")]
		public IActionResult VerifyOtp([FromBody] Dictionary<string, string> parameters)
		{
			if (parameters == null ||
				!parameters.ContainsKey("email") ||
				!parameters.ContainsKey("code"))
			{
				return BadRequest(new { success = false, message = "Thiếu email hoặc mã OTP" });
			}

			string email = parameters["email"];
			string code = parameters["code"];

			bool verified = _authBusiness.VerifyTwoFactorCode(email, code);
			if (!verified)
				return BadRequest(new { success = false, message = "Mã OTP không hợp lệ hoặc đã hết hạn." });

			// Khi OTP hợp lệ → trả JWT token cho frontend lưu
			var user = _authBusiness.GetUserByEmail(email);
			if (user == null)
				return NotFound(new { success = false, message = "Không tìm thấy người dùng." });

			string token = _authBusiness.GenerateJwtToken(user);

			return Ok(new
			{
				success = true,
				message = "Xác minh OTP thành công",
				token,
				user
			});
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("update-role")]
		public IActionResult UpdateRole([FromBody] Dictionary<string, string> payload)
		{
			if (!payload.ContainsKey("userId") || !payload.ContainsKey("role"))
				return BadRequest(new { message = "Thiếu userId hoặc role" });

			int userId = int.Parse(payload["userId"]);
			string role = payload["role"];

			bool ok = _authBusiness.UpdateUserRole(userId, role);

			if (!ok)
				return BadRequest(new { success = false, message = "Cập nhật role thất bại" });

			return Ok(new
			{
				success = true,
				message = "Cập nhật role thành công",
				data = new { userId, role }
			});
		}
		/// <summary>
		/// Đăng ký user mới
		/// </summary>
		/// 
		[AllowAnonymous]
		[HttpPost("register")]
        public IActionResult Register([FromBody] Users user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.PasswordHash))
                return BadRequest(new { success = false, message = "Thông tin user không hợp lệ" });

            var result = _authBusiness.CreateUser(user);
            Console.WriteLine(result);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "Có lỗi khi tạo user (result null)" });

            // Lấy error code
            int errorCode = 0;
            string errorMessage = string.Empty;

            if (result.ContainsKey("ErrorCode"))
                errorCode = Convert.ToInt32(result["ErrorCode"]);

            if (result.ContainsKey("Message"))
                errorMessage = result["Message"]?.ToString();

            if (errorCode != 0)
            {
                return BadRequest(new
                {
					success = false,
					message = "Tạo user thất bại",
                    errorCode,
                    errorMessage
                });
            }

            return Ok(new
			{
				success = true,
				message = "Tạo user thành công",
                data = result
            });
        }
		/// <summary>
		/// Cập nhật thông tin user
		/// </summary>
		[HttpPut("update-user")]
		public IActionResult UpdateUser([FromBody] Users user)
		{
			if (user == null || user.UserId <= 0)
				return BadRequest(new { success = false, message = "Thông tin user không hợp lệ" });

			var result = _authBusiness.UpdateUser(user);
			Console.WriteLine(result);

			if (result == null)
				return StatusCode(StatusCodes.Status500InternalServerError,
					new { success = false, message = "Có lỗi khi cập nhật user (result null)" });

			// Lấy error code
			int errorCode = 0;
			string errorMessage = string.Empty;

			if (result.ContainsKey("ErrorCode"))
				errorCode = Convert.ToInt32(result["ErrorCode"]);

			if (result.ContainsKey("Message"))
				errorMessage = result["Message"]?.ToString();

			if (errorCode != 0)
			{
				return BadRequest(new
				{
					success = false,
					message = "Cập nhật user thất bại",
					errorCode,
					errorMessage
				});
			}

			return Ok(new
			{
				success = true,

				message = "Cập nhật user thành công",
				data = result
			});
		}

		/// <summary>
		/// Xóa user theo Id
		/// </summary>
		[HttpDelete("delete-user/{userId}")]
		public IActionResult DeleteUser(int userId)
		{
			if (userId <= 0)
				return BadRequest(new { success = false, message = "UserId không hợp lệ" });

			var result = _authBusiness.DeleteUser(userId);

			if (result == null)
				return StatusCode(StatusCodes.Status500InternalServerError,
					new { success = false, message = "Có lỗi khi xóa user (result null)" });

			int errorCode = 0;
			string errorMessage = string.Empty;

			if (result.ContainsKey("ErrorCode"))
				errorCode = Convert.ToInt32(result["ErrorCode"]);

			if (result.ContainsKey("Message"))
				errorMessage = result["Message"]?.ToString();

			if (errorCode != 0)
			{
				return BadRequest(new
				{
					success = false,
					message = "Xóa user thất bại",
					errorCode,
					errorMessage
				});
			}

			return Ok(new
			{
				success = true,
				message = "Xóa user thành công",
				data = result
			});
		}
		// ==========================================================
		// 6. VALIDATE TOKEN – KIỂM TRA TOKEN CÒN HẠN KHÔNG
		// ==========================================================
		[Authorize]
		[HttpGet("validate-token")]
		public IActionResult ValidateToken()
		{
			string authHeader = Request.Headers["Authorization"];

			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
				return Unauthorized(new { success = false, message = "Token không tồn tại" });

			string token = authHeader.Substring("Bearer ".Length).Trim();

			var user = _authBusiness.ValidateJwtToken(token);

			if (user == null)
				return Unauthorized(new { success = false, message = "Token không hợp lệ hoặc đã hết hạn" });

			return Ok(new
			{
				success = true,
				message = "Token hợp lệ",
				user
			});
		}
		/// <summary>
		/// Lấy danh sách toàn bộ user
		/// </summary>
		[HttpGet("get-all-users")]
		public IActionResult GetAllUsers()
		{
			var users = _authBusiness.GetAllUsers();

			if (users == null || users.Count == 0)
				return NotFound(new { success = false, message = "Không có người dùng nào." });

			return Ok(new
			{
				success = true,
				message = "Lấy danh sách người dùng thành công",
				data = users
			});
		}

	


	}
}

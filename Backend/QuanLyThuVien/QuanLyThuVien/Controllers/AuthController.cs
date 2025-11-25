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
				return StatusCode(500, new { message = "Không thể gửi mã OTP. Vui lòng thử lại." });

			Console.WriteLine($"[LOGIN SUCCESS] Gửi OTP tới: {user.Email}");

			return Ok(new
			{
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
				return BadRequest(new { message = "Thiếu email hoặc mã OTP" });
			}

			string email = parameters["email"];
			string code = parameters["code"];

			bool verified = _authBusiness.VerifyTwoFactorCode(email, code);
			if (!verified)
				return BadRequest(new { message = "Mã OTP không hợp lệ hoặc đã hết hạn." });

			// Khi OTP hợp lệ → trả JWT token cho frontend lưu
			var user = _authBusiness.GetUserByEmail(email);
			if (user == null)
				return NotFound(new { message = "Không tìm thấy người dùng." });

			string token = _authBusiness.GenerateJwtToken(user);

			return Ok(new
			{
				message = "Xác minh OTP thành công",
				token,
				user
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
                return BadRequest(new { message = "Thông tin user không hợp lệ" });

            var result = _authBusiness.CreateUser(user);
            Console.WriteLine(result);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Có lỗi khi tạo user (result null)" });

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
                    message = "Tạo user thất bại",
                    errorCode,
                    errorMessage
                });
            }

            return Ok(new
            {
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
				return BadRequest(new { message = "Thông tin user không hợp lệ" });

			var result = _authBusiness.UpdateUser(user);
			Console.WriteLine(result);

			if (result == null)
				return StatusCode(StatusCodes.Status500InternalServerError,
					new { message = "Có lỗi khi cập nhật user (result null)" });

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
					message = "Cập nhật user thất bại",
					errorCode,
					errorMessage
				});
			}

			return Ok(new
			{
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
				return BadRequest(new { message = "UserId không hợp lệ" });

			var result = _authBusiness.DeleteUser(userId);

			if (result == null)
				return StatusCode(StatusCodes.Status500InternalServerError,
					new { message = "Có lỗi khi xóa user (result null)" });

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
					message = "Xóa user thất bại",
					errorCode,
					errorMessage
				});
			}

			return Ok(new
			{
				message = "Xóa user thành công",
				data = result
			});
		}



	}
}

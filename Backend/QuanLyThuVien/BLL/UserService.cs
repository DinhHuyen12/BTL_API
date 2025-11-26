using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

namespace BLL
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository repo)
		{
			_userRepository = repo;
		}

		public Dictionary<string, object> ForgotPassword(string email)
		{
			var response = new Dictionary<string, object>
			{
				{ "Success", false },
				{ "Message", "" }
			};

			var user = _userRepository.GetUserByEmail(email);
			if (user == null)
			{
				response["Message"] = "Email không tồn tại";
				return response;
			}

			string token = Guid.NewGuid().ToString();
			DateTime expiry = DateTime.Now.AddMinutes(15);

			_userRepository.SaveResetToken(email, token, expiry);

			// Gửi mail
			SendResetEmail(email, token);

			response["Success"] = true;
			response["Message"] = "Đã gửi email reset mật khẩu";
			return response;
		}

		public Dictionary<string, object> ResetPassword(string token, string newPassword)
		{
			var response = new Dictionary<string, object>
			{
				{ "Success", false },
				{ "Message", "" }
			};

			var user = _userRepository.ValidateResetToken(token);
			if (user == null)
			{
				response["Message"] = "Token không hợp lệ hoặc đã hết hạn";
				return response;
			}

			string hashed = HashPassword(newPassword);
			_userRepository.UpdatePassword(user.Email, hashed);

			response["Success"] = true;
			response["Message"] = "Đặt lại mật khẩu thành công";
			return response;
		}

		private string HashPassword(string password)
		{
			using var sha256 = SHA256.Create();
			var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(bytes);
		}

		//private void SendResetEmail(string email, string token)
		//{
		//	var message = new MimeMessage();
		//	message.From.Add(new MailboxAddress("Admin", "youremail@gmail.com"));
		//	message.To.Add(new MailboxAddress("", email));
		//	message.Subject = "Reset mật khẩu";
		//	message.Body = new TextPart("plain")
		//	{
		//		Text = $"Nhấn vào link để reset mật khẩu: https://yourdomain.com/reset?token={token}"
		//	};

		//	using var client = new SmtpClient();
		//	client.Connect("smtp.gmail.com", 587, false);
		//	client.Authenticate("contact.dev24h@gmail.com", "jcrx xzfh ahpe oisk"); // App password Gmail
		//	client.Send(message);
		//	client.Disconnect(true);
		//}
		private void SendResetEmail(string email, string token)
		{
			var resetLink = $"http://127.0.0.1:5500/reset-password.html?token={token}";

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Shop ThienThanNho", "contact.dev24h@gmail.com"));
			message.To.Add(new MailboxAddress("", email));
			message.Subject = "Yêu cầu đặt lại mật khẩu";

			// HTML template
			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;'>
            <table align='center' cellpadding='0' cellspacing='0' width='600' 
                   style='background: #fff; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.1);'>
                <tr>
                    <td style='padding: 20px; text-align: center; background-color: #007bff; color: #fff; border-top-left-radius: 8px; border-top-right-radius: 8px;'>
                        <h2>Đặt lại mật khẩu</h2>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 30px; text-align: center; color: #333;'>
                        <p>Xin chào,</p>
                        <p>Bạn vừa yêu cầu đặt lại mật khẩu cho tài khoản của mình.</p>
                        <p>Vui lòng bấm vào nút bên dưới để đặt lại mật khẩu:</p>
                        <p>
                            <a href='{resetLink}' 
                               style='display: inline-block; padding: 12px 24px; margin-top: 20px;
                                      background-color: #007bff; color: #fff; text-decoration: none; 
                                      font-size: 16px; border-radius: 5px;'>
                                Đặt lại mật khẩu
                            </a>
                        </p>
                        <p style='margin-top: 30px; font-size: 13px; color: #666;'>
                            Nếu bạn không yêu cầu, vui lòng bỏ qua email này.<br/>
                            Link có hiệu lực trong vòng <b>15 phút</b>.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 15px; text-align: center; font-size: 12px; color: #999; background: #f9f9f9; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px;'>
                        © 2025 Shop ThienThanNho. Mọi quyền được bảo lưu.
                    </td>
                </tr>
            </table>
        </body>
        </html>
    ";

			// Plain text fallback
			bodyBuilder.TextBody = $"Nhấn vào link để reset mật khẩu: {resetLink}";

			message.Body = bodyBuilder.ToMessageBody();

			using var client = new SmtpClient();
			client.Connect("smtp.gmail.com", 587, false);
			client.Authenticate("contact.dev24h@gmail.com", "jcrx xzfh ahpe oisk"); // App password
			client.Send(message);
			client.Disconnect(true);
		}

	}
}

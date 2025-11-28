using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
	public interface IUserRepository
	{// Lấy user theo email
		Users GetUserByEmail(string email);

		// Lưu token reset password
		void SaveResetToken(string email, string token, DateTime expiry);

		// Kiểm tra token còn hạn không
		Users ValidateResetToken(string token);

		// Đổi mật khẩu mới
		void UpdatePassword(string email, string passwordHash);
		bool UpdateUserRole(int userId, string role);
	}
}

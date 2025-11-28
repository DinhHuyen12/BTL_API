using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace DAL.Interfaces
{
    public interface IAuthRepository
    {
        public Users Login (string username);

        public Dictionary<string, object> CreateUser(Users user);

        public Dictionary<string, object> UpdateUser(Users user);
        public Dictionary<string, object> DeleteUser(int userId);
		//public Dictionary<string, object> ForgotPassword(string email);
		//public Dictionary<string, object> ResetPassword(string token, string newPasswordHash);
		//public PasswordResetToken GetResetToken(string token);
		//public Users GetUserByEmail(string email);

		bool GenerateTwoFactorCode(string email);
		bool VerifyTwoFactorCode(string email, string code);
		Users GetUserByEmail(string email);
		string GenerateJwtToken(Users user);


		// ⭐ NEW: KIỂM TRA TOKEN (GIẢI MÃ JWT)
		Users ValidateJwtToken(string token);

		// ⭐ OPTIONAL: lấy user bằng username (Repository đang cần)
		Users GetUserByUsername(string username);
		List<Users> GetAllUsers();
		bool UpdateUserRole(int userId, string role);


	}
}

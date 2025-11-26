using BLL.Interfaces;
using Model;
using DAL.Interfaces;
using System;
using Helper;
using System.Security.Claims;
using System.Text;
namespace BLL
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IAuthRepository _authRepository;


		public AuthBusiness(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Users Login(string username, string password)
        {
            // Lấy user từ DB
            var user = _authRepository.Login(username);
            if (user == null) return null;

			bool isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
			return isValid ? user : null;

			

		}
		

		public Dictionary<string,object> CreateUser(Users user)
        {
     
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
            return _authRepository.CreateUser(user);
        }

		//public Dictionary<string, object> UpdateUser(Users user)
		//{
		//    return _authRepository.UpdateUser(user);
		//}
		public Dictionary<string, object> UpdateUser(Users user)
		{
			// Nếu bạn muốn cho phép update mật khẩu thì hash lại:
			if (!string.IsNullOrEmpty(user.PasswordHash))
			{
				user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
			}

			return _authRepository.UpdateUser(user);
		}


		public Dictionary<string, object> DeleteUser(int userId)
		{
			return _authRepository.DeleteUser(userId);
		}

		public bool SendTwoFactorCode(string email)
		{
			return _authRepository.GenerateTwoFactorCode(email);
		}

		public bool VerifyTwoFactorCode(string email, string code)
		{
			return _authRepository.VerifyTwoFactorCode(email, code);
		}

		public Users GetUserByEmail(string email)
		{
			return _authRepository.GetUserByEmail(email);
		}

		public string GenerateJwtToken(Users user)
		{
			// Nếu DAL có private GenerateJwtToken, bạn có thể expose nó qua public method trong DAL.
			return _authRepository.GenerateJwtToken(user);
		}
		// ✅ Bổ sung mới cho xác thực 2 bước
		public bool GenerateTwoFactorCode(string email)
		{
			return _authRepository.GenerateTwoFactorCode(email);
		}


		// =========================
		// VALIDATE TOKEN (CHECK EXP)
		// =========================
		public Users ValidateJwtToken(string token)
		{
			return _authRepository.ValidateJwtToken(token);
		}

		// =========================
		// LẤY USER THEO USERNAME
		// =========================
		public Users GetUserByUsername(string username)
		{
			return _authRepository.GetUserByUsername(username);
		}
		public List<Users> GetAllUsers()
		{
			return _authRepository.GetAllUsers();
		}

	}
}

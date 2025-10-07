using BLL.Interfaces;
using Model;
using DAL.Interfaces;
using System;
using Helper;

namespace BLL
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IAuthRepository _authRepository;

        public AuthBusiness(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Users Login(string username,string password)
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

	}
}

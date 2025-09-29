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

        public bool UpdateUser(Users user)
        {
            return _authRepository.UpdateUser(user);
        }

        public bool DeleteUser(Users user)
        {
            return _authRepository.DeleteUser(user);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public interface IAuthBusiness
    {
        public Users Login(string username, string password);

        public Dictionary<string,object> CreateUser(Users user);

        public Dictionary<string, object> UpdateUser(Users user);
        public Dictionary<string, object> DeleteUser(int userId);

		bool SendTwoFactorCode(string email);
		bool VerifyTwoFactorCode(string email, string code);
		bool GenerateTwoFactorCode(string email);
		Users GetUserByEmail(string email);
		string GenerateJwtToken(Users user);

	}
}

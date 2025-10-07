using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IUserService
	{
		Dictionary<string, object> ForgotPassword(string email);
		Dictionary<string, object> ResetPassword(string token, string newPassword);
	}
}

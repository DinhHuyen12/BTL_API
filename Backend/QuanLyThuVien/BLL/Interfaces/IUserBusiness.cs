using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IUserBusiness
	{
		bool UpdateUserRole(int userId, string newRole);
	}
}

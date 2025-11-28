using BLL.Interfaces;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
	public class UserBusiness
	{
		private readonly IUserRepository _userRepository;

		private readonly IUserBusiness _userBusiness;

		public UserBusiness(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		public bool UpdateUserRole(int userId, string role)
		{
			return _userBusiness.UpdateUserRole(userId, role);
		}

	}
}

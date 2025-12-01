using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
	public class UserRoleService : IUserRoleService
	{
		private readonly IUserRoleRepository _userRoleRepository;


		private readonly IUserRoleRepository _repo;

		public UserRoleService(IUserRoleRepository repo)
		{
			_repo = repo;
		}

		public Dictionary<string, object> UpdateUserRole(UpdateUserRoleRequest request)
		{
			return _repo.UpdateUserRole(request);
		}
		public Dictionary<string, object> DeleteUserRole(DeleteUserRoleRequest request)
		{
			return _repo.DeleteUserRole(request);
		}
		public Dictionary<string, object> GetUserRoles(int userId)
		{
			return _repo.GetUserRoles(userId);
		}
		public Dictionary<string, object> AssignUserRole(AssignUserRoleRequest request)
	=> _repo.AssignUserRole(request);
	}
}

using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
	public interface IUserRoleRepository
	{
		Dictionary<string, object> UpdateUserRole(UpdateUserRoleRequest request);
		Dictionary<string, object> DeleteUserRole(DeleteUserRoleRequest request);
		Dictionary<string, object> GetUserRoles(int userId);
		Dictionary<string, object> AssignUserRole(AssignUserRoleRequest request);
	}
}

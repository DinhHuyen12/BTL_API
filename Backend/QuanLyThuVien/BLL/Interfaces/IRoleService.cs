using Model;
using QuanLyThuVien.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IRoleService
	{
		Dictionary<string, object> AddRole(AddRoleRequest request);
		Dictionary<string, object> UpdateRole(UpdateRoleRequest request);
		Dictionary<string, object> DeleteRole(int roleId);
	}
}

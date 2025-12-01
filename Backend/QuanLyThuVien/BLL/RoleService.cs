using BLL.Interfaces;
using DAL.Interfaces;
using Model;
using QuanLyThuVien.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _repository;

		public RoleService(IRoleRepository repository)
		{
			_repository = repository;
		}

		public Dictionary<string, object> AddRole(AddRoleRequest request)
		{
			return _repository.AddRole(request);
		}
		public Dictionary<string, object> UpdateRole(UpdateRoleRequest request)
		{
			return _repository.UpdateRole(request);
		}
		public Dictionary<string, object> DeleteRole(int roleId)
		{
			return _repository.DeleteRole(roleId);
		}

	}
}

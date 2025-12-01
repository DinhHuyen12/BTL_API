using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;
using QuanLyThuVien.Models.Requests;

namespace QuanLyThuVien.Controllers
{
	[ApiController]                            // <-- BẮT BUỘC
	[Route("api/[controller]")]
	public class RoleController : ControllerBase
	{
		private readonly IUserRoleService _userRoleService;
		private readonly IRoleService _roleService;

		public RoleController(IUserRoleService userRoleService, IRoleService roleService)
		{
			_userRoleService = userRoleService;
			_roleService = roleService;
		}

		
		[HttpPut("update-role")]
		public IActionResult UpdateRole([FromBody] UpdateRoleRequest request)
		{
			var result = _roleService.UpdateRole(request);
			return Ok(result);
		}
		//[HttpPut("update")]
		//public IActionResult UpdateUserRole([FromBody] UpdateUserRoleRequest request)
		//{
		//	var result = _userRoleService.UpdateUserRole(request);
		//	return Ok(result);
		//}

		[HttpPost("add")]
		public IActionResult AddRole([FromBody] AddRoleRequest request)
		{
			var result = _roleService.AddRole(request);
			return Ok(result);
		}
		[HttpDelete("delete-role/{id}")]
		public IActionResult DeleteRole(int id)
		{
			var result = _roleService.DeleteRole(id);
			return Ok(result);
		}


	}
}

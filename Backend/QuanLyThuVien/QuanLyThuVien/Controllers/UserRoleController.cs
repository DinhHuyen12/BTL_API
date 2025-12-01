using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace QuanLyThuVien.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserRoleController : ControllerBase
	{
		private readonly IUserRoleService _userRoleService;

		public UserRoleController(IUserRoleService userRoleService)
		{
			_userRoleService = userRoleService;
		}

		[HttpPut("update")]
		public IActionResult UpdateUserRole([FromBody] UpdateUserRoleRequest request)
		{
			var result = _userRoleService.UpdateUserRole(request);
			return Ok(result);
		}
		//[HttpDelete("delete-user-role")]
		//public IActionResult DeleteUserRole([FromBody] DeleteUserRoleRequest request)
		//{
		//	var result = _userRoleService.DeleteUserRole(request);
		//	return Ok(result);
		//}
		[HttpDelete("delete-user-role")]
		public IActionResult DeleteUserRole([FromBody] DeleteUserRoleRequest request)
		{
			var result = _userRoleService.DeleteUserRole(request);
			return Ok(result);
		}

		// GET quyền của user
		[HttpGet("get-role/{userId}")]
		public IActionResult GetUserRole(int userId)
		{
			var result = _userRoleService.GetUserRoles(userId);
			return Ok(result);
		}
		[HttpPost("assign")]
		public IActionResult AssignRole([FromBody] AssignUserRoleRequest request)
		{
			var result = _userRoleService.AssignUserRole(request);
			return Ok(result);
		}
	}
}

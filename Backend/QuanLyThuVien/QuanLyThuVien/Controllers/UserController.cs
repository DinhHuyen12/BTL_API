﻿using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService service)
		{
			_userService = service;
		}

		/// <summary>
		/// Gửi email reset mật khẩu
		/// </summary>
		[HttpPost("forgot-password")]
		public IActionResult ForgotPassword([FromBody] Dictionary<string, string> parameters)
		{
			if (parameters == null || !parameters.ContainsKey("email"))
				return BadRequest(new { message = "Thiếu email" });

			string email = parameters["email"];
			var result = _userService.ForgotPassword(email);

			if (!(bool)result["Success"])
			{
				return BadRequest(new
				{
					message = result["Message"],
					errorCode = 1
				});
			}

			return Ok(new
			{
				message = result["Message"],
				data = result
			});
		}

		/// <summary>
		/// Đặt lại mật khẩu bằng token
		/// </summary>
		[HttpPost("reset-password")]
		public IActionResult ResetPassword([FromBody] Dictionary<string, string> parameters)
		{
			if (parameters == null ||
				!parameters.ContainsKey("token") ||
				!parameters.ContainsKey("newPassword"))
			{
				return BadRequest(new { message = "Thiếu token hoặc mật khẩu mới" });
			}

			string token = parameters["token"];
			string newPassword = parameters["newPassword"];

			var result = _userService.ResetPassword(token, newPassword);

			if (!(bool)result["Success"])
			{
				return BadRequest(new
				{
					message = result["Message"],
					errorCode = 1
				});
			}

			return Ok(new
			{
				message = result["Message"],
				data = result
			});
		}
	}
}

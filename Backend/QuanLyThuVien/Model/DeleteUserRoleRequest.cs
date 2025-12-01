using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class DeleteUserRoleRequest
	{
		public int UserId { get; set; }
		public int RoleId { get; set; }
	}
}

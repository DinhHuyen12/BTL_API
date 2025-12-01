using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class AssignUserRoleRequest
	{
		public int UserId { get; set; }
		public int RoleId { get; set; }
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class UpdateUserRoleRequest
	{
		public int UserId { get; set; }
		public int OldRoleId { get; set; }
		public int NewRoleId { get; set; }
	}
}

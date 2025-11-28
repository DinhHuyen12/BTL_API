using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Users
    {
        public int UserId { get; set; }
        public string Username { get; set; }

		public string PasswordHash { get; set; } 
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
		public string? PasswordResetToken { get; set; }
		public DateTime? TokenExpiry { get; set; }
		public string? token { get; set; }


	}
}

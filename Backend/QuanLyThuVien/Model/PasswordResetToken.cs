using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class PasswordResetToken
	{
		public Guid TokenId { get; set; }
		public int UserId { get; set; }
		public string Token { get; set; }
		public DateTime ExpiryDate { get; set; }
		public bool IsUsed { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public interface IAuthBusiness
    {
        public Users Login(string username, string password);

        public Dictionary<string,object> CreateUser(Users user);

        public bool UpdateUser(Users user);
        public bool DeleteUser(Users user);
    }
}

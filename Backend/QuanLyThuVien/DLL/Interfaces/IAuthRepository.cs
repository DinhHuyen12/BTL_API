using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace DAL.Interfaces
{
    public interface IAuthRepository
    {
        public Users Login (string username);

        public Dictionary<string, object> CreateUser(Users user);

        public bool UpdateUser(Users user);
        public bool DeleteUser(Users user);


    }
}

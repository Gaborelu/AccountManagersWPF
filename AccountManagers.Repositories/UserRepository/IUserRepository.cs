
using AccountManagers.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagers.DataAccess.UserRepository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        int InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}

using AccountManagers.Models.User;
using System.Collections.Generic;

namespace AccountManagers.Common.Utils
{
    public interface IUsersOutput
    {
        void WriteFile(IEnumerable<User> users);
    }
}
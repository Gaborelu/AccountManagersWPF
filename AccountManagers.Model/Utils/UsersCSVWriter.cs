using AccountManagers.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagers.Common.Utils
{
    public class UsersCSVWriter : IUsersOutput
    {
        public void WriteFile(IEnumerable<User> users)
        {
            var path = @"C:\temp\data.csv";

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    //header
                    sw.WriteLine("Id,Name,Email,CNP,NoOfClients");

                    //data
                    foreach (var user in users)
                    {
                        sw.WriteLine($"{user.Id},{user.Name},{user.Email},{user.CNP},{user.NoOfClients}");
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.Message);
            }
        }
    }
}


using AccountManagers.Common;

namespace AccountManagers.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        private string _cnp;

        public string CNP
        {
            get {
                return _cnp;
            }
            set { _cnp = value; }
        }


        public int NoOfClients { get; set; }

    }
}

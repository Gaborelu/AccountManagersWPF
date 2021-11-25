
using AccountManagers.Common;

namespace AccountManagers.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        private string _CNP;

        public string CNP
        {
            get {
                //return _CNP.Mask(0,2); 
                return _CNP;
            }
            set { _CNP = value; }
        }

        public int NoOfClients { get; set; }

    }
}

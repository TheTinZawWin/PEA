using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_Common
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class SessionUserModel
    {
        public string id { get; set; }
        public string UserName { get; set; }
    }
}

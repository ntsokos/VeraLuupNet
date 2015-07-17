using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Framework.JsonModels
{
    public class AuthTokenModel
    {
        public string Expires { get; set; }
        public string PK_Account { get; set; }
        public string PK_User { get; set; }
        public string Username { get; set; }
        public string Server_Auth { get; set; }
    }
}

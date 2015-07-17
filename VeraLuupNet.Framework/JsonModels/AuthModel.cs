using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Framework.JsonModels
{
    public class AuthVeraModel
    {
        public string Identity { get; set; }
        public string IdentitySignature { get; set; }
        public string Server_Event { get; set; }
        public string Server_Event_Alt { get; set; }
        public string Server_Account { get; set; }
        public string Server_Account_Alt { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Framework.JsonModels
{
    public class DeviceModel
    {
        public string PK_Device { get; set; }
        public string PK_DeviceType { get; set; }
        public string PK_DeviceSubType { get; set; }
        public string MacAddress { get; set; }
        public string Server_Device { get; set; }
        public string Server_Device_Alt { get; set; }
        public string PK_Installation { get; set; }
        public string Name { get; set; }

    }
}

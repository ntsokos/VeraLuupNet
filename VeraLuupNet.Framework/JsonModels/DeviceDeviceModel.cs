using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Framework.JsonModels
{
    public class DeviceDeviceModel
    {
        public string PK_Device { get; set; }
        public string MacAddress { get; set; }
        public string ExternalIP { get; set; }
        public string AccessiblePort { get; set; }
        public string InternalIP { get; set; }
        public string AliveDate { get; set; }
        public string FirmwareVersion { get; set; }
        public string UpgradeDat { get; set; }
        public string Uptime { get; set; }
        public string Server_Device { get; set; }
        public string Server_Event { get; set; }
        public string Server_Relay { get; set; }
        public string Server_Support { get; set; }
        public string Server_Storage { get; set; }
        public string Timezone { get; set; }
        public string LocalPort { get; set; }
        public string ZWaveLocale { get; set; }
        public string ZWaveVersion { get; set; }
        public string FK_Branding { get; set; }
        public string Platform { get; set; }
        public string UILanguage { get; set; }
        public string UISkin { get; set; }
        public string HasWifi { get; set; }
        public string HasAlarmPanel { get; set; }
        public string UI { get; set; }
        public string EngineStatus { get; set; }
    }
}

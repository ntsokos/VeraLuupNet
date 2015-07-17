using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VeraLuupNet.Framework.Enums; 

namespace VeraLuupNet.Framework.JsonModels
{
    public class LuupStatusReply
    {
        public Startup startup { get; set; }
        public List<Device> devices { get; set; }
        public List<Scene> scenes { get; set; }
        public int LoadTime { get; set; }
        public int DataVersion { get; set; }
        public int UserData_DataVersion { get; set; }
        public int TimeStamp { get; set; }
        public int ZWaveStatus { get; set; }
        public int Mode { get; set; }
        public string LocalTime { get; set; }

        public VeraHouseMode HouseMode 
        {
            get
            {
                var ret = (VeraHouseMode) this.Mode;
                return ret;
            }
        }
            
    }

 

}
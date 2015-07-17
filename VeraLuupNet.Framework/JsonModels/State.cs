using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VeraLuupNet.Framework.JsonModels
{
    public class State
    {
        public int id { get; set; }
        public string service { get; set; }
        public string variable { get; set; }
        public string value { get; set; }
    }
}
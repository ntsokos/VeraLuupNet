using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VeraLuupNet.Framework.JsonModels
{
    public class Device
    {
        public int id { get; set; }
        public List<State> states { get; set; }
        public List<object> Jobs { get; set; }
        public Tooltip tooltip { get; set; }
        public int status { get; set; }
    }
}
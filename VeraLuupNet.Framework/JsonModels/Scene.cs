using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VeraLuupNet.Framework.JsonModels
{

    public class Scene
    {
        public int id { get; set; }
        public List<object> Jobs { get; set; }
        public Tooltip2 tooltip { get; set; }
        public int status { get; set; }
        public bool active { get; set; }
    }

}
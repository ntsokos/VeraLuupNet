using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Helpers
{
    public static class VeraLuupNetHelpers
    {
        public static bool IsJson(string str)
        {
            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(str);
                return (parsedJson != null); 
            }
            catch (Exception)
            {
                return false; 
            }

            return false;
        }

        public static string FormatJSON(string jsonStr)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(jsonStr);
            var ret = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            return ret;
        }
    }
}

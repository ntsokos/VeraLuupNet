using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Framework.Helpers
{
    public static class FrameworkHelpers
    {
        public static string GetRandomUrl(string url1, string url2)
        {
            Random rand = new Random();
            var dbl = rand.NextDouble();
            var index = dbl >= 0.5 ? 1 : 0;

            var ret = new String[] { url1, url2 }[index];

            return ret;
        }
    }
}

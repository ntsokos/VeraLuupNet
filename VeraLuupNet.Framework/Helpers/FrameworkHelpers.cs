using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static string GetHashedPassword(string username, string password)
        {
            const string SEED = "oZ7QE6LcLJp6fiWzdqZc";

            var finalString = username.ToLower() + password + SEED;
            var finalSha1PassBytes = Encoding.UTF8.GetBytes(finalString);
            var retBytes = new SHA1Cng().ComputeHash(finalSha1PassBytes);

            var ret = BitConverter.ToString(retBytes).Replace("-", "");


            return ret;
        }
    }
}

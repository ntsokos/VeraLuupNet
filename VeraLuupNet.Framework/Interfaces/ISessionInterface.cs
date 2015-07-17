using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeraLuupNet.Framework.Interfaces
{
    public interface ISessionInterface
    {
        void AddValue(string key, string value);
        string GetValue(string key);
    }
}

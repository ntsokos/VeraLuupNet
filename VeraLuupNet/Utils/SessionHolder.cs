using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeraLuupNet.Framework.Interfaces;

namespace VeraLuupNet.Utils
{
    public class SessionHolder: ISessionInterface
    {
        #region [ private properties ]

        private Dictionary<string, string> SessionDictionary;

        #endregion

        #region [ constructor ]

        public SessionHolder()
        {
            this.SessionDictionary = new Dictionary<string, string>();
        }

        #endregion

        #region [ ISessionInterface implementation ]

        public void AddValue(string key, string value)
        {
            this.SessionDictionary[key] = value; 
        }

        public string GetValue(string key)
        {
            var ret = string.Empty; 

            if (this.SessionDictionary.ContainsKey(key))
                ret = this.SessionDictionary[key];

            return ret;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia
{
    public class PlayerInfo
    {
        public string name { get; set; }
        public string flag { get; set; }
        public string regexMatch { get; set; }

        public PlayerInfo()
        {
            name = string.Empty;
            flag = string.Empty;
            regexMatch = string.Empty;
        }

        public PlayerInfo(string name, string flag, string regex)
        {
            this.name = name;
            this.flag = flag;
            regexMatch = regex;
        }
    }
}

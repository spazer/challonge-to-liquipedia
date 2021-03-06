﻿using System;
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
        public int smashggID { get; set; }

        public PlayerInfo()
        {
            name = string.Empty;
            flag = string.Empty;
            regexMatch = string.Empty;
            smashggID = 0;
        }

        public PlayerInfo(string name, string flag, string regex, int id)
        {
            this.name = name;
            this.flag = flag;
            regexMatch = regex;
            smashggID = id;
        }
    }
}

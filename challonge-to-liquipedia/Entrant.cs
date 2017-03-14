using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia
{
    class Entrant
    {
        public string gamertag;
        public string flag;

        public Entrant(string gamertag)
        {
            this.gamertag = gamertag;
            this.flag = string.Empty;
        }

        public Entrant(string gamertag, string flag)
        {
            this.gamertag = gamertag;
            this.flag = flag;
        }
    }
}

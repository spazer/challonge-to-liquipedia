using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia
{
    public class Entrant
    {
        private string gamertag;
        private string flag;
        private int placement;

        #region Constructors
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
        #endregion

        #region Properties
        public string Gamertag
        {
            get { return gamertag; }
            set { gamertag = value; }
        }

        public string Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public int Placement
        {
            get { return placement; }
            set { placement = value; }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia
{
    class TableEntry
    {
        private string oldname;
        private string newname;
        private string regex;
        private string flag;

        public TableEntry(string oldname, string newname, string regex, string flag)
        {
            this.oldname = oldname;
            this.newname = newname;
            this.regex = regex;
            this.flag = flag;
        }

        public string OldName
        {
            get { return oldname; }
            set { oldname = value; }
        }

        public string NewName
        {
            get { return newname; }
            set { newname = value; }
        }

        public string Regex
        {
            get { return regex; }
            set { regex = value; }
        }

        public string Flag
        {
            get { return flag; }
            set { flag = value; }
        }
    }
}

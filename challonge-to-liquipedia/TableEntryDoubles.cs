using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia
{
    class TableEntryDoubles
    {
        private string oldname;
        private string newname;
        private string regex;
        private string flag;

        private string oldname2;
        private string newname2;
        private string regex2;
        private string flag2;

        public TableEntryDoubles(string oldname, string newname, string regex, string flag, string oldname2, string newname2, string regex2, string flag2)
        {
            this.oldname = oldname;
            this.newname = newname;
            this.regex = regex;
            this.flag = flag;

            this.oldname2 = oldname2;
            this.newname2 = newname2;
            this.regex2 = regex2;
            this.flag2 = flag2;
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

        public string OldName2
        {
            get { return oldname2; }
            set { oldname2 = value; }
        }

        public string NewName2
        {
            get { return newname2; }
            set { newname2 = value; }
        }

        public string Regex2
        {
            get { return regex2; }
            set { regex2 = value; }
        }

        public string Flag2
        {
            get { return flag2; }
            set { flag2 = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace challonge_to_liquipedia
{
    public partial class FormPlayerReplace : Form
    {
        BindingSource bs;
        List<TableEntry> table;
        Dictionary<int, Entrant> entrantListRef;

        public FormPlayerReplace(ref Dictionary<int, Entrant> entrantList, PlayerDatabase playerdb)
        {
            InitializeComponent();

            entrantListRef = entrantList;
            bs = new BindingSource();
            table = new List<TableEntry>();
            
            foreach (Entrant player in entrantList.Values)
            {
                bool matchFound = false;
                foreach (PlayerInfo info in playerdb.players)
                {
                    if (Regex.IsMatch(player.Gamertag, info.regexMatch))
                    {
                        table.Add(new TableEntry(player.Gamertag, info.name, info.regexMatch, info.flag));
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    table.Add(new TableEntry(player.Gamertag, string.Empty, string.Empty, string.Empty));
                }
            }

            bs.DataSource = table;
            dataGridViewPlayers.AutoGenerateColumns = true;
            dataGridViewPlayers.DataSource = bs;

            // Make column 1 read-only
            dataGridViewPlayers.Columns[0].ReadOnly = true;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            // Transfer data from the table back to entrantList
            for (int i=0; i < table.Count; i++)
            {
                if (table[i].NewName != string.Empty)
                {
                    entrantListRef.ElementAt(i).Value.Gamertag = table[i].NewName;
                }
                else
                {
                    entrantListRef.ElementAt(i).Value.Gamertag = table[i].OldName;
                }

                entrantListRef.ElementAt(i).Value.Flag = table[i].Flag;
            }

            // Close the form
            this.Close();
        }

        private void dataGridViewPlayers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && dataGridViewPlayers.CurrentCell.ColumnIndex == 1)
            {
                e.Handled = true;
                DataGridViewCell cell = dataGridViewPlayers.Rows[0].Cells[0];
                dataGridViewPlayers.CurrentCell = cell;
                dataGridViewPlayers.BeginEdit(true);
            }
        }
    }
}

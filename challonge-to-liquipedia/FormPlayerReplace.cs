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
        enum BracketType { Singles, Doubles };

        BindingSource bs;
        List<TableEntry> tableSingles;
        List<TableEntryDoubles> tableDoubles;
        Dictionary<int, List<Entrant>> entrantListRef;

        private BracketType detectedBracketType = BracketType.Singles;

        public FormPlayerReplace(ref Dictionary<int, List<Entrant>> entrantList, PlayerDatabase playerdb)
        {
            InitializeComponent();

            entrantListRef = entrantList;
            bs = new BindingSource();
            

            // Determine bracket type
            foreach (List<Entrant> playerList in entrantList.Values)
            {
                if (playerList.Count > 1)
                {
                    detectedBracketType = BracketType.Doubles;
                }
            }

            if (detectedBracketType == BracketType.Singles)
            {
                tableSingles = new List<TableEntry>();
            }
            else
            {
                tableDoubles = new List<TableEntryDoubles>();
            }

            // Match regex strings against each player and see what comes up
            foreach (List<Entrant> playerList in entrantList.Values)
            {
                if (detectedBracketType == BracketType.Singles)
                {
                    bool matchFound = false;
                    foreach (PlayerInfo info in playerdb.players)
                    {
                        // Add matches to the table
                        if (Regex.IsMatch(playerList[0].Gamertag, info.regexMatch, RegexOptions.IgnoreCase))
                        {
                            tableSingles.Add(new TableEntry(playerList[0].Gamertag, info.name, info.regexMatch, info.flag));
                            matchFound = true;
                            break;
                        }
                    }

                    // Add empty strings to the table if no match is found
                    if (!matchFound)
                    {
                        tableSingles.Add(new TableEntry(playerList[0].Gamertag, string.Empty, string.Empty, string.Empty));
                    }
                }
                else
                {
                    string p1tag = string.Empty;
                    string p1name = string.Empty;
                    string p1regex = string.Empty;
                    string p1flag = string.Empty;

                    string p2tag = string.Empty;
                    string p2name = string.Empty;
                    string p2regex = string.Empty;
                    string p2flag = string.Empty;

                    bool matchFound = false;
                    foreach (PlayerInfo info in playerdb.players)
                    {
                        // Retrieve info if a match is found
                        if (Regex.IsMatch(playerList[0].Gamertag, info.regexMatch, RegexOptions.IgnoreCase))
                        {
                            p1tag = playerList[0].Gamertag;
                            p1name = info.name;
                            p1regex = info.regexMatch;
                            p1flag = info.flag;
                            matchFound = true;
                            break;
                        }
                    }
                    if (!matchFound)
                    {
                        p1tag = playerList[0].Gamertag;
                    }

                    // Repeat for player 2 if they exist
                    if (playerList.Count > 1)
                    {
                        matchFound = false;
                        foreach (PlayerInfo info in playerdb.players)
                        {
                            // Retrieve info if a match is found
                            if (Regex.IsMatch(playerList[1].Gamertag, info.regexMatch, RegexOptions.IgnoreCase))
                            {
                                p2tag = playerList[1].Gamertag;
                                p2name = info.name;
                                p2regex = info.regexMatch;
                                p2flag = info.flag;
                                matchFound = true;
                                break;
                            }
                        }
                        if (!matchFound)
                        {
                            p2tag = playerList[1].Gamertag;
                        }
                    }

                    tableDoubles.Add(new TableEntryDoubles(p1tag, p1name, p1regex, p1flag, p2tag, p2name, p2regex, p2flag));
                }
            }

            if (detectedBracketType == BracketType.Singles)
            {
                bs.DataSource = tableSingles;
            }
            else
            {
                bs.DataSource = tableDoubles;
            }
            
            dataGridViewPlayers.AutoGenerateColumns = true;
            dataGridViewPlayers.DataSource = bs;

            // Make column 1 read-only
            dataGridViewPlayers.Columns[0].ReadOnly = true;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (detectedBracketType == BracketType.Singles)
            {
                // Transfer data from the table back to entrantList
                for (int i = 0; i < tableSingles.Count; i++)
                {
                    if (tableSingles[i].NewName != string.Empty)
                    {
                        entrantListRef.ElementAt(i).Value[0].Gamertag = tableSingles[i].NewName;
                    }
                    else
                    {
                        entrantListRef.ElementAt(i).Value[0].Gamertag = tableSingles[i].OldName;
                    }

                    entrantListRef.ElementAt(i).Value[0].Flag = tableSingles[i].Flag;
                }
            }
            else
            {
                // Transfer data from the table back to entrantList
                for (int i = 0; i < tableDoubles.Count; i++)
                {
                    if (tableDoubles[i].NewName != string.Empty)
                    {
                        entrantListRef.ElementAt(i).Value[0].Gamertag = tableDoubles[i].NewName;
                    }
                    else
                    {
                        entrantListRef.ElementAt(i).Value[0].Gamertag = tableDoubles[i].OldName;
                    }

                    if (tableDoubles[i].NewName2 != string.Empty)
                    {
                        entrantListRef.ElementAt(i).Value[1].Gamertag = tableDoubles[i].NewName2;
                    }
                    else
                    {
                        entrantListRef.ElementAt(i).Value[1].Gamertag = tableDoubles[i].OldName2;
                    }

                    entrantListRef.ElementAt(i).Value[0].Flag = tableDoubles[i].Flag;
                    entrantListRef.ElementAt(i).Value[1].Flag = tableDoubles[i].Flag2;
                }
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

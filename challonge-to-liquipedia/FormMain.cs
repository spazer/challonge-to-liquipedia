using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using challonge_to_liquipedia.JSON_Structure;
using System.Text.RegularExpressions;

namespace challonge_to_liquipedia
{
    public partial class FormMain : Form
    {
        static string SMASH_DB_URI = "http://wiki.teamliquid.net/smash/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";
        static string FIGHTERS_DB_URI = "http://wiki.teamliquid.net/fighters/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";

        static string BASE_URI = "https://api.challonge.com/v1/tournaments/";
        static string EXTENSION_URI = ".json";
        static int PLAYER_BYE = 0;

        Dictionary<int, List<JSON_Structure.Match>> roundList = new Dictionary<int, List<JSON_Structure.Match>>();
        Dictionary<int, Entrant> entrantList = new Dictionary<int, Entrant>();

        PlayerDatabase playerdb;

        #region Form
        public FormMain()
        {
            InitializeComponent();

            // Read credentials if available
            try
            {
                StreamReader fileinfo = new StreamReader(@"userinfo");
                textBoxUsername.Text = fileinfo.ReadLine().Trim();
                textBoxPassword.Text = fileinfo.ReadLine().Trim().Unprotect();

                fileinfo.Close();
            }
            catch
            {
                Console.WriteLine("No saved credentials.");
            }

            if (radioButtonSmash.Checked == true)
            {
                playerdb = new PlayerDatabase(PlayerDatabase.DbSource.Smash);
            }
            else if (radioButtonFighters.Checked == true)
            {
                playerdb = new PlayerDatabase(PlayerDatabase.DbSource.Fighters);
            }
            UpdateRevID();
        }

        /// <summary>
        /// Save challonge credentials on close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBoxUsername.Text != string.Empty && textBoxPassword.Text != string.Empty)
            {
                try
                {
                    //Save credentials
                    StreamWriter userinfo = new StreamWriter(@"userinfo");
                    userinfo.WriteLine(textBoxUsername.Text);
                    userinfo.WriteLine(textBoxPassword.Text.Protect());

                    userinfo.Close();
                }
                catch
                {
                    Console.WriteLine("Couldn't save credentials.");
                }
            }
        }
        #endregion

        #region Buttons
        /// <summary>
        /// Retrieve data from challonge and parse it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRetrieve_Click(object sender, EventArgs e)
        {
            // Clear existing data
            roundList.Clear();
            entrantList.Clear();

            WebClient client = new WebClient { Credentials = new NetworkCredential(textBoxUsername.Text, textBoxPassword.Text) };
            string tournamentlink;
            string json = string.Empty;

            // Set the base URL for the tournament
            try
            {
                Uri inputUri = new Uri(textBoxURL.Text);
                tournamentlink = BASE_URI.ToString() + ParseURL(inputUri);
            }
            catch
            {
                richTextBoxOutput.Text = "Invalid URL";
                return;
            }

            try
            {
                // Only DE brackets are allowed
                string downloadURL = tournamentlink + EXTENSION_URI;
                client.QueryString.Add("include_participants", "1");
                client.QueryString.Add("include_matches", "1");
                json = client.DownloadString(downloadURL);

                // Convert json to UTF-8 for foreign text
                byte[] bytes = Encoding.Default.GetBytes(json);
                json = Encoding.UTF8.GetString(bytes);

                richTextBoxOutput.Text = json;
            }
            catch (WebException exception)
            {
                HttpWebResponse response = (System.Net.HttpWebResponse)exception.Response;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        richTextBoxOutput.Text = "Invalid API key or insufficient permissions";
                        break;
                    case HttpStatusCode.NotFound:
                        richTextBoxOutput.Text = "Object not found within your account scope";
                        break;
                    case HttpStatusCode.NotAcceptable:
                        richTextBoxOutput.Text = "Requested format is not supported - request JSON or XML only";
                        break;
                    case (HttpStatusCode)422:
                        richTextBoxOutput.Text = "Validation error(s) for create or update method";
                        break;
                    case (HttpStatusCode)500:
                        richTextBoxOutput.Text = "Something went wrong on Challonge's end. If you continually receive this, please contact Challonge.";
                        break;
                    default:
                        richTextBoxOutput.Text = response.StatusDescription;
                        break;
                }
            }

            // Deserialize the json
            JsonParser parser = new JsonParser();
            RootObject root = parser.ParseTournament(json);

            // Detect double elim brackets
            if (root.tournament.tournament_type != "double elimination")
            {
                MessageBox.Show("Warning: this is not a double elimination bracket. It is a:\r\n\r\n" + root.tournament.tournament_type);
            }

            // Parse the json
            GetSets(ref root);
            GetEntrants(ref root);
        }

        /// <summary>
        /// Fill the output window with wikicode and parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFill_Click(object sender, EventArgs e)
        {
            string output = string.Empty;

            // Don't proceed if data wasn't acquired
            if (entrantList.Count() == 0 || roundList.Count() == 0)
            {
                richTextBoxOutput.Text = "Acquire data first";
            }

            if (richTextBoxWinnersInput.Text != string.Empty)
            {
                output += "==Winners Bracket==\r\n";
                output += richTextBoxWinnersInput.Text;
                fillBracketSingles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output);
                output += "\r\n";
            }

            if (richTextBoxLosersInput.Text != string.Empty)
            {
                output += "==Losers Bracket==\r\n";
                output += richTextBoxLosersInput.Text;
                fillBracketSingles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output);
                output += "\r\n";
            }

            richTextBoxOutput.Text = output;
        }
        #endregion

        private void GetSets(ref RootObject root)
        {
            for (int i = 0; i < root.tournament.matches.Count(); i++)
            {
                if (root.tournament.matches[i].match.group_id == null)      // Skip group matches for now
                {
                    // Try to parse the score
                    bool scoreFound = false;
                    if (root.tournament.matches[i].match.scores_csv != string.Empty)
                    {
                        int index = root.tournament.matches[i].match.scores_csv.IndexOf("-");
                        if ( index != -1)       // Skip scores if no dash is found
                        {
                            if (index != 0)     // Look again if the index is zero, since that is probably a negative number
                            {
                                index = root.tournament.matches[i].match.scores_csv.IndexOf("-", 1);

                                if (index != -1)
                                {
                                    root.tournament.matches[i].match.entrant1wins = int.Parse(root.tournament.matches[i].match.scores_csv.Substring(0, index));
                                    root.tournament.matches[i].match.entrant2wins = int.Parse(root.tournament.matches[i].match.scores_csv.Substring(index + 1, root.tournament.matches[i].match.scores_csv.Length - index - 1));
                                    scoreFound = true;
                                }
                            }
                        }
                    }

                    if (!scoreFound)
                    {
                        root.tournament.matches[i].match.entrant1wins = -99;
                        root.tournament.matches[i].match.entrant2wins = -99;
                    }

                    // Check if the round already exists in roundList
                    if (roundList.ContainsKey(root.tournament.matches[i].match.round))
                    {
                        // Add the round
                        roundList[root.tournament.matches[i].match.round].Add(root.tournament.matches[i].match);
                    }
                    else    // Make a new entry for the round if it doesn't exist
                    {
                        // Add the current set to the newly created list
                        List<JSON_Structure.Match> newSetList = new List<JSON_Structure.Match>();
                        newSetList.Add(root.tournament.matches[i].match);

                        roundList.Add(root.tournament.matches[i].match.round, newSetList);
                    }
                }
            }

            // Only perform the entrants check if rounds 1 and 2 exist for winners and losers
            if (roundList.ContainsKey(1) && roundList.ContainsKey(2) && roundList.ContainsKey(-1) && roundList.ContainsKey(-2))
            {
                // Insert byes into round 1 and -1 if we don't have a power of 2
                int r1count = roundList[1].Count;
                if (!IsPowerOfTwo(roundList[1].Count))
                {
                    // Reconstruct the entirety of round 1 based on round 2 prerequisite matches
                    roundList[1].Clear();
                    foreach (JSON_Structure.Match r2match in roundList[2])
                    {
                        // Add a bye match if the prereq is null
                        if (r2match.player1_prereq_match_id == 0)
                        {
                            JSON_Structure.Match newMatch = new JSON_Structure.Match();

                            newMatch.player1_id = r2match.player1_id;
                            newMatch.player2_id = PLAYER_BYE;
                            newMatch.winner_id = r2match.player1_id;
                            newMatch.loser_id = PLAYER_BYE;
                            newMatch.round = 1;
                            newMatch.scores_csv = "1-0";
                            roundList[1].Add(newMatch);
                        }
                        // Otherwise add a match based on the prereq id
                        else
                        {
                            for (int i = 0; i < root.tournament.matches.Count(); i++) 
                            {
                                if (root.tournament.matches[i].match.id == r2match.player1_prereq_match_id)
                                {
                                    roundList[1].Add(root.tournament.matches[i].match);
                                    break;
                                }
                            }                            
                        }

                        // Add a bye match if the prereq is null
                        if (r2match.player2_prereq_match_id == 0)
                        {
                            JSON_Structure.Match newMatch = new JSON_Structure.Match();

                            newMatch.player1_id = r2match.player2_id;
                            newMatch.player2_id = PLAYER_BYE;
                            newMatch.winner_id = r2match.player2_id;
                            newMatch.loser_id = PLAYER_BYE;
                            newMatch.round = 1;
                            newMatch.scores_csv = "1-0";
                            roundList[1].Add(newMatch);
                        }
                        // Otherwise add a match based on the prereq id
                        else
                        {
                            for (int i = 0; i < root.tournament.matches.Count(); i++)
                            {
                                if (root.tournament.matches[i].match.id == r2match.player2_prereq_match_id)
                                {
                                    roundList[1].Add(root.tournament.matches[i].match);
                                    break;
                                }
                            }
                        }
                    }

                    // Refresh the r1 count. l1 should be half of this number.
                    int l1count = roundList[1].Count / 2;

                    // If l2 has less entries than l1, that means all of l2 has l1 prerequisites
                    if (roundList[-2].Count < l1count)
                    {
                        // Reconstruct the entirety of round -1 based on round -2 prerequisite matches
                        roundList[-1].Clear();
                        foreach (JSON_Structure.Match l2match in roundList[-2])
                        {
                            // Add a bye match if the prereq is null
                            if (l2match.player1_prereq_match_id == 0)
                            {
                                JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                newMatch.player1_id = l2match.player1_id;
                                newMatch.player2_id = PLAYER_BYE;
                                newMatch.winner_id = l2match.player1_id;
                                newMatch.loser_id = PLAYER_BYE;
                                newMatch.round = 1;
                                newMatch.scores_csv = "1-0";
                                roundList[-1].Add(newMatch);
                            }
                            // Otherwise add a match based on the prereq id
                            else
                            {
                                for (int i = 0; i < root.tournament.matches.Count(); i++)
                                {
                                    if (root.tournament.matches[i].match.id == l2match.player1_prereq_match_id)
                                    {
                                        roundList[-1].Add(root.tournament.matches[i].match);
                                        break;
                                    }
                                }
                            }

                            // Add a bye match if the prereq is null
                            if (l2match.player2_prereq_match_id == 0)
                            {
                                JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                newMatch.player1_id = l2match.player2_id;
                                newMatch.player2_id = PLAYER_BYE;
                                newMatch.winner_id = l2match.player2_id;
                                newMatch.loser_id = PLAYER_BYE;
                                newMatch.round = 1;
                                newMatch.scores_csv = "1-0";
                                roundList[-1].Add(newMatch);
                            }
                            // Otherwise add a match based on the prereq id
                            else
                            {
                                for (int i = 0; i < root.tournament.matches.Count(); i++)
                                {
                                    if (root.tournament.matches[i].match.id == l2match.player2_prereq_match_id)
                                    {
                                        roundList[-1].Add(root.tournament.matches[i].match);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // Otherwise, assume player 1 is a dropdown from winners, and does not need a prereq match
                    else
                    {
                        // Reconstruct the entirety of round -1 based on round -2 prerequisite matches
                        roundList[-1].Clear();
                        foreach (JSON_Structure.Match l2match in roundList[-2])
                        {
                            // Add a bye match if the prereq is null
                            if (l2match.player2_prereq_match_id == 0)
                            {
                                JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                newMatch.player1_id = l2match.player2_id;
                                newMatch.player2_id = PLAYER_BYE;
                                newMatch.winner_id = l2match.player2_id;
                                newMatch.loser_id = PLAYER_BYE;
                                newMatch.round = 1;
                                newMatch.scores_csv = "1-0";
                                roundList[-1].Add(newMatch);
                            }
                            // Otherwise add a match based on the prereq id
                            else
                            {
                                for (int i = 0; i < root.tournament.matches.Count(); i++)
                                {
                                    if (root.tournament.matches[i].match.id == l2match.player2_prereq_match_id)
                                    {
                                        // If this match is from winners, input a bye instead
                                        if (root.tournament.matches[i].match.round > 0)
                                        {
                                            JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                            newMatch.player1_id = l2match.player2_id;
                                            newMatch.player2_id = PLAYER_BYE;
                                            newMatch.winner_id = l2match.player2_id;
                                            newMatch.loser_id = PLAYER_BYE;
                                            newMatch.round = 1;
                                            newMatch.scores_csv = "1-0";
                                            roundList[-1].Add(newMatch);
                                        }
                                        else
                                        {
                                            roundList[-1].Add(root.tournament.matches[i].match);
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            // Sort rounds and sets
            roundList = roundList.OrderBy(x => Math.Abs(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            for (int i = 0; i < roundList.Count; i++)
            {
                List<JSON_Structure.Match> tempList = roundList[roundList.ElementAt(i).Key];

                for (int j = 0; j < tempList.Count; j++)
                {
                    tempList[j].matchnumber = j + 1;
                }

                roundList[roundList.ElementAt(i).Key] = tempList;
            }

            // Set values in form controls
            foreach (int displayRound in roundList.Keys.ToList<int>())
            {
                int round = Math.Abs(displayRound);

                if (displayRound > 0)
                {
                    //if (eventType == EventType.Singles)
                    //{
                    //    checkBoxWinners.Checked = true;
                    //}
                    //else
                    //{
                    //    checkBoxWinners.Checked = true;
                    //}
                    //
                    //if (checkBoxLockWinners.Checked) continue;

                    // Remember lowest round number
                    if (numericUpDownWinnersStart.Value == 0)
                    {
                        numericUpDownWinnersStart.Value = round;
                    }
                    else if (round < numericUpDownWinnersStart.Value)
                    {
                        numericUpDownWinnersStart.Value = round;
                    }

                    // Remember highest round number
                    if (numericUpDownWinnersEnd.Value == 0)
                    {
                        numericUpDownWinnersEnd.Value = round;
                    }
                    else if (round > numericUpDownWinnersEnd.Value)
                    {
                        numericUpDownWinnersEnd.Value = round;
                    }
                }
                else if (displayRound < 0)
                {
                    //if (eventType == EventType.Singles)
                    //{
                    //    checkBoxLosers.Checked = true;
                    //}
                    //else
                    //{
                    //    checkBoxLosers.Checked = true;
                    //}

                    //if (checkBoxLockLosers.Checked) continue;

                    // Remember lowest round number
                    if (numericUpDownLosersStart.Value == 0)
                    {
                        numericUpDownLosersStart.Value = round;
                    }
                    else if (round < numericUpDownLosersStart.Value)
                    {
                        numericUpDownLosersStart.Value = round;
                    }

                    // Remember highest round number
                    if (numericUpDownLosersEnd.Value == 0)
                    {
                        numericUpDownLosersEnd.Value = round;
                    }
                    else if (round > numericUpDownLosersEnd.Value)
                    {
                        numericUpDownLosersEnd.Value = round;
                    }
                }
            }
        }

        private void GetEntrants(ref RootObject root)
        {
            // Populate the entrant list using the json data
            for (int i = 0; i < root.tournament.participants.Count(); i++)
            {
                string name = root.tournament.participants[i].participant.name;

                // Remove tags where possible
                if (name.Contains("|"))
                {
                    if (checkBoxTrimTags.Checked)
                    {
                        int start = name.IndexOf("|") + 1;
                        name = name.Substring(start).Trim();
                    }
                    else
                    {
                        name = name.Replace("|", "{{!}}");
                    }
                }

                entrantList.Add(root.tournament.participants[i].participant.id, new Entrant(name));
            }

            // Bring up the player replacement window
            FormPlayerReplace formReplace = new FormPlayerReplace(ref entrantList, playerdb);
            this.Hide();
            formReplace.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Gets the subdomain if one exists "___.challonge.com" versus "challonge.com" and generates the proper tournament name
        /// in the form *subdomain*-*bracket name*
        /// </summary>
        /// <param name="inputUri">Input URI</param>
        /// <returns></returns>
        private string ParseURL(Uri inputUri)
        {
            string result = string.Empty;
            string subdomain = string.Empty;

            // Find subdomain if applicable
            if (inputUri.Host.ToLower() != "challonge.com")
            {
                subdomain = inputUri.Host.Replace(".challonge.com", "");
                result = subdomain + "-" + inputUri.AbsolutePath.Substring(1);  // Remove the starting slash

                return result;
            }
            else
            {
                return inputUri.AbsolutePath.Substring(1);
            }
        }

        #region Fill Methods
        /// <summary>
        /// Fills bracket wikicode
        /// </summary>
        /// <param name="startRound">Round to start filling with</param>
        /// <param name="endRound">Round to stop filling with</param>
        /// <param name="offset">Number of rounds to offset by</param>
        /// <param name="bracketText">A string containing the bracket wikicode</param>
        private void fillBracketSingles(int startRound, int endRound, int offset, ref string bracketText)
        {
            int increment;
            string bracketSide;

            int lastround = roundList.Keys.Max();

            if (startRound > 0)
            {
                increment = 1;
                bracketSide = LpStrings.WRound;
            }
            else
            {
                increment = -1;
                bracketSide = LpStrings.LRound;
            }
            
            for (int i = startRound; Math.Abs(i) <= Math.Abs(endRound); i += increment)
            {
                int outputRound;

                // Skip rounds that don't exist
                if (!roundList.ContainsKey(i)) continue;

                // Iterate through all sets in the round
                for (int j = 0; j < roundList[i].Count; j++)
                {
                    JSON_Structure.Match currentSet = roundList[i][j];

                    outputRound = Math.Abs(i) + offset;

                    // Check for player byes
                    if (currentSet.player1_id == PLAYER_BYE && currentSet.player2_id == PLAYER_BYE)
                    {
                        // If both players are byes, skip this entry
                        continue;
                    }
                    else if (currentSet.player1_id == PLAYER_BYE)
                    {
                        // Fill in player 1 as a bye if fill byes is checked
                        if (checkBoxFillByes.Checked == true) FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1, "Bye");

                        // Give player 2 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2, entrantList[currentSet.player2_id].Gamertag);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id].Flag);
                        if (checkBoxFillByeWins.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.Win, "2");
                        }
                    }
                    else if (currentSet.player2_id == PLAYER_BYE)
                    {
                        // Fill in player 2 as a bye
                        if (checkBoxFillByes.Checked == true) FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2, "Bye");

                        // Give player 1 a checkmark
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1, entrantList[currentSet.player1_id].Gamertag);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id].Flag);

                        if (checkBoxFillByeWins.Checked == true)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.Win, "1");
                        }
                    }
                    else
                    {
                        // Fill in the set normally
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1, entrantList[currentSet.player1_id].Gamertag);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id].Flag);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2, entrantList[currentSet.player2_id].Gamertag);
                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id].Flag);


                        // Check for DQs
                        if (currentSet.entrant1wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else if (currentSet.entrant2wins == -1)
                        {
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, "DQ");
                            FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                        }
                        else
                        {
                            if (currentSet.round == lastround && currentSet.player1_prereq_match_id == currentSet.player2_prereq_match_id)
                            {
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                }
                                else
                                {
                                    if (currentSet.winner_id == currentSet.player1_id)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, "{{win}}");
                                    }
                                    else if (currentSet.winner_id == currentSet.player2_id)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, "{{win}}");
                                    }
                                }
                            }
                            else
                            {
                                if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                                {
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                                }
                                else
                                {
                                    if (currentSet.winner_id == currentSet.player1_id)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Score, "{{win}}");
                                    }
                                    else if (currentSet.winner_id == currentSet.player2_id)
                                    {
                                        FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Score, "{{win}}");
                                    }
                                }
                            }
                        }

                        // Set the winner
                        if (currentSet.round == lastround && currentSet.player1_prereq_match_id == currentSet.player2_prereq_match_id)
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "2");
                            }
                            else if (currentSet.winner_id == currentSet.player2_id)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "1");
                            }
                        }
                        else if (currentSet.round == lastround && currentSet.matchnumber == 1 && roundList[i].Count > 1)
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + 1 + LpStrings.Win, "1");
                            }
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.Win, "1");
                            }
                            else if (currentSet.winner_id == currentSet.player2_id)
                            {
                                FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.Win, "2");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Find the specified liquipedia parameter and insert the specified value
        /// </summary>
        /// <param name="lpText">Block of liquipedia markup</param>
        /// <param name="param">Parameter to fill in</param>
        /// <param name="value">Value of the parameter</param>
        private void FillLPParameter(ref string lpText, string param, string value)
        {
            if (value == string.Empty) return;

            Regex rgx = new Regex(@"(\|" + param + @"=)([ \r\n])");
            System.Text.RegularExpressions.Match match = rgx.Match(lpText);

            if (match.Success)
            {
                lpText = lpText.Replace(match.Groups[1].Value + match.Groups[2].Value, match.Groups[1].Value + value + match.Groups[2].Value);
            }
        }
        #endregion

        /// <summary>
        /// Indicates whether an integer is a power of 2
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        /// <summary>
        /// Retrieves a regex AKA database from Liquipedia, and parses it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAKA_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();

            // Decide on the URL to use
            string json = string.Empty;
            if (radioButtonSmash.Checked)
            {
                json = client.DownloadString(SMASH_DB_URI);
            }
            else if (radioButtonFighters.Checked)
            {
                json = client.DownloadString(FIGHTERS_DB_URI);
            }
            else
            {
                return;
            }

            richTextBoxOutput.Text = json;

            // Save the json to file, then read the file
            if (radioButtonSmash.Checked)
            {
                if (!playerdb.SaveDatabase(json, PlayerDatabase.DbSource.Smash))
                {
                    richTextBoxOutput.Text = "Could not save Smash Database";
                }

                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Smash))
                {
                    richTextBoxOutput.Text = "Could not retrieve Smash Database";
                }
            }
            else if (radioButtonFighters.Checked)
            {
                if (!playerdb.SaveDatabase(json, PlayerDatabase.DbSource.Fighters))
                {
                    richTextBoxOutput.Text = "Could not save Fighters Database";
                }

                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Fighters))
                {
                    richTextBoxOutput.Text = "Could not retrieve Fighters Database";
                }
            }
            else
            {
                return;
            }

            // Update the revision number
            UpdateRevID();
        }

        /// <summary>
        /// Updates the revision number of the AKA database
        /// </summary>
        private void UpdateRevID()
        {
            if (playerdb == null) return;

            int revID = playerdb.RevID;

            if (revID == 0)
            {
                labelAkaDatabaseRev.Text = "Rev: none";
            }
            else
            {
                labelAkaDatabaseRev.Text = "Rev: " + revID.ToString();
            }
        }

        private void radioButtonDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSmash.Checked)
            {
                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Smash))
                {
                    richTextBoxOutput.Text = "Could not retrieve Smash Database";
                }
            }
            else if (radioButtonFighters.Checked)
            {
                if (!playerdb.ReadDatabaseFromFile(PlayerDatabase.DbSource.Fighters))
                {
                    richTextBoxOutput.Text = "Could not retrieve Fighters Database";
                }
            }
            else
            {
                return;
            }
        }
    }
}

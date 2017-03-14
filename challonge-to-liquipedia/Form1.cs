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
        static string BASE_URI = "https://api.challonge.com/v1/tournaments/";
        static string EXTENSION_URI = ".json";
        
        Dictionary<int, List<challonge_to_liquipedia.JSON_Structure.Match>> roundList = new Dictionary<int, List<challonge_to_liquipedia.JSON_Structure.Match>>();
        Dictionary<int, Entrant> entrantList = new Dictionary<int, Entrant>();

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
        }

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

        private void buttonRetrieve_Click(object sender, EventArgs e)
        {
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

            GetSets(ref root);
            GetEntrants(ref root);
        }

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
                        List<challonge_to_liquipedia.JSON_Structure.Match> newSetList = new List<challonge_to_liquipedia.JSON_Structure.Match>();
                        newSetList.Add(root.tournament.matches[i].match);

                        roundList.Add(root.tournament.matches[i].match.round, newSetList);
                    }
                }
            }

            // Sort rounds and sets
            roundList = roundList.OrderBy(x => Math.Abs(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            for (int i = 0; i < roundList.Count; i++)
            {
                List<challonge_to_liquipedia.JSON_Structure.Match> tempList = roundList[roundList.ElementAt(i).Key];

                tempList = tempList.OrderBy(x => x.id).ToList();

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
                entrantList.Add(root.tournament.participants[i].participant.id, new Entrant(root.tournament.participants[i].participant.name));
            }

            // Todo:
            // Normalize names using regex
            // Insert flags where available
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
                //result = inputUri.AbsolutePath.Substring(1);

                return inputUri.AbsolutePath.Substring(1);
            }
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            string output = string.Empty;

            if (richTextBoxWinnersInput.Text != string.Empty)
            {
                output += "===Winners Bracket===\r\n";
                output += richTextBoxWinnersInput.Text;
                fillBracketSingles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output);
                output += "\r\n";
            }

            if (richTextBoxLosersInput.Text != string.Empty)
            {
                output += "===Losers Bracket===\r\n";
                output += richTextBoxLosersInput.Text;
                fillBracketSingles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output);
                output += "\r\n";
            }

            richTextBoxOutput.Text = output;
        }

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
                    challonge_to_liquipedia.JSON_Structure.Match currentSet = roundList[i][j];

                    // Check to see if the players exist
                    if (currentSet.player1_id == 0) continue;
                    if (currentSet.player2_id == 0) continue;

                    outputRound = Math.Abs(i) + offset;
                    
                    // Fill in the set normally
                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1, entrantList[currentSet.player1_id].gamertag);
                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id].flag);
                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2, entrantList[currentSet.player2_id].gamertag);
                    FillLPParameter(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id].flag);

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
            //Regex.Replace(lpText, @"(\|" + param + @"=)([ \r\n])","$1"+value+"$2",)
            //lpText = rgx.Replace(lpText, @"$1" + Regex.Escape(value) + "$2");

            //int start = lpText.IndexOf("|" + param + "=");

            //if (start != -1)
            //{
            //    start += param.Length + 2;
            //    lpText = lpText.Insert(start, value);
            //}
        }
    }
}

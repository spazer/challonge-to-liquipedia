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
using System.IO.Compression;
using challonge_to_liquipedia.JSON_Structure;
using System.Text.RegularExpressions;

namespace challonge_to_liquipedia
{
    public partial class FormMain : Form
    {
        static string DEFINALSMWBRACKET = "{{DEFinalSmwBracket\r\n|tourneylink=\r\n|tourneyname=\r\n|l1placement=4\r\n|r2placement=3\r\n|r3loserplacement=2\r\n|r3winnerplacement=1\r\n\r\n<!-- FROM WINNERS -->\r\n|r1m1p1= |r1m1p1flag= |r1m1p1score=\r\n|r1m1p2= |r1m1p2flag= |r1m1p2score=\r\n|r1m1win=\r\n\r\n<!-- FROM LOSERS -->\r\n|l1m1p1= |l1m1p1flag= |l1m1p1score=\r\n|l1m1p2= |l1m1p2flag= |l1m1p2score=\r\n|l1m1win=\r\n\r\n<!-- LOSERS FINALS -->\r\n|r2m1p1= |r2m1p1flag= |r2m1p1score=\r\n|r2m1p2= |r2m1p2flag= |r2m1p2score=\r\n|r2m1win=\r\n\r\n<!-- GRAND FINALS -->\r\n|r3m1p1= |r3m1p1flag= |r3m1p1score= |r3m2p1score=\r\n|r3m1p2= |r3m1p2flag= |r3m1p2score= |r3m2p2score=\r\n|r3m1win=\r\n}}";
        static string DEFINALDOUBLESSMWBRACKET = "{{DEFinalDoublesSmwBracket\r\n|tourneylink=\r\n|tourneyname=\r\n|l1placement=4\r\n|r2placement=3\r\n|r3loserplacement=2\r\n|r3winnerplacement=1\r\n\r\n<!-- FROM WINNERS -->\r\n|r1m1t1p1= |r1m1t1p1flag=\r\n|r1m1t1p2= |r1m1t1p2flag= |r1m1t1score=\r\n|r1m1t2p1= |r1m1t2p1flag=\r\n|r1m1t2p2= |r1m1t2p2flag= |r1m1t2score=\r\n|r1m1win=\r\n\r\n<!-- FROM LOSERS -->\r\n|l1m1t1p1= |l1m1t1p1flag=\r\n|l1m1t1p2= |l1m1t1p2flag= |l1m1t1score=\r\n|l1m1t2p1= |l1m1t2p1flag=\r\n|l1m1t2p2= |l1m1t2p2flag= |l1m1t2score=\r\n|l1m1win=\r\n\r\n<!-- LOSERS FINALS -->\r\n|r2m1t1p1= |r2m1t1p1flag=\r\n|r2m1t1p2= |r2m1t1p2flag= |r2m1t1score=\r\n|r2m1t2p1= |r2m1t2p1flag=\r\n|r2m1t2p2= |r2m1t2p2flag= |r2m1t2score=\r\n|r2m1win=\r\n\r\n<!-- GRAND FINALS -->\r\n|r3m1t1p1= |r3m1t1p1flag=\r\n|r3m1t1p2= |r3m1t1p2flag= |r3m1t1score= |r3m2t1score=\r\n|r3m1t2p1= |r3m1t2p1flag=\r\n|r3m1t2p2= |r3m1t2p2flag= |r3m1t2score= |r3m2t2score=\r\n|r3m1win=\r\n}}";

        static string SMASH_DB_URI = "http://liquipedia.net/smash/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";
        static string FIGHTERS_DB_URI = "http://liquipedia.net/fighters/api.php?action=parse&page=Liquipedia:Players_Regex&prop=revid|wikitext&format=json";

        static string BASE_URI = "https://api.challonge.com/v1/tournaments/";
        static string EXTENSION_URI = ".json";
        static int PLAYER_BYE = 0;

        Dictionary<int, List<JSON_Structure.Match>> roundList = new Dictionary<int, List<JSON_Structure.Match>>();
        Dictionary<int, List<Entrant>> entrantList = new Dictionary<int, List<Entrant>>();

        PlayerDatabase playerdb;

        #region Form
        /// <summary>
        /// Main form constructor
        /// </summary>
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

            // Set
            radioButtonSinglesDoubles_CheckedChanged(this, new EventArgs());

            try
            { //try TLS 1.3
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288
                                                     | (SecurityProtocolType)3072
                                                     | (SecurityProtocolType)768
                                                     | SecurityProtocolType.Tls;
            }
            catch (NotSupportedException)
            {
                try
                { //try TLS 1.2
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
                                                         | (SecurityProtocolType)768
                                                         | SecurityProtocolType.Tls;
                }
                catch (NotSupportedException)
                {
                    try
                    { //try TLS 1.1
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)768
                                                             | SecurityProtocolType.Tls;
                    }
                    catch (NotSupportedException)
                    { //TLS 1.0
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    }
                }
            }
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
                client.Headers.Add("User-Agent: challonge-to-liquipedia");
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

            // Error checking
            if (root == null)
            {
                richTextBoxOutput.Text = "Could not retrieve json from link. Ensure the URL is correct. Try removing the end slash if one exists.";
            }

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

            if (radioButtonSingles.Checked)
            {
                if (richTextBoxWinnersInput.Text != string.Empty)
                {
                    output += "==Winners Bracket==\r\n";
                    output += richTextBoxWinnersInput.Text;
                    FillBracketSingles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output);
                    output += "\r\n";
                }

                if (richTextBoxLosersInput.Text != string.Empty)
                {
                    output += "==Losers Bracket==\r\n";
                    output += richTextBoxLosersInput.Text;
                    FillBracketSingles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output);
                    output += "\r\n";
                }

                if (checkBoxFinalBracket.Checked)
                {
                    output += "==Final Singles Bracket==\r\n";
                    output += FillFinalBracketSingles();
                }
            }
            else
            {
                if (richTextBoxWinnersInput.Text != string.Empty)
                {
                    output += "==Winners Bracket==\r\n";
                    output += richTextBoxWinnersInput.Text;
                    FillBracketDoubles((int)numericUpDownWinnersStart.Value, (int)numericUpDownWinnersEnd.Value, (int)numericUpDownWinnersOffset.Value, ref output);
                    output += "\r\n";
                }

                if (richTextBoxLosersInput.Text != string.Empty)
                {
                    output += "==Losers Bracket==\r\n";
                    output += richTextBoxLosersInput.Text;
                    FillBracketDoubles(-(int)numericUpDownLosersStart.Value, -(int)numericUpDownLosersEnd.Value, (int)numericUpDownLosersOffset.Value, ref output);
                    output += "\r\n";
                }

                if (checkBoxFinalBracket.Checked)
                {
                    output += "==Final Doubles Bracket==\r\n";
                    output += FillFinalBracketDoubles();
                }
            }

            richTextBoxOutput.Text = output;
        }

        /// <summary>
        /// Retrieves a regex AKA database from Liquipedia, and parses it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAKA_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";

            // Decide on the URL to use
            string json = string.Empty;
            if (radioButtonSmash.Checked)
            {
                var responseStream = new GZipStream(client.OpenRead(SMASH_DB_URI), CompressionMode.Decompress);
                var reader = new StreamReader(responseStream);
                json = reader.ReadToEnd();
            }
            else if (radioButtonFighters.Checked)
            {
                var responseStream = new GZipStream(client.OpenRead(FIGHTERS_DB_URI), CompressionMode.Decompress);
                var reader = new StreamReader(responseStream);
                json = reader.ReadToEnd();
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
        #endregion

        #region JSON Parsing
        /// <summary>
        /// Get set list from json data
        /// </summary>
        /// <param name="root">json object</param>
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
                                    int temp = 0;
                                    int.TryParse(root.tournament.matches[i].match.scores_csv.Substring(0, index), out temp);
                                    root.tournament.matches[i].match.entrant1wins = temp;
                                    int.TryParse(root.tournament.matches[i].match.scores_csv.Substring(index + 1, root.tournament.matches[i].match.scores_csv.Length - index - 1), out temp);
                                    root.tournament.matches[i].match.entrant2wins = temp;
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

            // Sort all rounds by identifier (A->Z, then AA->ZZ)
            for (int i = 0; i < roundList.Count; i++)
            {
                roundList[roundList.ElementAt(i).Key] = roundList.ElementAt(i).Value.OrderBy(x => x.identifier.Length).ThenBy(x => x.identifier).ToList();
            }

            // Only perform the entrants check if rounds 1 and 2 exist for winners and losers
            if (roundList.ContainsKey(1) && roundList.ContainsKey(2) && roundList.ContainsKey(-1) && roundList.ContainsKey(-2))
            {
                // Insert byes into round 1 and -1 if round 1 doesn't have double the games of round 2
                int r1count = roundList[1].Count;
                if (roundList[1].Count != roundList[2].Count * 2)
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

                    // If l2 has less entries than l1count, that means all of l2 has l1 prerequisites
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
                                newMatch.round = -1;
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
                                        // Add a bye if the prereq is from the winners bracket
                                        if (root.tournament.matches[i].match.round > 0)
                                        {
                                            JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                            newMatch.player1_id = l2match.player1_id;
                                            newMatch.player2_id = PLAYER_BYE;
                                            newMatch.winner_id = l2match.player1_id;
                                            newMatch.loser_id = PLAYER_BYE;
                                            newMatch.round = -1;
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

                            // Add a bye match if the prereq is null
                            if (l2match.player2_prereq_match_id == 0)
                            {
                                JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                newMatch.player1_id = l2match.player2_id;
                                newMatch.player2_id = PLAYER_BYE;
                                newMatch.winner_id = l2match.player2_id;
                                newMatch.loser_id = PLAYER_BYE;
                                newMatch.round = -1;
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
                                        // Add a bye if the prereq is from the winners bracket
                                        if (root.tournament.matches[i].match.round > 0)
                                        {
                                            JSON_Structure.Match newMatch = new JSON_Structure.Match();

                                            newMatch.player1_id = l2match.player2_id;
                                            newMatch.player2_id = PLAYER_BYE;
                                            newMatch.winner_id = l2match.player2_id;
                                            newMatch.loser_id = PLAYER_BYE;
                                            newMatch.round = -1;
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
            //roundList = roundList.OrderBy(x => Math.Abs(x.Key)).ToDictionary(x => x.Key, x => x.Value);
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

        /// <summary>
        /// Get entrants list from json data
        /// </summary>
        /// <param name="root">json object</param>
        private void GetEntrants(ref RootObject root)
        {
            // Populate the entrant list using the json data
            for (int i = 0; i < root.tournament.participants.Count(); i++)
            {
                string inputName = root.tournament.participants[i].participant.name;
                List<Entrant> newParticipant = new List<Entrant>();

                if (inputName == string.Empty)
                {
                    // Fallback
                    inputName = root.tournament.participants[i].participant.display_name;
                }

                if (radioButtonDoubles.Checked && (textBoxSeparator.Text != string.Empty))
                {
                    Regex rgx = new Regex("(.*)" + Regex.Escape(textBoxSeparator.Text) + "(.*)");
                    System.Text.RegularExpressions.Match match = rgx.Match(inputName);

                    if (match.Success)
                    {
                        Entrant player1 = new Entrant(RemoveTags(match.Groups[1].Value).Trim());
                        Entrant player2 = new Entrant(RemoveTags(match.Groups[2].Value).Trim());
                        newParticipant.Add(player1);
                        newParticipant.Add(player2);
                    }
                    else
                    {
                        Entrant player1 = new Entrant(inputName.Trim());
                        Entrant player2 = new Entrant(string.Empty);
                        newParticipant.Add(player1);
                        newParticipant.Add(player2);
                    }
                }
                else
                {
                    Entrant player1 = new Entrant(RemoveTags(inputName));
                    newParticipant.Add(player1);
                }

                entrantList.Add(root.tournament.participants[i].participant.id, newParticipant);
            }

            // Bring up the player replacement window
            FormPlayerReplace formReplace = new FormPlayerReplace(ref entrantList, playerdb);
            this.Hide();
            formReplace.ShowDialog();
            this.Show();
        }
        #endregion

        /// <summary>
        /// Removes pipes from tags
        /// </summary>
        /// <param name="input">Input name</param>
        /// <returns>Name with pipes removed/replaced</returns>
        private string RemoveTags(string input)
        {
            // Remove tags where possible
            if (input.Contains("|"))
            {
                if (checkBoxTrimTags.Checked)
                {
                    int start = input.IndexOf("|") + 1;
                    input = input.Substring(start).Trim();
                }
                else
                {
                    input = input.Replace("|", "{{!}}");
                }
            }

            return input;
        }

        /// <summary>
        /// Gets the subdomain if one exists ("___.challonge.com" versus "challonge.com") and generates the proper tournament name
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

        #region Singles
        /// <summary>
        /// Fills bracket wikicode
        /// </summary>
        /// <param name="startRound">Round to start filling with</param>
        /// <param name="endRound">Round to stop filling with</param>
        /// <param name="offset">Number of rounds to offset by</param>
        /// <param name="bracketText">A string containing the bracket wikicode</param>
        private void FillBracketSingles(int startRound, int endRound, int offset, ref string bracketText)
        {
            int increment;
            string bracketSide;

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

                    FillSetSingles(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber, currentSet);
                }
            }
        }

        /// <summary>
        /// Fill one singles set
        /// </summary>
        /// <param name="bracketText">Base bracket text in wikicode</param>
        /// <param name="matchIdentifier">Round and match identifier (eg. "r1m1")</param>
        /// <param name="currentSet">Object of the set to input</param>
        private void FillSetSingles(ref string bracketText, string matchIdentifier, JSON_Structure.Match currentSet)
        {
            // Check for player byes
            if (currentSet.player1_id == PLAYER_BYE && currentSet.player2_id == PLAYER_BYE)
            {
                // If both players are byes, skip this entry
                return;
            }
            else if (currentSet.player1_id == PLAYER_BYE)
            {
                // Fill in player 1 as a bye if fill byes is checked
                if (checkBoxFillByes.Checked == true) FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1, "Bye");

                // Give player 2 a checkmark
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2, entrantList[currentSet.player2_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id][0].Flag);
                if (checkBoxFillByeWins.Checked == true)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "2");
                }
            }
            else if (currentSet.player2_id == PLAYER_BYE)
            {
                // Fill in player 2 as a bye
                if (checkBoxFillByes.Checked == true) FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2, "Bye");

                // Give player 1 a checkmark
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1, entrantList[currentSet.player1_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id][0].Flag);

                if (checkBoxFillByeWins.Checked == true)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "1");
                }
            }
            else
            {
                // Fill in the set normally
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1, entrantList[currentSet.player1_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id][0].Flag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2, entrantList[currentSet.player2_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id][0].Flag);


                // Check for DQs
                if (currentSet.entrant1wins == -1)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, "DQ");
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, LpStrings.Checkmark);
                }
                else if (currentSet.entrant2wins == -1)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, "DQ");
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, LpStrings.Checkmark);
                }
                else
                {
                    if (currentSet.round == roundList.Keys.Max() && currentSet.player1_prereq_match_id == currentSet.player2_prereq_match_id)
                    {
                        if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                        {
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, currentSet.entrant1wins.ToString());
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, currentSet.entrant2wins.ToString());
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, "{{win}}");
                            }
                            else if (currentSet.winner_id == currentSet.player2_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, "{{win}}");
                            }
                        }
                    }
                    else
                    {
                        if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                        {
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P1 + LpStrings.Score, "{{win}}");
                            }
                            else if (currentSet.winner_id == currentSet.player2_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.P2 + LpStrings.Score, "{{win}}");
                            }
                        }
                    }
                }

                // Set the winner
                if (currentSet.winner_id == currentSet.player1_id)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "1");
                }
                else if (currentSet.winner_id == currentSet.player2_id)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "2");
                }
            }
        }

        /// <summary>
        /// Fill a standard DE final singles bracket
        /// </summary>
        /// <returns>Final bracket text</returns>
        private string FillFinalBracketSingles()
        {
            // Error checking
            if (!checkBoxFinalBracket.Checked || roundList.Count == 0) return string.Empty;

            string finaltext = DEFINALSMWBRACKET;

            int playerCount = roundList[1].Count * 2;

            for (int i = 0; i < 10; i++)
            {
                if (Math.Pow(2, i) >= playerCount)
                {
                    // i is winners finals. We want grand finals, so add 1 to the round
                    int gfRound = i + 1;

                    if (roundList.ContainsKey(gfRound))
                    {
                        JSON_Structure.Match currentSet = roundList[gfRound][0];

                        if (currentSet.player1_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m1p1", entrantList[currentSet.player1_id][0].Gamertag);
                        if (currentSet.player1_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m1p1flag", entrantList[currentSet.player1_id][0].Flag);
                        if (currentSet.player1_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m1p1score", currentSet.entrant1wins.ToString());

                        if (currentSet.player2_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m1p2", entrantList[currentSet.player2_id][0].Gamertag);
                        if (currentSet.player2_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m1p2flag", entrantList[currentSet.player2_id][0].Flag);
                        if (currentSet.player2_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m1p2score", currentSet.entrant2wins.ToString());

                        // Check for a bracket reset. Entrant order is reversed on reset.
                        if (roundList[gfRound].Count == 2)
                        {
                            if (roundList[gfRound][1].player1_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m2p2score", roundList[gfRound][1].entrant1wins.ToString());
                            if (roundList[gfRound][1].player2_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m2p1score", roundList[gfRound][1].entrant2wins.ToString());

                            if (roundList[gfRound][1].winner_id == roundList[gfRound][1].player1_id) FillLPParameter(ref finaltext, "r3m1win", "2");
                            else if (roundList[gfRound][1].winner_id == roundList[gfRound][1].player2_id) FillLPParameter(ref finaltext, "r3m1win", "1");
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id) FillLPParameter(ref finaltext, "r3m1win", "1");
                            else if (currentSet.winner_id == currentSet.player2_id) FillLPParameter(ref finaltext, "r3m1win", "2");
                        }

                        // Find and fill the three preceding matches
                        JSON_Structure.Match wfSet = setIDsearch(currentSet.player1_prereq_match_id);
                        if (wfSet != null)
                        {
                            FillSetSingles(ref finaltext, "r1m1", wfSet);
                        }

                        JSON_Structure.Match lfSet = setIDsearch(currentSet.player2_prereq_match_id);
                        if (lfSet != null)
                        {
                            FillSetSingles(ref finaltext, "r2m1", lfSet);

                            JSON_Structure.Match lsSet = setIDsearch(lfSet.player2_prereq_match_id);
                            if (lsSet != null)
                            {
                                FillSetSingles(ref finaltext, "l1m1", lsSet);
                            }
                        }
                    }

                    break;
                }
            }

            return finaltext;
        }
        #endregion

        #region Doubles
        /// <summary>
        /// Fills bracket wikicode
        /// </summary>
        /// <param name="startRound">Round to start filling with</param>
        /// <param name="endRound">Round to stop filling with</param>
        /// <param name="offset">Number of rounds to offset by</param>
        /// <param name="bracketText">A string containing the bracket wikicode</param>
        private void FillBracketDoubles(int startRound, int endRound, int offset, ref string bracketText)
        {
            int increment;
            string bracketSide;

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

                    FillSetDoubles(ref bracketText, bracketSide + outputRound + LpStrings.Match + currentSet.matchnumber, currentSet);
                }
            }
        }

        /// <summary>
        /// Fill one doubles set
        /// </summary>
        /// <param name="bracketText">Base bracket text in wikicode</param>
        /// <param name="matchIdentifier">Round and match identifier (eg. "r1m1")</param>
        /// <param name="currentSet">Object of the set to input</param>
        private void FillSetDoubles(ref string bracketText, string matchIdentifier, JSON_Structure.Match currentSet)
        {
            // Check for player byes
            if (currentSet.player1_id == PLAYER_BYE && currentSet.player2_id == PLAYER_BYE)
            {
                // If both players are byes, skip this entry
                return;
            }
            else if (currentSet.player1_id == PLAYER_BYE)
            {
                // Fill in player 1 as a bye if fill byes is checked
                if (checkBoxFillByes.Checked == true)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P1, "Bye");
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P2, "Bye");
                }

                // Give player 2 a checkmark
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.player2_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player2_id][0].Flag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.player2_id][1].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id][1].Flag);
                if (checkBoxFillByeWins.Checked == true)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "2");
                }
            }
            else if (currentSet.player2_id == PLAYER_BYE)
            {
                // Fill in player 2 as a bye
                if (checkBoxFillByes.Checked == true)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P1, "Bye");
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P2, "Bye");
                }

                // Give player 1 a checkmark
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.player1_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id][0].Flag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.player1_id][1].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player1_id][1].Flag);

                if (checkBoxFillByeWins.Checked == true)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, LpStrings.Checkmark);
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "1");
                }
            }
            else
            {
                // Fill in the set normally
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P1, entrantList[currentSet.player1_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player1_id][0].Flag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P2, entrantList[currentSet.player1_id][1].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player1_id][1].Flag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P1, entrantList[currentSet.player2_id][0].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P1 + LpStrings.Flag, entrantList[currentSet.player2_id][0].Flag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P2, entrantList[currentSet.player2_id][1].Gamertag);
                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.P2 + LpStrings.Flag, entrantList[currentSet.player2_id][1].Flag);


                // Check for DQs
                if (currentSet.entrant1wins == -1)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, "DQ");
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, LpStrings.Checkmark);
                }
                else if (currentSet.entrant2wins == -1)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, "DQ");
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, LpStrings.Checkmark);
                }
                else
                {
                    if (currentSet.round == roundList.Keys.Max() && currentSet.player1_prereq_match_id == currentSet.player2_prereq_match_id)
                    {
                        if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                        {
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, currentSet.entrant1wins.ToString());
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, currentSet.entrant2wins.ToString());
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, "{{win}}");
                            }
                            else if (currentSet.winner_id == currentSet.player2_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, "{{win}}");
                            }
                        }
                    }
                    else
                    {
                        if (currentSet.entrant1wins != -99 && currentSet.entrant2wins != -99)
                        {
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, currentSet.entrant1wins.ToString());
                            FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, currentSet.entrant2wins.ToString());
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T1 + LpStrings.Score, "{{win}}");
                            }
                            else if (currentSet.winner_id == currentSet.player2_id)
                            {
                                FillLPParameter(ref bracketText, matchIdentifier + LpStrings.T2 + LpStrings.Score, "{{win}}");
                            }
                        }
                    }
                }

                // Set the winner
                if (currentSet.winner_id == currentSet.player1_id)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "1");
                }
                else if (currentSet.winner_id == currentSet.player2_id)
                {
                    FillLPParameter(ref bracketText, matchIdentifier + LpStrings.Win, "2");
                }
            }
        }

        /// <summary>
        /// Fill a standard DE final doubles bracket
        /// </summary>
        /// <returns>Final bracket text</returns>
        private string FillFinalBracketDoubles()
        {
            // Error checking
            if (!checkBoxFinalBracket.Checked || roundList.Count == 0) return string.Empty;

            string finaltext = DEFINALDOUBLESSMWBRACKET;

            int playerCount = roundList[1].Count * 2;

            for (int i = 0; i < 10; i++)
            {
                if (Math.Pow(2, i) >= playerCount)
                {
                    // i is winners finals. We want grand finals, so add 1 to the round
                    int gfRound = i + 1;

                    if (roundList.ContainsKey(gfRound))
                    {
                        JSON_Structure.Match currentSet = roundList[gfRound][0];

                        if (currentSet.player1_id != PLAYER_BYE)
                        {
                            FillLPParameter(ref finaltext, "r3m1t1p1", entrantList[currentSet.player1_id][0].Gamertag);
                            FillLPParameter(ref finaltext, "r3m1t1p1flag", entrantList[currentSet.player1_id][0].Flag);
                            FillLPParameter(ref finaltext, "r3m1t1p2", entrantList[currentSet.player1_id][1].Gamertag);
                            FillLPParameter(ref finaltext, "r3m1t1p2flag", entrantList[currentSet.player1_id][1].Flag);
                            FillLPParameter(ref finaltext, "r3m1t1score", currentSet.entrant1wins.ToString());
                        }

                        if (currentSet.player2_id != PLAYER_BYE)
                        {
                            FillLPParameter(ref finaltext, "r3m1t2p1", entrantList[currentSet.player2_id][0].Gamertag);
                            FillLPParameter(ref finaltext, "r3m1t2p1flag", entrantList[currentSet.player2_id][0].Flag);
                            FillLPParameter(ref finaltext, "r3m1t2p2", entrantList[currentSet.player2_id][1].Gamertag);
                            FillLPParameter(ref finaltext, "r3m1t2p2flag", entrantList[currentSet.player2_id][1].Flag);
                            FillLPParameter(ref finaltext, "r3m1t2score", currentSet.entrant2wins.ToString());
                        }

                        // Check for a bracket reset. Entrant order is reversed on reset.
                        if (roundList[gfRound].Count == 2)
                        {
                            if (roundList[gfRound][1].player1_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m2t2score", roundList[gfRound][1].entrant1wins.ToString());
                            if (roundList[gfRound][1].player2_id != PLAYER_BYE) FillLPParameter(ref finaltext, "r3m2t1score", roundList[gfRound][1].entrant2wins.ToString());

                            if (roundList[gfRound][1].winner_id == roundList[gfRound][1].player1_id) FillLPParameter(ref finaltext, "r3m1win", "2");
                            else if (roundList[gfRound][1].winner_id == roundList[gfRound][1].player2_id) FillLPParameter(ref finaltext, "r3m1win", "1");
                        }
                        else
                        {
                            if (currentSet.winner_id == currentSet.player1_id) FillLPParameter(ref finaltext, "r3m1win", "1");
                            else if (currentSet.winner_id == currentSet.player2_id) FillLPParameter(ref finaltext, "r3m1win", "2");
                        }

                        // Find and fill the three preceding matches
                        JSON_Structure.Match wfSet = setIDsearch(currentSet.player1_prereq_match_id);
                        if (wfSet != null)
                        {
                            FillSetDoubles(ref finaltext, "r1m1", wfSet);
                        }

                        JSON_Structure.Match lfSet = setIDsearch(currentSet.player2_prereq_match_id);
                        if (lfSet != null)
                        {
                            FillSetDoubles(ref finaltext, "r2m1", lfSet);

                            JSON_Structure.Match lsSet = setIDsearch(lfSet.player2_prereq_match_id);
                            if (lsSet != null)
                            {
                                FillSetDoubles(ref finaltext, "l1m1", lsSet);
                            }
                        }
                    }

                    break;
                }
            }

            return finaltext;
        }
        #endregion

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
        
        /// <summary>
        /// Searches for a specific set ID in the set list
        /// </summary>
        /// <param name="inputID">ID to find</param>
        /// <returns>Set with the matching ID</returns>
        private JSON_Structure.Match setIDsearch(int inputID)
        {
            foreach (List<JSON_Structure.Match> setList in roundList.Values)
            {
                foreach (JSON_Structure.Match set in setList)
                {
                    if (set.id == inputID)
                    {
                        return set;
                    }
                }
            }

            return null;
        }
        #endregion

        #region Player Database Methods
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

        /// <summary>
        /// Opens the relevant database when the database radio button is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion

        /// <summary>
        /// Enables or disables the separator text box when singles or doubles is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSinglesDoubles_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSingles.Checked)
            {
                textBoxSeparator.Enabled = false;
            }
            else
            {
                textBoxSeparator.Enabled = true;
            }
        }
    }
}

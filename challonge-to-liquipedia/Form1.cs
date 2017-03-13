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

namespace challonge_to_liquipedia
{
    public partial class FormMain : Form
    {
        static string BASE_URI = "https://api.challonge.com/v1/tournaments/";
        static string MATCH_URI = "/matches.json";
        static string PARTICIPANT_URI = "/participants.json";

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

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient { Credentials = new NetworkCredential(textBoxUsername.Text, textBoxPassword.Text) };
            string tournamentlink;

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
                //string output = client.DownloadString(new Uri(tournamentlink, MATCH_URI));
                string downloadURL = tournamentlink + PARTICIPANT_URI;
                string output = client.DownloadString(downloadURL);
                richTextBoxOutput.Text = output;

                downloadURL = tournamentlink + MATCH_URI;
                output = client.DownloadString(downloadURL);
                richTextBoxMatches.Text = output;
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
        }

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
    }
}

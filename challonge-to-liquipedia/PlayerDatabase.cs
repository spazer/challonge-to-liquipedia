﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia
{
    public class PlayerDatabase
    {
        private static string AKA_DB_FILENAME = @"AkaDatabase.json";
        private static string REVID_PATH = @"parse.revid";
        private static string WIKITEXT_PATH = @"parse.wikitext.*";

        private static string TEMPLATE_START = @"{{AltSlot";
        private static string TEMPLATE_END = @"}}";
        private static string TEMPLATE_PLAYER = @"|player=";
        private static string TEMPLATE_FLAG = @"|flag=";
        private static string TEMPLATE_ALTS = @"|alts=";

        public List<PlayerInfo> players;

        private int revID;

        public PlayerDatabase()
        {
            players = new List<PlayerInfo>();
            ReadDatabaseFromFile();
        }

        public int RevID
        {
            get { return revID; }
        }

        public bool ReadDatabaseFromFile()
        {
            // Clear the list
            players.Clear();

            // Read AKA database json if available
            string json = string.Empty;
            try
            {
                using (StreamReader file = new StreamReader(AKA_DB_FILENAME))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject obj = (JObject)JToken.ReadFrom(reader);

                        revID = obj.SelectToken(REVID_PATH).Value<int>();
                        json = obj.SelectToken(WIKITEXT_PATH).Value<string>();
                        ParseWikitext(json);

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void ParseWikitext(string json)
        {
            int i = 0;
            try
            {
                while (json.IndexOf(TEMPLATE_START, i) != -1 && json.IndexOf(TEMPLATE_END, i) != -1)
                {
                    int entrystart = json.IndexOf(TEMPLATE_START, i);
                    //string temp = json.Substring(entrystart, i);
                    int entryend = json.IndexOf(TEMPLATE_END, i) + TEMPLATE_END.Length;
                    //temp = json.Substring(entryend, i);

                    string segment = json.Substring(entrystart, entryend - entrystart);

                    int playerPos = segment.IndexOf(TEMPLATE_PLAYER);
                    int flagPos = segment.IndexOf(TEMPLATE_FLAG);
                    int altPos = segment.IndexOf(TEMPLATE_ALTS);

                    string player = string.Empty;
                    string flag = string.Empty;
                    string regex = string.Empty;

                    // Get player
                    if (playerPos != -1 && flagPos != -1)
                    {
                        player = segment.Substring(playerPos + TEMPLATE_PLAYER.Length, flagPos - playerPos - TEMPLATE_PLAYER.Length).Trim();
                    }

                    // Get flag
                    if (flagPos != -1 && altPos != -1)
                    {
                        flag = segment.Substring(flagPos + TEMPLATE_FLAG.Length, altPos - flagPos - TEMPLATE_FLAG.Length).Trim();
                    }

                    // Get alts
                    if (altPos != -1)
                    {
                        regex = segment.Substring(altPos + TEMPLATE_ALTS.Length, segment.Length - altPos - TEMPLATE_ALTS.Length - TEMPLATE_END.Length).Trim();
                    }

                    PlayerInfo newPlayer = new PlayerInfo(player, flag, regex);
                    players.Add(newPlayer);

                    i = entryend;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Error durign parsing of AKA database: " + ex.ParamName + " = " + ex.ActualValue);
                Console.WriteLine(ex.StackTrace);
            }
            catch
            {
                Console.WriteLine("Error during parsing of AKA database: " + i);
            }
        }
    }
}
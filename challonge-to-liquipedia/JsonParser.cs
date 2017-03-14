using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace challonge_to_liquipedia.JSON_Structure
{
    class JsonParser
    {
        public RootObject ParseTournament(string input)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            RootObject tournament = JsonConvert.DeserializeObject<RootObject>(input, settings);

            return tournament;
        }
    }
}

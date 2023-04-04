﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challonge_to_liquipedia.JSON_Structure
{
    public class Participant
    {
        public int id { get; set; }
        public int tournament_id { get; set; }
        public string name { get; set; }
        public int seed { get; set; }
        public bool active { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object invite_email { get; set; }
        public int final_rank { get; set; }
        public object misc { get; set; }
        public object icon { get; set; }
        public bool on_waiting_list { get; set; }
        public object invitation_id { get; set; }
        public object group_id { get; set; }
        public object checked_in_at { get; set; }
        public object challonge_username { get; set; }
        public object challonge_email_address_verified { get; set; }
        public bool removable { get; set; }
        public bool participatable_or_invitation_attached { get; set; }
        public bool confirm_remove { get; set; }
        public bool invitation_pending { get; set; }
        public string display_name_with_invitation_email_address { get; set; }
        public object email_hash { get; set; }
        public object username { get; set; }
        public string display_name { get; set; }
        public object attached_participatable_portrait_url { get; set; }
        public bool can_check_in { get; set; }
        public bool checked_in { get; set; }
        public bool reactivatable { get; set; }
        public List<object> group_player_ids { get; set; }
    }
}

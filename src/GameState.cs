namespace Nancy.Simple
{
    public class GameState
    {
        public string tournament_id { get; set; }
        public string game_id { get; set; }
        public int round { get; set; }
        public int bet_index { get; set; }
        public int small_blind { get; set; }
        public int current_buy_in { get; set; }
        public int pot { get; set; }
        public int minimum_raise { get; set; }
        public int dealer { get; set; }
        public int orbits { get; set; }
        public int in_action { get; set; }
        public player[] players { get; set; }
        public object[] community_cards { get; set; }

        public class player
        {
            public int id { get; set; }
            public string name { get; set; }
            public string status { get; set; }
            public string version { get; set; }
            public int stack { get; set; }
            public int bet { get; set; }
        }
    }
}

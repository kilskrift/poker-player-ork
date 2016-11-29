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
        public object[] players { get; set; }
        public object[] community_cards { get; set; }
    }
}

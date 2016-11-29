using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "Default C# folding player";


        public static int BetRequest(GameState gameState)
        {
            //var communityCards = gameState.
            var playerIndex = gameState.in_action;
            var player= gameState.players[playerIndex];
            //player
            //TODO: Use this method to return the value You want to bet
            return 150;
        }


        public static int BetRequest(JObject gameState)
        {
            //var communityCards = gameState.

            //TODO: Use this method to return the value You want to bet
            return 150;
        }

        
        public static void ShowDown(JObject gameState)
        {
            //TODO: Use this method to showdown
        }
    }
}


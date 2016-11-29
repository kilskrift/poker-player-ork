using System.Linq;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "Default C# folding player";


        public static int BetRequest(GameState gameState)
        {
            //var communityCards = gameState.
            var bet = 0;
            var playerIndex = gameState.in_action;
            var player = gameState.players[playerIndex];
            
            if (gameState.community_cards.Length >= 3)
            {

                var ourCards = player.hole_cards;
                var ourCardList = ourCards.ToList();
                ourCardList.AddRange(gameState.community_cards);

                var ourRanks = ourCardList.Select(y => y.rank);
                var ourGroups = ourRanks.GroupBy(y => y);


                if (gameState.community_cards.Length == 3)
                {
                    if (ourGroups.Count() == 2)
                    {
                        return 150;
                    }

                    if (ourGroups.Count() == 3)
                    {
                        return 10;
                    }
                    //
                    return 0;
                }

                if (gameState.community_cards.Length == 4)
                {
                    if (ourGroups.Count() == 2)
                    {
                        return 150;
                    }

                    if (ourGroups.Count() == 3)
                    {
                        return 150;
                    }

                    if (ourGroups.Count() == 4)
                    {
                        return 10;
                    }
                    //
                    return 0;
                }

                if (gameState.community_cards.Length == 5)
                {
                    if (ourGroups.Count() == 2)
                    {
                        return 150;
                    }

                    if (ourGroups.Count() == 3)
                    {
                        return 150;
                    }

                    if (ourGroups.Count() == 4)
                    {
                        return 100;
                    }
                    //
                    return 0;
                }

                return bet;
            }
            else
            {
                return bet;
            }
            //TODO: Use this method to return the value You want to bet
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


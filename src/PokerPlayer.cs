﻿using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "Default C# folding player";


        public static int BetRequest(GameState gameState)
        {
            var playerIndex = gameState.in_action;
            var player = gameState.players[playerIndex];
            Console.Error.WriteLine("Community_cards: " + gameState.community_cards.Length);

            var handManager = new HandManager();
            var communityAndhand = player.hole_cards.ToList();
            communityAndhand.AddRange(gameState.community_cards);
            var result = handManager.EvaluateHand(communityAndhand);
            

            return GreedyBet(gameState, player);
            //TODO: Use this method to return the value You want to bet
        }

        private static int GreedyBet(GameState gameState, GameState.player player)
        {
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
                        return 153;
                    }

                    if (ourGroups.Count() == 3)
                    {
                        return 13;
                    }
                    //
                    return 3;
                }

                if (gameState.community_cards.Length == 4)
                {
                    if (ourGroups.Count() == 2)
                    {
                        return 145;
                    }

                    if (ourGroups.Count() == 3)
                    {
                        return 114;
                    }

                    if (ourGroups.Count() == 4)
                    {
                        return 14;
                    }
                    //
                    return 4;
                }

                if (gameState.community_cards.Length == 5)
                {
                    if (ourGroups.Count() == 2)
                    {
                        return 155;
                    }

                    if (ourGroups.Count() == 3)
                    {
                        return 145;
                    }

                    if (ourGroups.Count() == 4)
                    {
                        return 105;
                    }
                    //
                    return 5;
                }

                return 1;
            }
            else
            {
                return gameState.minimum_raise;
            }
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


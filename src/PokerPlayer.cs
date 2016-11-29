﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
    public static class PokerPlayer
    {
        public static readonly string VERSION = "Default C# folding player";


        public static int BetRequest(GameState gameState)
        {
            try
            {
                Console.Error.WriteLine("Community_cards: " + gameState.community_cards.Length);
                var playerIndex = gameState.in_action;
                var player = gameState.players[playerIndex];
                var handManager = new HandManager();
                var communityAndhand = player.hole_cards.ToList();

                if (gameState.community_cards.Length >= 3)
                {
                    var rank = GetRanking(communityAndhand);
                    if (rank >= 4)
                    {
                        return 150;
                    }
                }

                communityAndhand.AddRange(gameState.community_cards);
                var result = handManager.EvaluateHand(communityAndhand);

                //TODO: Set bet:

                if (result.Hand == Hand.FourOfAKind)
                {
                    Console.Error.WriteLine("FourOfAKind, raise " + 201);
                    return 201;
                }

                if (result.Hand == Hand.ThreeOfAKind)
                {
                    Console.Error.WriteLine("ThreeOfAKind, raise " + 151);
                    return 151;
                }

                if (result.Hand == Hand.TwoPair)
                {
                    Console.Error.WriteLine("TwoPair, min raise " + gameState.minimum_raise);
                    return gameState.minimum_raise;
                }


                if (result.Hand == Hand.Pair && gameState.community_cards.Length >= 3)
                {
                    Console.Error.WriteLine("Pair, min raise " + gameState.minimum_raise);
                    return gameState.minimum_raise;
                }

                if (result.Hand == Hand.HighCard && gameState.community_cards.Length >= 3)
                {
                    Console.Error.WriteLine("Crap card-");
                    return 0;
                }

                return GreedyBet(gameState, player);
            }
            catch (Exception ex)
            {
                return GreedyBet(gameState, gameState.players[gameState.in_action]);
            }
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


        public static int GetRanking(List<GameState.HoleCard> cards)
        {
            string rainManURI = "http://rainman.leanpoker.org/rank";
            using (var webClient = new WebClient())
            {
                var cardsData = JsonConvert.SerializeObject(cards);
                var cardsDataWithPrefix = "cards=" + cardsData;
                webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rainManURI);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                string postData = cardsDataWithPrefix;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(postData);
                request.ContentLength = byte1.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);

                var response = request.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                var rankObject = JsonConvert.DeserializeObject<Ranking>(content);
                newStream.Close();
                response.Close();
                Console.Error.WriteLine("My ranking: " + rankObject.rank);
                return rankObject.rank;
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


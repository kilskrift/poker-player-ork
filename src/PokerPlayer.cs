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

                try
                {
                    var numberOfActivePlayers = gameState.players.Count(p => p.status == "active");
                    if (numberOfActivePlayers > 3)
                    {
                        //Play defense 
                        Console.Error.WriteLine("Playing defensively!");
                        return FoldAlways(gameState, gameState.players[gameState.in_action]);
                    }
                }
                catch (Exception ex)
                {
                     Console.Error.WriteLine("Exception in counting players "+ ex);
                }

                if (gameState.community_cards.Length >= 3)
                {
                    try
                    {
                        var cardString = "";
                        foreach (var card in communityAndhand)
                        {
                            cardString += " Rank: " + card + " Suit: " + card.suit;
                        }

                        Console.Error.WriteLine("Fetching rank! for " + cardString);

                        var rank = GetRanking(communityAndhand);

                        Console.Error.WriteLine("Ranking fetched!: " + rank);

                        if (rank >= 4)
                        {
                            return 150;
                        }

                    }


                    catch (Exception exception)

                    {
                        Console.Error.WriteLine("exception in GetRanking "+ exception);
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


                if (result.Hand == Hand.Pair)
                {
                    Console.Error.WriteLine("High card, check-" + gameState.minimum_raise);

                    return gameState.minimum_raise;
                }

                if (result.Hand == Hand.HighCard)
                {
                    Console.Error.WriteLine("Pair, min raise " + gameState.minimum_raise);
                    return gameState.minimum_raise;
                }

                if (result.Hand == Hand.Crap && player.hole_cards.Length > 1)
                {
                    FoldAlways(gameState, player);
                }

                return FoldAlways(gameState, player);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("exception, using FoldAlways Instead"+ ex);

                return FoldAlways(gameState, gameState.players[gameState.in_action]);
            }
            //TODO: Use this method to return the value You want to bet
        }

        private static int FoldAlways(GameState gameState, GameState.player player)
        {
            
            Console.Error.WriteLine("FoldAlways - fold always: ");
            return 0;
        }

        public static int Fold()
        {
            return 0;
        }


        public static int GetRanking(List<GameState.Card> cards)
        {
            if (cards.Count < 5)
            {
                return 0;
            }

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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple
{
    public class HandManager
    {
        public HandResult EvaluateHand(IEnumerable<GameState.HoleCard> cards)
        {
            var evaluatedCard = cards.Select(c => new EvaluatedCard(c));
            var maxFourOfAKind = FourOfAKind(evaluatedCard);

            if (maxFourOfAKind.Any())
            {
                return new HandResult() {Cards = maxFourOfAKind, Hand = Hand.FourOfAKind};
            }

            var maxThreeOfAKind = ThreeOfAKind(evaluatedCard);
            if (maxThreeOfAKind.Any())
            {
                return new HandResult() {Cards = maxFourOfAKind, Hand = Hand.ThreeOfAKind};
            }

            var maxPair = Pair(evaluatedCard);

            if (maxPair.Any())
            {
                return new HandResult() {Cards = maxPair, Hand = Hand.Pair};
            }

            var highCard = HighCard(evaluatedCard);

            if (highCard.Any())
            {
                return new HandResult() {Cards = highCard, Hand = Hand.HighCard};
            }

            return new HandResult()
            {
                Cards = new List<EvaluatedCard>() {evaluatedCard.OrderByDescending(c => c.RankValue).First()},
                Hand = Hand.HighCard
            };
        }

        private IEnumerable<EvaluatedCard> FourOfAKind(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var orderedPairGroups = orderedRankGroups.Where(g => g.ToList().Count == 4);

            if (orderedPairGroups.Any())
            {
                var maxFourOfAKind = orderedPairGroups.First();
                return maxFourOfAKind.ToList();
            }

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> ThreeOfAKind(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var orderedPairGroups = orderedRankGroups.Where(g => g.ToList().Count == 3);

            if (orderedPairGroups.Any())
            {
                var maxThreeOfAKind = orderedPairGroups.First();
                return maxThreeOfAKind.ToList();
            }

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> Pair(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var orderedPairGroups = orderedRankGroups.Where(g => g.ToList().Count == 2);

            if (orderedPairGroups.Any())
            {
                var maxPair = orderedPairGroups.First();
                return maxPair.ToList();
            }

            return new List<EvaluatedCard>();
        }

        private IEnumerable<EvaluatedCard> HighCard(IEnumerable<EvaluatedCard> cards)
        {
            var orderedRankGroups = cards.GroupBy(g => g.RankValue).OrderByDescending(g => g.Key);
            var highestCard = cards.Max(y => y.RankValue);

            if (highestCard >= 11)
            {
                var highCard = orderedRankGroups.First();
                return highCard.ToList();
            }
            return new List<EvaluatedCard>();
        }
    }

    public class HandResult
        {
            public IEnumerable<EvaluatedCard> Cards { get; set; }
            public Hand Hand { get; set; }
        }
    }

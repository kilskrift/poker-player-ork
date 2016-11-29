using System;

namespace Nancy.Simple
{
    public class EvaluatedCard
    {
        private readonly int _rankValue;
        private readonly string _suitValue;
        public EvaluatedCard(GameState.HoleCard card)
        {
            _rankValue = ParseRank(card.rank);
            _suitValue = card.suit;
        }

        public string Suit
        {
            get
            {
                return _suitValue;
            }
        }

        private int ParseRank(string rank)
        {
            switch (rank)
            {
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                    return Int32.Parse(rank);
                case "J":
                    return 11;
                case "Q":
                    return 12;
                case "K":
                    return 13;
                case "A":
                    return 14;
                default:
                    return -1;
            }
        }

        public int RankValue
        {
            get
            {
                return _rankValue;
            }
        }

    }
}

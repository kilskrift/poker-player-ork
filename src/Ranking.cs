using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple
{
    public class Ranking
    {
        public int rank { get; set; }
        public int value { get; set; }
        public int second_value { get; set; }
        public List<int> kickers { get; set; }
        public List<CardsUsed> cards_used { get; set; }
        public List<GameState.HoleCard> cards { get; set; }
    }

    public class CardsUsed
    {
        public string rank { get; set; }
        public string suit { get; set; }
    }
}

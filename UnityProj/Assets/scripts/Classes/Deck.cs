using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts.Classes
{
    public class Deck
    {
        public List<Card> cards;
        public List<Card> dealtCards;
        public bool isFaceDown = true;

        public Deck(List<Card> c)
        {
            cards = c;
        }
    }
}

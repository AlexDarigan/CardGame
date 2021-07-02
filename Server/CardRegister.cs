using System.Collections;
using System.Collections.Generic;

namespace CardGame.Server
{
    public class CardRegister : IEnumerable<Card>
    {
        /*
         * Our card register keeps a list of cards in the current game. In the player.LoadDeck function we get
         * the current count of the Cards and set it as the Id of the next card to be added. This means the card id,
         * like a list index, will always be -1 the card count after the card has been added (so if we add a card
         * it will be at index 0 with an id of 0 in a list of count 1)
         */
        private readonly List<Card> Cards = new();
        public int Count => Cards.Count;
        public Card this[int i] => Cards[i];

        public IEnumerator<Card> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Card card)
        {
            Cards.Add(card);
        }
    }
}
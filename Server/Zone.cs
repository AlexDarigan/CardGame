using System.Collections;
using System.Collections.Generic;

namespace CardGame.Server
{
    public class Zone: IEnumerable<Card>
    {
        private readonly List<Card> _cards = new List<Card>();

        public int Count => _cards.Count;
        public Zone(){}
        public void Add(Card card) => _cards.Add(card);
        public void Remove(Card card) => _cards.Remove(card);
        public bool Contains(Card card) => _cards.Contains(card);
        public Card this[int index] => _cards[index];
        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }
}
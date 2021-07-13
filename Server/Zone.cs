using System.Collections;
using System.Collections.Generic;

namespace CardGame.Server
{
    public class Zone : IEnumerable<Card>
    {
        private List<Card> Cards { get; } = new();

        public int Count => Cards.Count;
        public Card this[int index] => Cards[index];

        public IEnumerator<Card> GetEnumerator() { return Cards.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public void Add(Card card) { Cards.Add(card); }
        public void Remove(Card card) { Cards.Remove(card); }
        public bool Contains(Card card) { return Cards.Contains(card); }
        public int FindIndex(Card card) { return Cards.FindIndex(c => c == card); }
    }
}
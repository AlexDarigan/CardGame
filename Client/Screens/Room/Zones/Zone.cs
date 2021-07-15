using System.Collections;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class Zone : Spatial, IEnumerable<Card>
    {
        protected List<Card> Cards { get; } = new();
        [Export()] public Vector3 OffSet { get; set; }
        public Zone() { }

        public int Count => Cards.Count;
        public Card this[int index] => Cards[index];

        public IEnumerator<Card> GetEnumerator() { return Cards.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public virtual void Add(Card card)
        {
            AddChild(card);
            Cards.Add(card);
            card.Translate(OffSet * Cards.Count);
            card.CurrentZone = this;
            card.RotationDegrees = new Vector3(0, 0, 180);
            Translate(new Vector3(-2, 0, 0));
        }

        public virtual void Insert(int index, Card card)
        {
            Cards.Insert(index, card);
            card.CurrentZone = this;
        }

        public virtual void Remove(Card card)
        {
            RemoveChild(card);
            Cards.Remove(card);
            card.CurrentZone = null;
            Translate(new Vector3(1, 0, 0));
        }
    }
}

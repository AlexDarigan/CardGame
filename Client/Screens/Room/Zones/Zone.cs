using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Zone : Spatial, IEnumerable<Card>
    {
        private List<Card> Cards { get; } = new();
        private List<Location> Locations { get; } = new ();
        [Export()] public Vector3 OffSet { get; set; }

        public Zone() { }
        public int Count => Cards.Count;
        public Card this[int index] => Cards[index];
        public IEnumerator<Card> GetEnumerator() { return Cards.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public void Add(Card card)
        {
            card.CurrentZone = this;
            Cards.Add(card);
            UpdatePositions();
        }

        public void Insert(int index, Card card)
        {
            Cards.Insert(index, card);
            card.CurrentZone = this;
            UpdatePositions();
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            card.CurrentZone = null;
            UpdatePositions();
        }
        
        private void UpdatePositions()
        {
            Locations.Clear();
            foreach (Card card in Cards)
            {
                // Initial Location of Card and then adjust for all cards in the zone
                // (Might be easier to use a physical slot system?)
                Vector3 destination = (Translation + OffSet * Locations.Count) - OffSet * (Cards.Count - Locations.Count);
                Location location = new(destination, RotationDegrees);
                Locations.Add(location);
                card.Location = location;
            }
        }
    }
    

    public readonly struct Location
    {
        public readonly Vector3 RotationDegrees;
        public readonly Vector3 Translation;

        public Location(Vector3 translation, Vector3 rotationDegrees)
        {
            Translation = translation;
            RotationDegrees = rotationDegrees;
        }
    }
}

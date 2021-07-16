using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Zone : Spatial, IEnumerable<Card>
    {
        private List<Card> Cards { get; } = new();
        public List<Location> Locations { get; } = new ();
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
                // Place Location at our translation + offset per location (ie card) in zone
                Locations.Add(new Location(Translation + OffSet * Locations.Count, OffSet, RotationDegrees, card));
                
                // Shift all locations left after adding a new one
                // 1. Add Location 0
                // 2. Shift Location 0 Left
                // 3. Add Location 1
                // 4. Shift Location 0 Left
                // 5. Shift Location 1 Left
                // 6. Add Location 2
                // 7. Shift Location 0 Left
                // 8. Shift Location 1 Left
                // 9. Shift Location 2 Left
                foreach (Location location in Locations) { location.ShiftLeft(); }
            }
        }
    }
    

    public class Location
    {
        private Vector3 OffSet { get; }
        public Vector3 RotationDegrees { get; }
        public Vector3 Translation { get; private set; }
        public Card Card { get; }

        public Location(Vector3 translation, Vector3 offSet, Vector3 rotationDegrees, Card card)
        {
            Translation = translation;
            OffSet = offSet;
            RotationDegrees = rotationDegrees;
            Card = card;
        }

        public void ShiftLeft() { Translation -= OffSet; }
    }
}

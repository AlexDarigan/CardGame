using System.Collections;
using System.Collections.Generic;
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
            Location location = new (Translation + OffSet * Locations.Count, OffSet, RotationDegrees) {Card = card};
            //card.CurrentLocation = location;
            card.CurrentZone = this;
            Cards.Add(card);
            Locations.Add(location);
           // Reset();
            ShiftLeft();
        }

        public void Insert(int index, Card card)
        {
            Cards.Insert(index, card);
            Reset();
            card.CurrentZone = this;
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            card.CurrentZone = null;
            RemoveEmptyLocation();
            ShiftRight();
        }

        private void Reset()
        {
            Locations.Clear();
            foreach (Card c in Cards)
            {
                Locations.Add(new Location(Translation + OffSet * Locations.Count, OffSet, RotationDegrees) {Card = c});
            }
        }
        
    
        private void RemoveEmptyLocation()
        {
            int index = 0;
            while (index < Locations.Count && Cards.Contains(Locations[index].Card)) { index++; }
            for (int i = index; i < Locations.Count - 1; i++)
            {
                Location location = Locations[i];
                location.Card = Locations[i + 1].Card;
                location.Card.CurrentLocation = location;
            }
            
            Locations.RemoveAt(Locations.Count - 1);
        }

        private void ShiftRight() { foreach (Location location in Locations) { location.ShiftRight(); } }
        private void ShiftLeft() { foreach (Location location in Locations) { location.ShiftLeft(); } }

        public virtual void Update(Room room)
        {
            const float duration = .2f;
            foreach (Location location in Locations)
            {
                room.Effects.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(location.Card, nameof(Card.RotationDegrees), location.Card.RotationDegrees, location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }
    }
    

    public class Location
    {
        private Vector3 OffSet { get; }
        public Vector3 RotationDegrees { get; }
        public Vector3 Translation { get; private set; }
        public Card Card { get; set; }

        public Location(Vector3 translation, Vector3 offSet, Vector3 rotationDegrees)
        {
            Translation = translation;
            OffSet = offSet;
            RotationDegrees = rotationDegrees;
        }

        public void ShiftRight() { Translation += OffSet; }
        public void ShiftLeft() { Translation -= OffSet; }
    }
}

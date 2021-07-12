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
            card.CurrentLocation = location;
            card.CurrentZone = this;
            Cards.Add(card);
            Locations.Add(location);
            ShiftLeft();
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            card.CurrentZone = null;
            card.CurrentLocation.Card = null; 
            RemoveEmptyLocation();
            ShiftRight();
        }
        
    
        private void RemoveEmptyLocation()
        {
            int index = 0;
            while (Locations[index].Card is not null) { index++; }
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

        public void Update(Room room)
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

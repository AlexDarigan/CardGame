using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class Zone: Spatial, IEnumerable<Card>
    {
        private readonly List<Card> _cards = new();
        private readonly List<Location> _locations = new();
        public Spatial Top => GetChild<Spatial>(Count);

        public int Count => _cards.Count;
        public Location NextLocation => _locations[Count - 1];

        public override void _Ready()
        {
            foreach (Spatial location in GetChildren()) { _locations.Add(new Location(location.Translation, location.RotationDegrees)); }
        }

        public Location Add(Card card)
        {
            _cards.Add(card);
            return _locations[Count - 1];
        }

        public Location Remove(Card card)
        {
            Location location = _locations[_cards.Count - 1];
            _cards.Remove(card);
            return location;
        }
        
        public Card this[int index] => _cards[index];
        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Location
    {
        public readonly Vector3 Translation;
        public readonly Vector3 RotationDegrees;

        public Location(Vector3 translation, Vector3 rotationDegrees)
        {
            Translation = translation;
            RotationDegrees = rotationDegrees;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Zone : Object, IEnumerable<Card>
    {
        private readonly List<Card> _cards = new();
        public readonly IReadOnlyList<Location> Locations;

        public Zone(Node view)
        {
            Locations = (from Spatial location in view.GetChildren()
                select new Location(location.Translation, location.RotationDegrees)).ToList();
        }

        public int Count => _cards.Count;
        public Location Destination => Locations[_cards.Count - 1];
        public Card this[int index] => _cards[index];

        public IEnumerator<Card> GetEnumerator() { return _cards.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public void Add(Card card) { _cards.Add(card); }
        public void Remove(Card card) { _cards.Remove(card); }
    }

    public class Location
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
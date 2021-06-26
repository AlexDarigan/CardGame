using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
    public class Zone: Godot.Object, IEnumerable<Card>
    {
        private readonly List<Card> _cards = new();
        public readonly IReadOnlyList<Location> Locations;
        public int Count => _cards.Count;
        public Location Destination => Locations[_cards.Count - 1];
        
        public Zone(Node view) => Locations = (from Spatial location in view.GetChildren() select new Location(location.Translation, location.RotationDegrees)).ToList();

        public void Add(Card card)
        {
            _cards.Add(card);
            //return Locations[Count - 1];
        }

        public void Remove(Card card) => _cards.Remove(card);
        public Card this[int index] => _cards[index];
        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class Zone: Godot.Object, IEnumerable<Card>
    {
        private readonly Node _view;
        private readonly List<Card> _cards = new();
        private readonly List<Location> _locations = new();
        public int Count => _cards.Count;
        public Location NextLocation => _locations[Count - 1];

        public Zone(Node view)
        {
            _view = view;
            foreach (Spatial location in _view.GetChildren()) { _locations.Add(new Location(location.Translation, location.RotationDegrees)); }
        }
        
        public Location Add(Card card)
        {
            _view.GetChild<Spatial>(Count).Visible = true;
            _cards.Add(card);
            return _locations[Count - 1];
        }

        public Location Remove(Card card)
        {
            Location location = _locations[_cards.Count - 1];
            _cards.Remove(card);
            _view.GetChild<Spatial>(Count).Visible = false;
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
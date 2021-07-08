using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
    public class Zone : Object, IEnumerable<Card>
    {
        private List<Card> Cards { get; set; } = new();
        public List<Location> Locations { get; set; } = new ();
        private Vector3 OffSet { get; set; }= Vector3.Zero;
        private Vector3 Origin { get; set; }= Vector3.Zero;
        private Vector3 Rotation { get; set; } = Vector3.Zero;
        
        public Zone(string name, bool isPlayer)
        {
            switch (name)
            {
                case "Deck":
                    OffSet = new Vector3(0, 0.034f, 0);
                    Origin = isPlayer ? new Vector3(8, 0, 7.5f) : new Vector3(8, 0, -7.5f);
                    Rotation = isPlayer ? new Vector3(0, 0, 180) : new Vector3(0, 180, 180);
                    break;
                case "Hand":
                    OffSet = new Vector3(1.1f, 0, 0);
                    Origin = isPlayer ? new Vector3(0, 4, 11) : new Vector3(0, 6, -7.5f);
                    Rotation = isPlayer ? new Vector3(33, 0, 0) : new Vector3(33, 0, 180);
                    break;
                case "Units":
                    OffSet = new Vector3(1.1f, 0.0f, 0);
                    Origin = isPlayer ? new Vector3(0, 0.5f, 3) : new Vector3(0, 0.5f, -3);
                    Rotation = new Vector3(0, 0, 0);
                    break;
                case "Support":
                    OffSet = new Vector3(1.1f, 0.0f, 0);
                    Origin = isPlayer ? new Vector3(0, 0.5f, 7) : new Vector3(0, 0.5f, -7);
                    Rotation = new Vector3(0, 0, 180);
                    break;
                case "Discard":
                    OffSet = new Vector3(0, 0.04f, 0);
                    Origin = isPlayer ? new Vector3(8, 0, 2.5f) : new Vector3(8, 0.5f, -2.5f);
                    Rotation = new Vector3(0, 0, 0);
                    break;
            }
        }

        public int Count => Cards.Count;
        public Location Destination => Locations[Cards.Count - 1];
        public Card this[int index] => Cards[index];
        public bool Contains(Card card) => Cards.Contains(card);
        public IEnumerator<Card> GetEnumerator() { return Cards.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public void Add(Card card)
        {
            Location location = new (Origin + OffSet * Locations.Count, OffSet, Rotation) {Card = card};
            card.CurrentLocation = location;
            Cards.Add(card);
            Locations.Add(location);
            ShiftLeft();
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            card.CurrentLocation.Card = null; 
            ShiftRight();
        }
        
    
        private void ShiftRight()
        {
            // Seek out the location where our card has become null
            for (int i = 0; i < Locations.Count; i++)
            {
                if (Locations[i].Card is null)
                {
                    for (int index = i; index < Locations.Count - 1; index++)
                    {
                        Location location = Locations[index];
                        location.Card = Locations[index + 1].Card;
                        location.Card.CurrentLocation = location;
                    }
                }
                
                Locations[i].ShiftRight();
            }
            
            Locations.RemoveAt(Locations.Count - 1);
        }

        private void ShiftLeft() { foreach (Location location in Locations) { location.ShiftLeft(); } }
        
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

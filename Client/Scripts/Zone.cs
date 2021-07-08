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
            
            // bool isPlayer = view.GetParent().Name == "Player";
            switch (name)
            {
                case "Deck":
                    // Do we need scaling?
                    OffSet = new Vector3(0, 0.03f, 0);
                    Origin = isPlayer ? new Vector3(8, 0, 7.5f) : new Vector3(8, 0, -7.5f);
                    Rotation = isPlayer ? new Vector3(0, 0, 180) : new Vector3(0, 180, 180);
                    break;
                case "Hand":
                    //OffSet = new Vector3(2.25f, 0.03f, 0);
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
                    OffSet = new Vector3(0, 0.03f, 0);
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
            Cards.Add(card);
            Vector3 lOffset = Vector3.Back;
            Location location = new Location(Origin, OffSet, Rotation, Locations.Count, card);
            card.CurrentLocation = location;
            Locations.Add(location);
            ShiftLeft();
        }

        public void Remove(Card card)
        {
            Cards.Remove(card);
            // This does set it to null but then shift right isn't working properly?
            // Seems this may be a problem only when opponent is facing it (does this card exist?)
            card.CurrentLocation.Card = null; 
            ShiftRight();
        }
        
        // Console.WriteLine("Changing Locations!");
        // // DownShift all available cards to next available locations
        // // Then remove the top locations
        private void ShiftRight()
        {
            // Seek out the location where our card has become null
            int index = 0;
            for (int i = 0; i < Locations.Count; i++)
            {
                if (Locations[i].Card is null)
                {
                    index = i;
                    break;
                }
            }

            // Move card references down the list
            for (int i = index; i < Locations.Count - 1; i++)
            {
                Locations[i].Card = Locations[i + 1].Card;
                Locations[i].Card.CurrentLocation = Locations[i];
            }

            // Remove the last element (which should be the only null element)
            Locations.RemoveAt(Locations.Count - 1);

            // Update translations
            foreach (Location location in Locations)
            {
                location.Card.CurrentLocation = location;
                float x = location.Translation.x + OffSet.x;
                float y = location.Translation.y + OffSet.y;
                float z = location.Translation.z + OffSet.z;
                location.Translation = new Vector3(x, y, z);
            }
        }

        public void ShiftLeft()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Location location = Locations[i];
                if (location.Index != i) { location.Index = i; }
            }
            foreach (Location location in Locations)
            {
                float x = location.Translation.x - OffSet.x;
                float y = location.Translation.y - OffSet.y; 
                float z = location.Translation.z - OffSet.z;
                location.Translation = new Vector3(x, y, z);
            }
        }
        
    }

    public class Location
    {
        public Vector3 Origin { get; set; }
        public Vector3 RotationDegrees { get; set; }
        public Vector3 Translation { get; set; }
        public int Index { get; set; }
        public Card Card { get; set; }

        public Location(Vector3 origin, Vector3 translation, Vector3 rotationDegrees, int index, Card card)
        {
            Origin = origin;
            // Generate where our offset from origin using the number of cards we have
            float x = translation.x * index;
            float y = translation.y * index;
            float z = translation.z * index;
            
            // Set our translation to the sum of origin and calculated offset
            Translation = new Vector3(origin.x + x, origin.y + y, origin.z + z);
            RotationDegrees = rotationDegrees;
            Index = index;
            Card = card;
        }
        
        
    }
}

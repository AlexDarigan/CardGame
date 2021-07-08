using System;
using Godot;

namespace CardGame.Client
{
    public class Participant
    {
        public int Health = 8000;
        public Zone Deck { get; protected set; }
        public Zone Discard { get; protected set; }
        public Zone Hand { get; protected set; }
        public Zone Supports { get; protected set; }
        public Zone Units { get; protected set; }
        public Participant() { }
        
        
    }
}
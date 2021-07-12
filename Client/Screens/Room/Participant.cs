using CardGame.Client.Views;
using Godot;

namespace CardGame.Client
{
    public class Participant: Node
    {
        public int Health = 8000;
        public Zone Deck { get; protected set; }
        public Zone Discard { get; protected set; }
        public Zone Hand { get; protected set; }
        public Zone Supports { get; protected set; }
        public Zone Units { get; protected set; }
        public Avatar Avatar { get; protected set; }
        public HealthBar HealthBar { get; set; }
        protected Participant() { }

        public override void _Ready()
        {
            Deck = GetNode<Zone>("Deck");
            Discard = GetNode<Zone>("Discard");
            Hand = GetNode<Zone>("Hand");
            Units = GetNode<Zone>("Units");
            Supports = GetNode<Zone>("Supports");
            Avatar = GetNode<Avatar>("Avatar");
            HealthBar = GetNode<HealthBar>("HealthBar");
        }
    }
}
using Godot;

namespace CardGame.Client
{
    public class Rival: Participant
    {
        public Rival() { }
        public override void _Ready()
        {
            Deck = GetNode<Zone>("Deck");
            Discard = GetNode<Zone>("Discard");
            Hand = GetNode<Zone>("Hand");
            Units = GetNode<Zone>("Units");
            Supports = GetNode<Zone>("Supports");
        }
    }
}
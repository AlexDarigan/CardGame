using CardGame.Client.Views;
using Godot;

namespace CardGame.Client
{
    public class Participant: Node
    {
        [Export()] public bool IsClient { get; private set; }
        private HealthBar Health { get; set; }
        public Zone Deck { get; protected set; }
        public Zone Discard { get; protected set; }
        public Zone Hand { get; protected set; }
        public Zone Supports { get; protected set; }
        public Zone Units { get; protected set; }
        public Avatar Avatar { get; protected set; }
        private HealthBar HealthBar { get; set; }
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

        public void SetHealth(int value, Room room)
        {
            // Should we just send this directly to the HealthBar itself?
            // Alternative we could send the HealthBar to effects?
            HealthBar.DisplayHealth(value, this, room);
        }
    }
}
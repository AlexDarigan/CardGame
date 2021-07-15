using System.Globalization;
using Godot;

namespace CardGame.Client
{
    public class Participant: Node
    {
        [Export()] public bool IsClient { get; private set; }
        private int Health { get; set;  } = 8000;
        public Zone Deck { get; private set; }
        public Zone Graveyard { get; private set; }
        public Zone Hand { get; private set; }
        public Zone Supports { get; private set; }
        public Zone Units { get; private set; }
        public Avatar Avatar { get; private set; }
        private TextureProgress HealthBar { get; set; }
        private Label LifeChange { get; set; }
        private Label LifeCount { get; set; }
        protected Participant() { }

        public override void _Ready()
        {
            Deck = GetNode<Zone>("Deck");
            Graveyard = GetNode<Zone>("Discard");
            Hand = GetNode<Zone>("Hand");
            Units = GetNode<Zone>("Units");
            Supports = GetNode<Zone>("Supports");
            Avatar = GetNode<Avatar>("Avatar");
            HealthBar = GetNode<TextureProgress>("HealthBar");
            LifeChange = GetNode<Label>("LifeChange");
            LifeCount = GetNode<Label>("HealthBar/Count");
            if (!IsClient) { GetNode<Label>("HealthBar/Name").Text = "Rival"; }
        }

        public void SetHealth(int newHealth, Room room)
        {
            int difference = Health - newHealth;
            LifeChange.Text = difference.ToString();
            LifeChange.Visible = true;
            room.Effects.InterpolateCallback(LifeChange, 0.4f, "set_visible", false);
            room.Effects.InterpolateProperty(HealthBar, "value", HealthBar.Value, newHealth, .5f);
            LifeCount.Text = newHealth.ToString(CultureInfo.InvariantCulture);
            Health = newHealth;
        }

        public void LoadDeck() { }
        public void Draw(Card card) { }
        public void Deploy(Card card) { }
        public void SetFaceDown(Card card) { }
        public void Activate(Card card) { }
        public void AttackUnit(Card attacker, Card defender) { }
        public void AttackPlayer(Card attacker) { }
        

        
    }
}
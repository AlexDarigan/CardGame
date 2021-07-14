using Godot;

namespace CardGame.Client
{
    public class CardType: CardProperty<CardTypes>
    {
        private Spatial Power { get; }
        protected override CardTypes Property { get; set; }
        public override CardTypes Value
        {
            get => Property;
            set
            {
                Property = value;
                Power.Visible = value == CardTypes.Unit;
            }
        }

        public CardType(Node card)
        {
            Power = card.GetNode<Spatial>("Power");
        }
    }
}
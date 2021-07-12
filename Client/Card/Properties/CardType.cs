using System;
using Godot;

namespace CardGame.Client.Views
{
    public class CardType: CardProperty<CardTypes>
    {
        private Spatial Power { get; set; }
        
        public CardType(Card card)
        {
            Card = card;
            OnReady();
            async void OnReady()
            {
                await card.ToSignal(card, "ready");
                Power = card.GetNode<Spatial>("Power");
            }
        }

        public override void Set(CardTypes value)
        {
            Value = value;
            Console.WriteLine(value.GetType());
            Console.WriteLine(value);
            Power.Visible = value == CardTypes.Unit;
        }
        
        public override CardTypes Get() { return Value; }
    }
}
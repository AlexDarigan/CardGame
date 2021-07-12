using System;

namespace CardGame.Client.Views
{
    public class CardState: CardProperty<CardStates>
    {
        public CardState(Card card) { Card = card;}

        public override void Set(CardStates value) { Value = value; }
        public override CardStates Get()
        {
            return Value;
        }
    }
}
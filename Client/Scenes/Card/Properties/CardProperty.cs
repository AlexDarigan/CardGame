using System;

namespace CardGame.Client.Views
{
    public abstract class CardProperty<T>
    {
        protected T Value { get; set; }
        protected Card Card { get; set; }
        protected CardProperty(){}
        protected CardProperty(Card card) { Card = card; }
        public abstract void Set(T value);
        public abstract T Get();
    }
}
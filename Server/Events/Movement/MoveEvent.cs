namespace CardGame.Server.Events
{
    public abstract class MoveEvent: Event
    {
        protected Card Card { get; }
        protected Player Controller { get; }
        protected MoveEvent(){}
        protected MoveEvent(Card card)
        {
            Controller = card.Controller;
            Card = card;
        }
    }
}
namespace CardGame.Server
{
    public class DrawEvent : Event
    {
        private readonly Card Card;
        private readonly Player Controller;

        public DrawEvent(Card card)
        {
            Command = CommandId.Draw;
            Controller = card.Controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, isClient, Card.Id);
            queue(Controller.Opponent.Id, Command, !isClient, -1);
        }
    }
}
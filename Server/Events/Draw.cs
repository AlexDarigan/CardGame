namespace CardGame.Server.Events
{
    public class Draw : Event
    {
        private readonly Card Card;
        private readonly Player Controller;

        public Draw(Card card)
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
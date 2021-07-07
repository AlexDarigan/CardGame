namespace CardGame.Server.Events
{
    public class Draw : Event
    {
        private Card Card { get; }
        private Player Controller { get; }

        public Draw(Card card)
        {
            Command = CommandId.Draw;
            Controller = card.Controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, IsClient, Card.Id);
            queue(Controller.Opponent.Id, Command, !IsClient, -1);
        }
    }
}
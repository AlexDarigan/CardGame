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
            queue(Controller.Id, CommandId.MoveCard, Who.Player, Card.Id, Card.SetCodes, Zones.Deck, Zones.Hand, 0, 0);
            queue(Controller.Opponent.Id, Command, Who.Rival, -1);
        }
    }
}
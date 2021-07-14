namespace CardGame.Server.Events
{
    public class Deploy : Event
    {
        private Card Card { get; }
        private Player Controller { get; }

        public Deploy(Player controller, Card card)
        {
            Command = CommandId.Deploy;
            Controller = controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.MoveCard, Who.Player, Card.Id, Card.SetCodes, Zones.Hand, Zones.Units, 0, 0);
            queue(Controller.Opponent.Id, CommandId.MoveCard, Who.Rival, Card.Id, Card.SetCodes, Zones.Hand, Zones.Units, 0, 0);
        }
    }
}
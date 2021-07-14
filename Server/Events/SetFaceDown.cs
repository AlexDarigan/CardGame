namespace CardGame.Server.Events
{
    public class SetFaceDown : Event
    {
        private Card Card { get; }
        private Player Controller { get; }

        public SetFaceDown(Player controller, Card card)
        {
            Command = CommandId.SetFaceDown;
            Controller = controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.MoveCard, Who.Player, Card.Id, Card.SetCodes, Zones.Hand, Zones.Supports, 0, 0);
            queue(Controller.Opponent.Id, Command, Who.Rival, -1);
        }
    }
}
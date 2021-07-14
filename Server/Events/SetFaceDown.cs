namespace CardGame.Server.Events
{
    public class SetFaceDown : Event
    {
        private Card Card { get; }
        private Player Controller { get; }
        private int Source { get; }
        private int Destination { get; }

        public SetFaceDown(Player controller, Card card, int source, int destination)
        {
            Command = CommandId.SetFaceDown;
            Controller = controller;
            Card = card;
            Source = source;
            Destination = destination;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.MoveCard, Who.Player, Card.Id, Card.SetCodes, Zones.Hand, Zones.Supports, Source, Destination);
            queue(Controller.Opponent.Id, CommandId.MoveCard, Who.Rival, 0, SetCodes.NullCard, Zones.Hand, Zones.Supports, Source, Destination);
        }
    }
}
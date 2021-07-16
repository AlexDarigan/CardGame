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
            queue(Controller.Id, CommandId.PlayerSetFaceDown, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalSetFaceDown);
        }
    }
}
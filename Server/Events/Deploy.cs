namespace CardGame.Server.Events
{
    public class Deploy : Event
    {
        private Card Card { get; }
        private Player Controller { get; }
        private int Source { get; }
        private int Destination { get; }

        public Deploy(Player controller, Card card, int source, int destination)
        {
            Command = CommandId.SetFaceDown;
            Controller = controller;
            Card = card;
            Source = source;
            Destination = destination;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerDeploy, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalDeploy, Card.Id, Card.SetCodes);
        }
    }
}
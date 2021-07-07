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
            queue(Controller.Id, Command, IsClient, Card.Id, Card.SetCodes);
            queue(Controller.Opponent.Id, Command, !IsClient, Card.Id, Card.SetCodes);
        }
    }
}
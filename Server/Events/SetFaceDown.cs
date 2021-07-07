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
            queue(Controller.Id, Command, IsClient, Card.Id);
            queue(Controller.Opponent.Id, Command, !IsClient, -1);
        }
    }
}
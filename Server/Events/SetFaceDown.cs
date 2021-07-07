namespace CardGame.Server.Events
{
    public class SetFaceDown : Event
    {
        private readonly Card Card;
        private readonly Player Controller;

        public SetFaceDown(Player controller, Card card)
        {
            Command = CommandId.SetFaceDown;
            Controller = controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, isClient, Card.Id);
            queue(Controller.Opponent.Id, Command, !isClient, -1);
        }
    }
}
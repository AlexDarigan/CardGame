namespace CardGame.Server.Events
{
    public class SetFaceDown : Event
    {
        private Card Card { get; }
        private Player Controller { get; }
        
        public SetFaceDown(Player controller, Card card)
        {
            Controller = controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerSetFaceDown, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalSetFaceDown);
        }
    }
}
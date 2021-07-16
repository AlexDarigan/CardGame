namespace CardGame.Server.Events
{
    public class SetFaceDown : MoveEvent
    {
        public SetFaceDown(Card card): base(card) { }
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerSetFaceDown, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalSetFaceDown);
        }
    }
}
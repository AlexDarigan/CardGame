namespace CardGame.Server.Events
{
	public class SentToGraveyard: MoveEvent
	{
		public SentToGraveyard(Card card): base(card) { }
		public override void QueueOnClients(Enqueue queue)
		{
			queue(Card.Owner.Id, CommandId.SentToGraveyard, Card.Id);
			queue(Card.Owner.Opponent.Id, CommandId.SentToGraveyard, Card.Id);
		}
	}
}

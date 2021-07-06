namespace CardGame.Server
{
	public class SentToGraveyardEvent: Event
	{
		private Card Card { get; }

		public SentToGraveyardEvent(Card card)
		{
			Command = CommandId.SentToGraveyard;
			Card = card;
		}

		public override void QueueOnClients(Enqueue queue)
		{
			queue(Card.Controller.Id, Command, Card.Id);
			queue(Card.Controller.Opponent.Id, Command, Card.Id);

		}
	}
}

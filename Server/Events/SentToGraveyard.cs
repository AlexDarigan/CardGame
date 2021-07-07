namespace CardGame.Server.Events
{
	public class SentToGraveyard: Event
	{
		private Card Card { get; }

		public SentToGraveyard(Card card)
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

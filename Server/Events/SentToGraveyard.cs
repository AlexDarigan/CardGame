namespace CardGame.Server.Events
{
	public class SentToGraveyard: Event
	{
		private Card Card { get; }
		private Player Controller { get; }

		public SentToGraveyard(Player controller, Card card, int source, int destination)
		{
			Controller = controller;
			Card = card;
		}

		public override void QueueOnClients(Enqueue queue)
		{
			queue(Card.Owner.Id, CommandId.SentToGraveyard, Card.Id);
			queue(Card.Owner.Opponent.Id, CommandId.SentToGraveyard, Card.Id);
		}
	}
}

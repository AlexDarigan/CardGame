namespace CardGame.Server.Events
{
	public class SentToGraveyard: Event
	{
		private Card Card { get; }
		private Player Controller { get; }
		private int Source { get; }
		private int Destination { get; }

		public SentToGraveyard(Player controller, Card card, int source, int destination)
		{
			//Command = CommandId.SetFaceDown;
			Controller = controller;
			Card = card;
			Source = source;
			Destination = destination;
		}

		public override void QueueOnClients(Enqueue queue)
		{
			Zones origin = Card.CardTypes == CardTypes.Unit ? Zones.Units : Zones.Supports;
			queue(Controller.Id, CommandId.MoveCard, Who.Player, Card.Id, Card.SetCodes, origin, Zones.Discard, Source, Destination);
			queue(Controller.Opponent.Id, CommandId.MoveCard, Who.Rival, Card.Id, Card.SetCodes, origin, Zones.Discard, Source, Destination);
		}
	}
}

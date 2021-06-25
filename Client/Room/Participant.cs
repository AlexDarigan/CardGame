using Godot;

namespace CardGame.Client
{
	public class Participant: Object
	{
		public int Health = 8000;
		public States State;
		public readonly bool IsClient;
		
		public readonly Zone Deck = null;
		public readonly Zone Discard = null;
		public readonly Zone Hand = null;
		public readonly Zone Units = null;
		public readonly Zone Support = null;

		public Participant(Node view)
		{
			IsClient = view.Name == "Player";
			foreach (Node zone in view.GetChildren()) { Set(zone.Name, new Zone(zone)); }
		}

		public void Update(States state) => State = state;
	}
}

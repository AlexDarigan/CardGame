using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
	[UsedImplicitly]
	public class ParticipantFactory : Spatial
	{
		[Export()] public readonly bool IsClient;
		public Participant GetParticipant() => new Participant(this, IsClient);
	}
	
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

		public Participant(Node view, bool isClient)
		{
			IsClient = isClient;
			foreach (Zone zone in view.GetChildren()) { Set(zone.Name, zone); }
		}
	}
}

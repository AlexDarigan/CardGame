using Godot;

namespace CardGame.Client
{
	public class Room: Node
	{
		// TODO
		// Connect Participant Models with Participant Views
		// Basic Commands (Start, Draw, Deploy, Set, Attack, Activate, End, Win/Lose)
		// Include SFX/GFX
		// Basic Input Controller
		// Cards
		// Card Library
		// Deck Loading
		// Command Queue! (Use emit_signal/event/callv trick)
		// Command Objects
		// MultiplayerScript (avoid direct RPCs)
		
		
		[Export()]
		private Spatial Table;
		private readonly object MultiplayerInterface;
		private readonly object CommandQueue;
		private readonly object Library;
		private readonly object InputController;
		private const int Server = 1;
		
		public override void _Ready()
		{
			
			RpcId(Server, "OnClientReady");
			Table = GetNode<Spatial>("Table");
		}
		
	}
}

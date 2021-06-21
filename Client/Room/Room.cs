using Godot;

namespace CardGame.Client
{
	public class Room: Node
	{
		
		// TODO (REWRITE)
		// 1 - Add Card Data
		// 2 - Add Card Scene
		// 3 - Add Player/Rival View
		// 4 - Add Player/Rival ModelController
		// 5 - Connect Controller To Views
		// 6 - Load Deck
		// 7 - Register Cards && Add To Cards Node && Add To Player Deck
		// 8 - Draw Starting Hand / Basic Command Queue
		// 9 - Add Basic Input Controller / Multiplayer Commands
		//		...Draw, Deploy, Set, Activate, Destroy, Discard, End, Win, Lose
		// 10 - Add Commands for Draw/Deploy/Set/Activate/Destroy/Discard/End/Win/Lose
		// Etc -> Add SFX, ParticleFX, Tests, Hooks for Testing
		
		private Spatial Table;
		private readonly object MultiplayerInterface;
		private readonly object CommandQueue;
		private readonly object Library;
		private readonly object InputController;
		private const int Server = 1;
		
		public override void _Ready()
		{
			
			RpcId(Server, "OnClientReady");
			//Table = GetNode<Spatial>("Table");
		}
		
	}
}

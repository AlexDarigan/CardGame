using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace CardGame.Client
{
	public class Room: Node
	{
		
		// TODO (REWRITE)
		// 1 - Add Card Data // DONE
		// 2 - Add Card Scene // DONE
		// 3 - Add Player/Rival View // DONE
		// 4 - Add Player/Rival ModelController // DONE
		// 5 - Connect Controller To Views // DONE
		// 6 - Load Deck
		// 7 - Register Cards && Add To Cards Node && Add To Player Deck
		// 8 - Draw Starting Hand / Basic Command Queue
		// 9 - Add Basic Input Controller / Multiplayer Commands
		//		...Draw, Deploy, Set, Activate, Destroy, Discard, End, Win, Lose
		// 10 - Add Commands for Draw/Deploy/Set/Activate/Destroy/Discard/End/Win/Lose
		// Etc -> Add SFX, ParticleFX, Tests, Hooks for Testing
		
		// NOTE: We'll be using a lot of scattered code inside here before sorting it out later
		private Spatial Table;
		private readonly object MultiplayerInterface;
		private readonly Queue CommandQueue;
		private readonly object Library;
		private readonly object InputController;
		private Player Player;
		private Player Rival;
		private const int Server = 1;
		
		public override void _Ready()
		{
			RpcId(Server, "OnClientReady");
			Table = GetNode<Spatial>("Table");
			Player = new Player((Participant) Table.GetNode("Player"));
			Rival = new Player((Participant) Table.GetNode("Rival"));
		}

		public void Queue(object command)
		{
			CommandQueue.Enqueue(command);
		}

		public void Execute()
		{
			
		}

		private void LoadDeck()
		{
			
		}

		private void Draw()
		{
			
		}
	}
	
	class Player
	{
		private readonly Participant Zones;
		private int Health = 8000;
		private IList<Card> Deck = new List<Card>();
		private IList<Card> Discard = new List<Card>();
		private IList<Card> Hand = new List<Card>();
		private IList<Card> Units = new List<Card>();
		private IList<Card> Support = new List<Card>();

		public Player(Participant zones)
		{
			Zones = zones;
		}
	}
}

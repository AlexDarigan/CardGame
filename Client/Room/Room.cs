using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
	public class Room : Spatial
	{

		// BUG
		// Our client-side deploy looks garbage
		// Card is in wrong place
		// Ideas: 1 await is not working properly
		// Ideas 2: Something is resetting position
		// Idea 3: Deck is reversed
		// TODO (REWRITE)
		// 9 - Add Basic Input Controller / Multiplayer Commands
		//		...Draw, Deploy, Set, Activate, Destroy, Discard, End, Win, Lose
		// 10 - Add Commands for Draw/Deploy/Set/Activate/Destroy/Discard/End/Win/Lose
		// Etc -> Add SFX, ParticleFX, Tests, Hooks for Testing

		// NOTE: We'll be using a lot of scattered code inside here before sorting it out later
		[Signal] public delegate void Updated();
		private Spatial Table;
		private readonly Queue<Command> CommandQueue = new();
		private readonly object InputController;
		public Player Player { get; private set; }
		public Player Rival { get; private set; }
		private Register Register;
		private Tween GFX;
		private Control GUI;
		private const int Server = 1;

		public override void _Ready()
		{
			Table = GetNode<Spatial>("Table");
			Register = GetNode<Register>("Cards");
			GFX = GetNode<Tween>("GFX");
			GUI = GetNode<Control>("GUI");
			Player = new Player((Participant) Table.GetNode("Player"), true);
			Rival = new Player((Participant) Table.GetNode("Rival"), false);
			RpcId(Server, "OnClientReady");
		}
		
		[Puppet]
		public void Queue(CommandId commandId, params object[] args)
		{
			CommandQueue.Enqueue((Command) Call(commandId.ToString(), args));
		}

		[Puppet]
		public async void Update()
		{
			while (CommandQueue.Count > 0)
			{
				GFX.RemoveAll();
				CommandQueue.Dequeue().Execute(GFX);
				GFX.Start();
				await ToSignal(GFX, "tween_all_completed");
			}
			EmitSignal(nameof(Updated));
		}

		[Puppet]
		public void SetState(States state) => Player.State = state;

		[Puppet]
		public void Deploy(Card card)
		{
			RpcId(Server, "Deploy", card.Id);
		}

		private Command LoadDeck(bool isClient, Dictionary<int, SetCodes> deck) => new LoadDeck(GetPlayer(isClient), deck, Register);
		private Command Draw(bool isClient, int cardId) => new Draw(GetPlayer(isClient), Register[cardId]);
		private Command Deploy(bool isClient, int cardId) => new Deploy(GetPlayer(isClient), Register[cardId]);
		private Player GetPlayer(bool isClient) => isClient ? Player : Rival;


	}

	public class Player
	{
		public States State;
		public readonly bool isClient;
		public readonly Participant Zones;
		private int Health = 8000;
		public IList<Card> Deck = new List<Card>();
		public IList<Card> Discard = new List<Card>();
		public IList<Card> Hand = new List<Card>();
		public IList<Card> Units = new List<Card>();
		public IList<Card> Support = new List<Card>();

		public Player(Participant zones, bool _isClient)
		{
			Zones = zones;
			isClient = _isClient;
		}
	}
}



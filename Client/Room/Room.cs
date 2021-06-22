using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class Room : Node
	{

		// TODO (REWRITE)
		// 1 - Add Card Data // DONE
		// 2 - Add Card Scene // DONE
		// 3 - Add Player/Rival View // DONE
		// 4 - Add Player/Rival ModelController // DONE
		// 5 - Connect Controller To Views // DONE
		// 6 - Load Deck // Register Cards
		// 7 - Register Cards && Add To Cards Node && Add To Player Deck
		// 8 - Draw Starting Hand / Basic Command Queue
		// 9 - Add Basic Input Controller / Multiplayer Commands
		//		...Draw, Deploy, Set, Activate, Destroy, Discard, End, Win, Lose
		// 10 - Add Commands for Draw/Deploy/Set/Activate/Destroy/Discard/End/Win/Lose
		// Etc -> Add SFX, ParticleFX, Tests, Hooks for Testing

		// NOTE: We'll be using a lot of scattered code inside here before sorting it out later
		private Spatial Table;
		private readonly Queue<Command> CommandQueue = new();
		private readonly object InputController;
		private Player Player;
		private Player Rival;
		private Register Register;
		private const int Server = 1;

		public override void _Ready()
		{
			Table = GetNode<Spatial>("Table");
			Register = GetNode<Register>("Cards");
			Player = new Player((Participant) Table.GetNode("Player"));
			Rival = new Player((Participant) Table.GetNode("Rival"));
			RpcId(Server, "OnClientReady");
		}
		
		[Puppet]
		public void Queue(CommandId commandId, params object[] args)
		{
			CommandQueue.Enqueue((Command) Call(commandId.ToString(), args));
			Console.WriteLine(commandId.ToString());
		}

		[Puppet]
		public void Update()
		{
			Console.WriteLine("Update Not Implemented");
		}

		private Command LoadDeck(Dictionary<int, SetCodes> deck) => new LoadDeck(Player, deck, Register);
		private Command Draw(int playerId, int cardId) => new Draw(Player, Register[cardId]);


	}

	public class Player
	{
		private readonly Participant Zones;
		private int Health = 8000;
		public IList<Card> Deck = new List<Card>();
		private IList<Card> Discard = new List<Card>();
		private IList<Card> Hand = new List<Card>();
		private IList<Card> Units = new List<Card>();
		private IList<Card> Support = new List<Card>();

		public Player(Participant zones)
		{
			Zones = zones;
		}
	}

	public abstract class Command: Object
	{
		// Commands are required to be Godot Objects otherwise we can't use .Call()
		protected Command()
		{
		}
	}

	public class LoadDeck : Command
	{
		private readonly Player _player;
		private readonly Dictionary<int, SetCodes> _deck;

		public LoadDeck(Player player, Dictionary<int, SetCodes> deck, Register register)
		{
			_player = player;
			_deck = deck;

			// We execute this command immediately so cards exist for future incoming commands..
			// ..possibly this suggests we should separate this from add/register_card option?
			foreach (KeyValuePair<int, SetCodes> pair in deck)
			{
				register.Add(pair.Key, pair.Value);
				player.Deck.Add(register[pair.Key]);
			}
		}
	}

	public class Draw: Command
	{
		private readonly Player _player;
		private readonly Card Card;

		public Draw(Player player, Card card)
		{
			_player = player;
			Card = card;
			Console.WriteLine($"{player} drew card {Card.Id}: {Card}");
		}
	}
}

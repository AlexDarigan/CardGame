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
		private Tween GFX;
		private const int Server = 1;

		public override void _Ready()
		{
			Table = GetNode<Spatial>("Table");
			Register = GetNode<Register>("Cards");
			GFX = GetNode<Tween>("GFX");
			Player = new Player((Participant) Table.GetNode("Player"));
			Rival = new Player((Participant) Table.GetNode("Rival"));
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
				await CommandQueue.Dequeue().Execute(GFX);
			}
		}

		private Command LoadDeck(bool isClient, Dictionary<int, SetCodes> deck) => new LoadDeck(GetPlayer(isClient), deck, Register);
		private Command Draw(bool isClient, int cardId) => new Draw(GetPlayer(isClient), Register[cardId]);
		private Player GetPlayer(bool isClient) => isClient ? Player : Rival;


	}

	public class Player
	{
		public readonly Participant Zones;
		private int Health = 8000;
		public IList<Card> Deck = new List<Card>();
		public IList<Card> Discard = new List<Card>();
		public IList<Card> Hand = new List<Card>();
		public IList<Card> Units = new List<Card>();
		public IList<Card> Support = new List<Card>();

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
			AddUserSignal("NullCommand");
		}

		public abstract SignalAwaiter Execute(Tween gfx);
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


		public override SignalAwaiter Execute(Tween gfx)
		{ 
			CallDeferred("emit_signal", "NullCommand");
			return ToSignal(this, "NullCommand");
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
		}
		
		public override SignalAwaiter Execute(Tween gfx)
		{
			gfx.RemoveAll();
			
			if (_player.Zones.Name == "Rival")
			{
			 	CallDeferred("emit_signal", "NullCommand");
			 	return ToSignal(this, "NullCommand");
			}
			
			Spatial source = _player.Zones.Deck.GetNode<Spatial>($"{_player.Deck.Count - 1}");
			Position3D destination = _player.Zones.Hand.GetNode<Position3D>($"{_player.Hand.Count}");
			
			_player.Deck.Remove(Card);
			_player.Hand.Add(Card);
			source.Visible = false; // We're effectively replacing the marker with a real card

			const float duration = 0.25f; //1 - (_player.Hand.Count * 0.1f);
			gfx.InterpolateProperty(Card, "translation", source.Translation, destination.Translation,  duration);
			gfx.InterpolateProperty(Card, "rotation_degrees", source.RotationDegrees, destination.RotationDegrees, duration);
			
			gfx.Start();
			return ToSignal(gfx, "tween_all_completed");
		}
	}
}

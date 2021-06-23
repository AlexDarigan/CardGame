using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class Room : Spatial
	{

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
			while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(GFX); }
			EmitSignal(nameof(Updated));
		}

		[Puppet]
		public void SetState(States state) => Player.State = state;

		[Puppet]
		public void Deploy(Card card)
		{
			RpcId(Server, "Deploy", card);
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
		
			// We execute this on instantiation because other commands will require the cards to exist to work
			// properly (however maybe we can investigate yielding constructors?)
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
			// Our rival doesn't have a real card, so we need to make a local check lest we end up moving the same card around 
			Card card = _player.isClient ? Card : _player.Deck.Last();
			gfx.RemoveAll();
			
			Spatial source = _player.Zones.Deck.GetNode<Spatial>($"{_player.Deck.Count - 1}");
			Spatial destination = _player.Zones.Hand.GetNode<Spatial>($"{_player.Hand.Count}");
			
			_player.Deck.Remove(card);
			_player.Hand.Add(card);
			source.Visible = false; // We're effectively replacing the marker with a real card

			const float duration = 0.25f;
			gfx.InterpolateProperty(card, "translation", source.Translation, destination.Translation,  duration);
			gfx.InterpolateProperty(card, "rotation_degrees", source.RotationDegrees, destination.RotationDegrees, duration);
			
			gfx.Start();
			return ToSignal(gfx, "tween_all_completed");
		}
	}

	public class Deploy : Command
	{
		private readonly Player _player;
		private readonly Card Card;

		public Deploy(Player player, Card card)
		{
			_player = player;
			Card = card;
		}
		
		public override SignalAwaiter Execute(Tween gfx)
		{
			Card card = _player.isClient ? Card : _player.Hand.Last();
			gfx.RemoveAll();

			Spatial destination = _player.Zones.Units.GetNode<Spatial>($"{_player.Units.Count}");

			_player.Hand.Remove(Card);
			_player.Units.Add(Card);
			
			const float duration = 0.25f;
			gfx.InterpolateProperty(card, "translation", card.Translation, destination.Translation,  duration);
			gfx.InterpolateProperty(card, "rotation_degrees", card.RotationDegrees, destination.RotationDegrees, duration);
				
			gfx.Start();
			return ToSignal(gfx, "tween_all_completed");
		}
	}
}



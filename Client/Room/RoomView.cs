using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
	public class RoomView : Spatial { }
	public class Room : Node
	{
		// TODO (REWRITE)
		// 9 - Add Basic Input Controller / Multiplayer Commands
		//		...Draw, Deploy, Set, Activate, Destroy, Discard, End, Win, Lose
		// 10 - Add Commands for Draw/Deploy/Set/Activate/Destroy/Discard/End/Win/Lose
		// Etc -> Add SFX, ParticleFX, Tests, Hooks for Testing

		// NOTE: We'll be using a lot of scattered code inside here before sorting it out later
		[Signal] public delegate void Updated();
		private EventHandler UpdatedX;
		private readonly Node Cards;
		private readonly Dictionary<int, Card> _cards = new();
		private readonly Queue<Command> _commandQueue = new();
		private readonly Tween _gfx;
		private readonly Control _gui;
		private readonly Participant _player;
		private readonly Participant _rival;
		private const int Server = 1;
		private Card _currentCard;

		public Room(Node view, string name, MultiplayerAPI multiplayerApi)
		{
			Name = name;
			CustomMultiplayer = multiplayerApi;
			Cards = view.GetNode<Node>("Cards");
			_gfx = view.GetNode<Tween>("GFX");
			_gui = view.GetNode<Control>("GUI");
			_player = new Participant(view.GetNode<Node>("Table/Player"));
			_rival = new Participant(view.GetNode<Node>("Table/Rival"));
		}

		public override void _Ready() => RpcId(Server, "OnClientReady"); 
		
		[Puppet] public void Queue(CommandId commandId, params object[] args) => _commandQueue.Enqueue((Command) Call(commandId.ToString(), args));
		
		[Puppet]
		public async void Update()
		{
			while (_commandQueue.Count > 0) { await _commandQueue.Dequeue().Execute(_gfx); }
			EmitSignal(nameof(Updated));
		}
		
		[Puppet] public void SetState(States state) => _player.State = state;
		[Puppet] public void Deploy(Card card) => RpcId(Server, "Deploy", card.Id);
		[Puppet] public void Set(Card card) => RpcId(Server, "Set", card.Id);
		[Puppet] public void Pass() => RpcId(Server, "Pass");
		[Puppet] public void EndTurn() => RpcId(Server, "EndTurn");

		private Command LoadDeck(bool isClient, Dictionary<int, SetCodes> deck) => new LoadDeck(GetPlayer(isClient), deck, CreateCard);
		private Command Draw(bool isClient, int cardId) => new Draw(GetPlayer(isClient), GetCard(cardId));
		private Command Deploy(bool isClient, int cardId) => new Deploy(GetPlayer(isClient), GetCard(cardId));
		private Participant GetPlayer(bool isClient) => isClient ? _player : _rival;
		private Card GetCard(int id, SetCodes setCode = SetCodes.NullCard) => _cards.ContainsKey(id) ? _cards[id] : CreateCard(id, setCode);

		private Card CreateCard(int id, SetCodes setCode)
		{
			Card card = Library.GetCard(Cards, setCode, id);
			_cards[id] = card;
			card.Connect(nameof(Card.OnCardEntered), this, nameof(OnMouseEnterCard));
			card.Connect(nameof(Card.OnCardExited), this, nameof(OnMouseExitCard));
			return card;
		}

		public void OnMouseEnterCard(Card card) { _currentCard = card; }
		public void OnMouseExitCard(Card card) { if (_currentCard == card) { _currentCard = null; } }

		public override void _Input(InputEvent input)
		{
			if (input is InputEventMouseButton {Doubleclick: true, ButtonIndex: (int) ButtonList.Left} && _currentCard is not null)
			{
				OnCardPressed();
			}
		}

		private void OnCardPressed()
		{
			// Switch against Card State
			Console.WriteLine($"{_currentCard} pressed");
		}
	}
}


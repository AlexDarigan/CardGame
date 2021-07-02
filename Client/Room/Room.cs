using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
	public delegate void Declaration(CommandId commandId, params object[] args);
	public delegate void Update();
	public class Room : Node
	{
		// TODO (REWRITE)
		// 9 - Add Basic Input Controller / Multiplayer Commands
		//		...Draw, Deploy, Set, Activate, Destroy, Discard, End, Win, Lose
		// 10 - Add Commands for Draw/Deploy/Set/Activate/Destroy/Discard/End/Win/Lose
		// Etc -> Add SFX, ParticleFX, Tests, Hooks for Testing

		// BASIC INPUT SYSTEM
		
		// ACTIONS
		// ..Activate
		// ..AttackUnit
		// ..AttackPlayer
		// ..PassPlay
		// ..EndTurn
		// NOTE: Focus on simplest first? (Pass/End)
		
		// PRE-REQUISITES
		// ..PlayerState
		// ..CardState
		// ..CardUpdate
		// ..Targets for Activation/Attack
		
		// MUSINGS
		// ..Should we change zones to be selectable?
		// ..Does positioning benefit us?
		// ..It makes sense considering how our zones look
		
		[Signal] public delegate void Updated();
		
		private readonly Cards _cards;
		private readonly Queue<Command> _commandQueue = new();
		private readonly Tween _gfx;
		private readonly Control _gui;
		private readonly Participant _player;
		private readonly Participant _rival;
		private const int Server = 1;

		public Room(Node view, string name, MultiplayerAPI multiplayerApi)
		{
			AddChild(view);
			Name = name;
			CustomMultiplayer = multiplayerApi;
			_cards = view.GetNode<Cards>("Cards");
			_gfx = view.GetNode<Tween>("GFX");
			_gui = view.GetNode<Control>("GUI");
			_player = new Participant(view.GetNode<Node>("Table/Player"), (commandId,args) => RpcId(Server, commandId.ToString(), args));
			_rival = new Participant(view.GetNode<Node>("Table/Rival"), delegate{  });
			_cards.Player = _player;
		}

		public override void _Ready() => RpcId(Server, "OnClientReady"); 
		
		[Puppet] public void Queue(CommandId commandId, params object[] args) => _commandQueue.Enqueue((Command) Call(commandId.ToString(), args));
		
		[Puppet]
		public async void Update(States states)
		{
			while (_commandQueue.Count > 0) { await _commandQueue.Dequeue().Execute(_gfx); }
			_player.Update(states);
			EmitSignal(nameof(Updated), states);
		}
		
		[Puppet] public void UpdateCard(int id, CardState state) => _cards[id].Update(state);
		
		[Puppet] private void Deploy(Card card) => RpcId(Server, "Deploy", card.Id);
		[Puppet] private void Set(Card card) => RpcId(Server, "Set", card.Id);
		[Puppet] public void Pass() => RpcId(Server, "Pass");
		[Puppet] public void EndTurn() => RpcId(Server, "EndTurn");

		private Command LoadDeck(bool isClient, Dictionary<int, SetCodes> deck) => new LoadDeck(GetPlayer(isClient), deck, _cards.GetCard);
		private Command Draw(bool isClient, int cardId) => new Draw(GetPlayer(isClient), GetCard(cardId));
		private Command Deploy(bool isClient, int cardId) => new Deploy(GetPlayer(isClient), GetCard(cardId));
		private Command SetFaceDown(bool isClient, int cardId) => new Set(GetPlayer(isClient), GetCard(cardId));
		private Participant GetPlayer(bool isClient) => isClient ? _player : _rival;
		private Card GetCard(int id, SetCodes setCode = SetCodes.NullCard) => _cards.GetCard(id, setCode);
	}
}



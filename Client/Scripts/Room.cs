using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace CardGame.Client
{
    public delegate void Declaration(CommandId commandId, params object[] args);

    public delegate void Update();

    public class Room : Node
    {
        [Signal] public delegate void Updated();

        private const int Server = 1;
        private readonly Cards _cards;
        private readonly Queue<Command> _commandQueue = new();
        private readonly Tween _gfx;
        private readonly Control _gui;
        private readonly Participant _player;
        private readonly Participant _rival;

        private Room() { /* Required By Godot */ }

        public Room(Node view, string name, MultiplayerAPI multiplayerApi)
        {
            Name = name;
            AddChild(view, true);
            CustomMultiplayer = multiplayerApi;
            _cards = new Cards(view.GetNode<Spatial>("Cards"));
            _gfx = view.GetNode<Tween>("GFX");
            _gui = view.GetNode<Control>("GUI");
            _player = new Participant(view.GetNode<Node>("Table/Player"),
                (commandId, args) => RpcId(Server, commandId.ToString(), args));
            _rival = new Participant(view.GetNode<Node>("Table/Rival"), delegate { });
            _cards.Player = _player;
            GD.Print("Room Created");
            _gui.GetNode<Button>("Menu/EndTurn");
            _gui.GetNode<Label>("ID").Text = multiplayerApi.GetNetworkUniqueId().ToString();
        }

        public override void _Ready() => RpcId(Server, "OnClientReady");
        
        [Puppet]
        public async void Update(States states)
        {
            while (_commandQueue.Count > 0) await _commandQueue.Dequeue().Execute(_gfx);
            _player.Update(states);
            _gui.GetNode<Label>("State").Text = states.ToString();
            EmitSignal(nameof(Updated), states); 
        }
        
        [Puppet] public void Queue(CommandId commandId, params object[] args) => _commandQueue.Enqueue((Command) Call(commandId.ToString(), args)); 
        [Puppet] public void UpdateCard(int id, CardState state) => _cards[id].Update(state); 
        private Command LoadDeck(bool who, System.Collections.Generic.Dictionary<int, SetCodes> deck) => new LoadDeck(GetPlayer(who), deck, _cards.GetCard); 
        private Command Draw(bool who, int id) => new Draw(GetPlayer(who), GetCard(id)); 
        private Command Deploy(bool who, int id) => new Deploy(GetPlayer(who), GetCard(id)); 
        private Command SetFaceDown(bool who, int id) => new Set(GetPlayer(who), GetCard(id)); 
        private Participant GetPlayer(bool isClient) => isClient ? _player : _rival; 
        private Card GetCard(int id, SetCodes setCode = SetCodes.NullCard) => _cards.GetCard(id, setCode); 
    }
}

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

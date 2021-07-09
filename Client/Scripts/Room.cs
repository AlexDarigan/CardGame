using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using CardGame.Client.Commands;

namespace CardGame.Client
{
    public delegate void Declaration(string commandId, params object[] args);
    
    public class Room : Node
    {
        private delegate Command Invoker(params object[] args);
        private static Dictionary<CommandId, Invoker> Commands { get; } = new();
        private Queue<Command> CommandQueue { get; } = new();
        public event EventHandler GameUpdated;
        private const int Server = 1;
        private Cards Cards { get; }
        public Tween Gfx { get; }
        private AudioStreamPlayer Sfx { get; }
        private AudioStreamPlayer Bgm { get; }
        private Control Gui { get; }
        private Player Player { get; }
        private Rival Rival { get; }
        private RoomView RoomView { get; set; }
        private Room() { /* Required By Godot */ }

        static Room()
        {
            foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            {
                ConstructorInfo c = Type.GetType($"CardGame.Client.Commands.{Enum.GetName(commandId.GetType(), commandId)}")?.GetConstructors()[0];
                Commands[commandId] = args => (Command) c?.Invoke(args);
            }
        }
        
        public Room(RoomView view, string name, MultiplayerAPI multiplayerApi)
        {
            RoomView = view;
            Name = name;
            CustomMultiplayer = multiplayerApi;
            Mouse mouse = new Mouse();
            Player = new Player(mouse);
            Rival = new Rival();
            Sfx = new AudioStreamPlayer();
            Bgm = new AudioStreamPlayer();
            Cards = new Cards();
            //CommandQueue = new CommandQueue(Player, Rival, Cards);
            Gfx = new Tween();
            RoomView.Id = multiplayerApi.GetNetworkUniqueId();
            RoomView.EndTurnPressed += Player.EndTurn;
            Player.Declare += Declare;
            Cards.Player = Player;
            
            foreach (Node child in new Node[]{view, Gfx, Sfx, Bgm, Cards, mouse}) { AddChild(child, true); }
        }
        
        public override void _Ready() { RpcId(Server, "OnClientReady"); }

        private void Declare(string commandId, params object[] args) { RpcId(Server, commandId, args); }

        [Puppet]
        public void LoadDeck(bool isPlayer, Dictionary<int, SetCodes> deck)
        {
            Participant player = isPlayer ? Player : Rival;
            foreach (Card card in deck.Select(pair => Cards.GetCard(pair.Key, pair.Value)))
            {
                player.Deck.Add(card);
                card.Owner = player;
                card.Controller = player;
                Location location = player.Deck.Locations.Last();
                card.Translation = location.Translation;
                card.RotationDegrees = location.RotationDegrees;
            }
        }
        
        [Puppet]
        public async void Update(States state, Dictionary<int, CardState> updateCards)
        {
            while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(this); }
            Player.State = state;
            foreach (KeyValuePair<int, CardState> pair in updateCards) { Cards[pair.Key].Update(pair.Value); }
            RoomView.UpdateState(state);
            GameUpdated?.Invoke(null, null);
        }

        [Puppet]
        public void Queue(CommandId commandId, params object[] args)
        {
            CommandQueue.Enqueue(Commands[commandId](args));
        }
        
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
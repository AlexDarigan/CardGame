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
        public event Action<Room, States> GameUpdated;
        private const int Server = 1;
        private delegate Command Invoker(params object[] args);
        
        private static Dictionary<CommandId, Invoker> Commands { get; } = new();
        private Queue<Command> CommandQueue { get; } = new();
        private Cards Cards { get; } = new();
        public Tween Gfx { get; } = new();
        private AudioStreamPlayer Sfx { get; } = new();
        private AudioStreamPlayer Bgm { get; } = new();
        private Player Player { get; }
        private Rival Rival { get; } = new();
        public RoomView RoomView { get; private set; } = Scenes.Room();
        private Room() { /* Required by Godot*/ }

        static Room()
        {
            foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            {
                ConstructorInfo c = Type.GetType($"CardGame.Client.Commands.{Enum.GetName(commandId.GetType(), commandId)}")?.GetConstructors()[0];
                Commands[commandId] = args => (Command) c?.Invoke(args);
            }
        }
        
        public Room(string name, MultiplayerAPI multiplayerApi)
        {
            Name = name;
            CustomMultiplayer = multiplayerApi;
            Mouse mouse = new Mouse();
            // Probably better to handle mouse in 1) A Dedicated Input Controller and 2) Events..
            // ..then we can place Player up as inlined instance
            Player = new Player(mouse);
            RoomView.Id = multiplayerApi.GetNetworkUniqueId();
            RoomView.EndTurnPressed += Player.EndTurn;
            Player.Declare += Declare;
            GameUpdated += RoomView.OnGameUpdated;
            Cards.Player = Player;
            
            foreach (Node child in new Node[]{RoomView, Gfx, Sfx, Bgm, Cards, mouse}) { AddChild(child, true); }
        }
        
        public override void _Ready() { RpcId(Server, "OnClientReady"); }

        private void Declare(string commandId, params object[] args) { RpcId(Server, commandId, args); }
        
        [Puppet] private async void Update() { while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(this); } }
        [Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(Commands[commandId](args)); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
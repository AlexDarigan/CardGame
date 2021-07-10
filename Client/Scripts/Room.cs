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
            
            // 1 - InputController
            // 2 - Remove Mouse From Player
            // 3 - Inline Player Instance
            
            Mouse mouse = new Mouse();
            Player = new Player(mouse);
            RoomView.Id = multiplayerApi.GetNetworkUniqueId();
            RoomView.EndTurnPressed += Player.EndTurn;
            Player.Declare += (commandId, args) => { RpcId(Server, commandId, args); };
            Cards.Player = Player;
            
            foreach (Node child in new Node[]{RoomView, Gfx, Sfx, Bgm, Cards, mouse}) { AddChild(child, true); }
        }
        
        public override void _Ready() { RpcId(Server, "OnClientReady"); }
        
        [Puppet] private async void Update() { while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(this); } }
        [Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(Commands[commandId](args)); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
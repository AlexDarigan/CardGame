using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using CardGame.Client.Commands;

namespace CardGame.Client
{
    public delegate void Declaration(CommandId commandId, params object[] args);
    
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
        private Player Player { get; } = new();
        private Rival Rival { get; } = new();
        public RoomView RoomView { get;  } = Scenes.Room();
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
            Player.OnAttackDeclared += RoomView.OnAttackDeclared;
            Player.OnAttackCancelled += RoomView.OnAttackCancelled;
            Player.Declare += (commandId, args) => { RpcId(Server, Enum.GetName(commandId.GetType(), commandId), args); };
            Cards.Player = Player;
            foreach (Node child in new Node[]{RoomView, Gfx, Sfx, Bgm, Cards}) { AddChild(child, true); }

            RoomView.RivalHeartPressed += Player.OnRivalHeartPressed;
        }
        
        
        [Puppet] private async void Update() { while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(this); } }
        [Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(Commands[commandId](args)); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
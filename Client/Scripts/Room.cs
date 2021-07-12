using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using CardGame.Client.Commands;
using CardGame.Client.Views;

namespace CardGame.Client
{
    public delegate void Declaration(CommandId commandId, params object[] args);
    
    public class Room : Node
    {
        private const int Server = 1;
        private delegate Command Invoker(params object[] args);
        private static Dictionary<CommandId, Invoker> Commands { get; } = new();
        private Queue<Command> CommandQueue { get; } = new();
        private Cards Cards { get; set; }
        public Tween Gfx { get; set; }
        private AudioStreamPlayer Sfx { get; set; }
        private AudioStreamPlayer Bgm { get; set; }
        private Player Player { get; } = new();
        private Rival Rival { get; } = new();
        public RoomView RoomView { get; set; }
    
        static Room()
        {
            foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            {
                ConstructorInfo c = Type.GetType($"CardGame.Client.Commands.{Enum.GetName(commandId.GetType(), commandId)}")?.GetConstructors()[0];
                Commands[commandId] = args => (Command) c?.Invoke(args);
            }
        }
        
        public Room() { }

        public override void _Ready()
        {
            Cards = GetNode<Cards>("Cards");
            RoomView = GetNode<RoomView>("Room");
            Bgm = GetNode<AudioStreamPlayer>("BGM");
            Sfx = GetNode<AudioStreamPlayer>("SFX");
            Gfx = GetNode<Tween>("GFX");
            RoomView.Id = CustomMultiplayer.GetNetworkUniqueId();
            RoomView.RivalHeart.Pressed += Player.OnRivalHeartPressed;
            Player.OnAttackDeclared += RoomView.OnAttackDeclared;
            Player.OnAttackCancelled += RoomView.OnAttackCancelled;
            Player.Declare += (commandId, args) => { RpcId(Server, Enum.GetName(commandId.GetType(), commandId), args); };
            Cards.Player = Player;
            RpcId(1, "OnClientReady");
        }


        [Puppet] private async void Update() { while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(this); } }
        [Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(Commands[commandId](args)); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
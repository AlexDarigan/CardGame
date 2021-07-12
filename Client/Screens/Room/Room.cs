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
       // public Tween Gfx { get; set; }
        private Player Player { get; } = new();
        private Rival Rival { get; } = new();

        // RoomView Items
        public int Id { get; set; }
        private Label PlayerId { get; set; }
        public Label State;
        public TurnCounter TurnCounter;
        public HealthBar PlayerHealth;
        public HealthBar RivalHealth;
        public ChessClockButton ChessClockButton;
        public Label GameOver;
        public Heart PlayerHeart;
        public Heart RivalHeart;
        private Mouse Mouse { get; set; }
        
        // Resorted Elements
        public Effects Effects { get; private set; }
    
        static Room()
        {
            foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            {
                ConstructorInfo c = Type.GetType($"CardGame.Client.Commands.{Enum.GetName(commandId.GetType(), commandId)}")?.GetConstructors()[0];
                Commands[commandId] = args => (Command) c?.Invoke(args);
            }
        }

        public static Room Instance() { return (Room) GD.Load<PackedScene>("res://Client/Screens/Room/Room.tscn").Instance(); }
        
        public Room() { }

        public override void _Ready()
        {
            
            // New Elements
            Effects = GetNode<Effects>("Effects");
            
            // RoomView Things
            PlayerId = GetNode<Label>("GUI/ID");
            State = GetNode<Label>("GUI/State");
            TurnCounter = GetNode<TurnCounter>("GUI/TurnCount");
            PlayerHealth = GetNode<HealthBar>("GUI/PlayerHealth");
            RivalHealth = GetNode<HealthBar>("GUI/RivalHealth");
            ChessClockButton = GetNode<ChessClockButton>("Table/ChessClockButton");
            GameOver = GetNode<Label>("GUI/GameOver");
            PlayerHeart = GetNode<Heart>("Table/PlayerHeart");
            RivalHeart = GetNode<Heart>("Table/RivalHeart");
            PlayerId.Text = Id.ToString();
            
            // Room Things
            Mouse = GetNode<Mouse>("Mouse");
            Cards = GetNode<Cards>("Cards");
            // Bgm = GetNode<AudioStreamPlayer>("BGM");
            // Sfx = GetNode<AudioStreamPlayer>("SFX");
            // Gfx = GetNode<Tween>("GFX");
            RivalHeart.Pressed += Player.OnRivalHeartPressed;
            Player.OnAttackDeclared += OnAttackDeclared;
            Player.OnAttackCancelled += OnAttackCancelled;
            Player.Declare += (commandId, args) => { RpcId(Server, Enum.GetName(commandId.GetType(), commandId), args); };
            Cards.Player = Player;
            
            RpcId(1, "OnClientReady");
        }
        
        
        public void OnAttackDeclared()
        {
            Mouse.OnAttackDeclared();
        }

        public void OnAttackCancelled()
        {
            Mouse.OnAttackCancelled();
        }



        [Puppet] private async void Update() { while (CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(this); } }
        [Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(Commands[commandId](args)); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
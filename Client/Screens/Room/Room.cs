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
        
        // RoomView Items
        public ChessClockButton ChessClockButton;
        public InputController InputController { get; set; }
        
        // Resorted Elements
        public Effects Effects { get; private set; }
        public Participant Player { get; private set; }
        public Participant Rival { get; private set; }
        public GUI GUI { get; private set; }
        public CommandQueue CommandQueue { get; private set; }
        public Cards Cards { get; private set; }
        // Input

        public static Room Instance() { return (Room) GD.Load<PackedScene>("res://Client/Screens/Room/Room.tscn").Instance(); }
        
        public Room() { }

        public override void _Ready()
        {
            // New Elements
            Effects = GetNode<Effects>("Effects");
            Player = GetNode<Participant>("Player");
            Rival = GetNode<Participant>("Rival");
            GUI = GetNode<GUI>("GUI");
            CommandQueue = GetNode<CommandQueue>("CommandQueue");

            // WIP
            InputController = GetNode<InputController>("InputController");

            
            // RoomView Things
            ChessClockButton = GetNode<ChessClockButton>("Table/ChessClockButton");
            GUI.Id.Text = CustomMultiplayer.GetNetworkUniqueId().ToString();
            
            // Room Things
            Cards = GetNode<Cards>("Cards");
            Rival.Avatar.Pressed += InputController.OnRivalAvatarPressed;
            InputController.Declare += (commandId, args) => { RpcId(Server, Enum.GetName(commandId.GetType(), commandId), args); };
            Cards.InputController = InputController;
            RpcId(1, "OnClientReady");
        }
        
        
        public void OnAttackDeclared()
        {
            InputController.OnAttackDeclared();
        }

        public void OnAttackCancelled()
        {
            InputController.OnAttackCancelled();
        }
        
        [Puppet] private async void Update() { CommandQueue.Execute(this); }
        [Puppet] private void Queue(CommandId commandId, object[] args) { CommandQueue.Enqueue(commandId, args); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
    }
}
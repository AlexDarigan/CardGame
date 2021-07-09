using System;
using System.Collections.Generic;
using Godot;
using CardGame.Client.Commands;

namespace CardGame.Client
{
    public delegate void Declaration(CommandId commandId, params object[] args);
    
    public class Room : Node
    {
        public event EventHandler GameUpdated;
        private const int Server = 1;
        private Cards Cards { get; }
        private CommandQueue CommandQueue { get; }
        private AudioStreamPlayer Sfx { get; }
        private AudioStreamPlayer Bgm { get; }
        private Control Gui { get; }
        private Player Player { get; }
        private Rival Rival { get; }
        private Room() { /* Required By Godot */ }

        public Room(Node view, string name, MultiplayerAPI multiplayerApi)
        {
            Name = name;
            CustomMultiplayer = multiplayerApi;

            Mouse mouse = new Mouse();
            Player = new Player(mouse);
            Rival = new Rival();
            Sfx = new AudioStreamPlayer();
            Bgm = new AudioStreamPlayer();
            Cards = new Cards();
            CommandQueue = new CommandQueue(Player, Rival, Cards);

            foreach (Node child in new []{view, CommandQueue, Sfx, Bgm, Cards, mouse}) { AddChild(child, true); }
            
            Gui = view.GetNode<Control>("GUI");
            Gui.GetNode<Button>("Menu/EndTurn").Connect("pressed", this, nameof(OnEndTurnPressed));
            Gui.GetNode<Label>("ID").Text = multiplayerApi.GetNetworkUniqueId().ToString();
            
            
            Player.Declare += Declare;
            Cards.Player = Player;
        }

        public override void _Ready() { RpcId(Server, "OnClientReady"); }

        private void Declare(CommandId command, params object[] args) { RpcId(Server, command.ToString(), args); }
        
        [Puppet]
        public async void Update(States state, Dictionary<int, CardState> updateCards)
        {
            await CommandQueue.Execute();
            Player.State = state;
            foreach (KeyValuePair<int, CardState> pair in updateCards)
            {
                Cards[pair.Key].Update(pair.Value);
            }
            Gui.GetNode<Label>("State").Text = state.ToString();
            GameUpdated?.Invoke(null, null);
        }

        [Puppet]
        public void Queue(CommandId commandId, params object[] args)
        {
            CommandQueue.Enqueue(commandId, args);
        }
        
        public void OnEndTurnPressed() { Player.EndTurn(); }
    }
}

// GettingPlayer
// GettingCards
// GetPlayer, GetCards, Arguments, can system reflection take variable arguments?
// Can System Reflection STORE commands variable arguments?

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
// ..Targets for Activation/Attack


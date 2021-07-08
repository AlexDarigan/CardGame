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
        private Queue<Command> CommandQueue { get; } = new();
        private Tween Gfx { get; }
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
        
            Gfx = new Tween();
            Sfx = new AudioStreamPlayer();
            Bgm = new AudioStreamPlayer();
            Cards = new Cards();
            Mouse mouse = new Mouse();
            
            foreach (Node child in new []{view, Gfx, Sfx, Bgm, Cards, mouse}) { AddChild(child, true); }
            
            Gui = view.GetNode<Control>("GUI");
            Gui.GetNode<Button>("Menu/EndTurn").Connect("pressed", this, nameof(OnEndTurnPressed));
            Gui.GetNode<Label>("ID").Text = multiplayerApi.GetNetworkUniqueId().ToString();
            
            Player = new Player(mouse);
            Rival = new Rival();
            Player.Declare += Declare;
            Cards.Player = Player;
        }
        
        public override void _Ready() => RpcId(Server, "OnClientReady");

        private void Declare(CommandId command, params object[] args) { RpcId(Server, command.ToString(), args); }
        
        [Puppet]
        public async void Update(States state)
        {
            while (CommandQueue.Count > 0) await CommandQueue.Dequeue().Execute(Gfx);
            Player.State = state;
            Gui.GetNode<Label>("State").Text = state.ToString();
            GameUpdated?.Invoke(null, null);
        }
        
        [Puppet] public void Queue(CommandId commandId, params object[] args) => CommandQueue.Enqueue((Command) Call(commandId.ToString(), args)); 
        [Puppet] public void UpdateCard(int id, CardState state) => Cards[id].Update(state); 
        private Command LoadDeck(bool who, Dictionary<int, SetCodes> deck) => new LoadDeck(GetPlayer(who), deck, Cards.GetCard); 
        private Command Draw(bool who, int id) => new Draw(GetPlayer(who), GetCard(id)); 
        private Command Deploy(bool who, int id, SetCodes setCodes) => new Deploy(GetPlayer(who), GetCard(id, setCodes));
        private Command SetFaceDown(bool who, int id) => new SetFaceDown(GetPlayer(who), GetCard(id));
        private Command SetHealth(bool who, int health) => new SetHealth(GetPlayer(who), health);
        private Command SentToGraveyard(int id) => new SentToGraveyard(GetCard(id));
        private Command Battle(int attacker, int defender) => new Battle(GetCard(attacker), GetCard(defender));
        private Participant GetPlayer(bool isClient) => isClient ? Player : Rival; 
        private Card GetCard(int id, SetCodes setCode = default) => Cards.GetCard(id, setCode);
        public void OnEndTurnPressed() { Player.EndTurn(); }
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
// ..Targets for Activation/Attack


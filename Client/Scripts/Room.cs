using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using CardGame.Client.Commands;

namespace CardGame.Client
{
    public delegate void Declaration(string commandId, params object[] args);
    
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
        private RoomView RoomView { get; set; }
        private Room() { /* Required By Godot */ }

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
            CommandQueue = new CommandQueue(Player, Rival, Cards);
            RoomView.Id = multiplayerApi.GetNetworkUniqueId();
            RoomView.EndTurnPressed += Player.EndTurn;
            RoomView.EndTurnPressed += OnEndTurnPressed;
            Player.Declare += Declare;
            Cards.Player = Player;
            
            foreach (Node child in new Node[]{view, CommandQueue, Sfx, Bgm, Cards, mouse}) { AddChild(child, true); }
        }
        
        public override void _Ready() { RpcId(Server, "OnClientReady"); }

        private void Declare(string commandId, params object[] args)
        {
            RoomView.PlayerId.Text = "1";
            RpcId(Server, commandId, args);
        }

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
            await CommandQueue.Execute();
            Player.State = state;
            foreach (KeyValuePair<int, CardState> pair in updateCards) { Cards[pair.Key].Update(pair.Value); }
            RoomView.State.Text = state.ToString();
            GameUpdated?.Invoke(null, null);
        }

        [Puppet]
        public void Queue(CommandId commandId, params object[] args) { CommandQueue.Enqueue(commandId, args); }

        public void OnEndTurnPressed()
        {
            if (Player.State != States.IdleTurnPlayer)
            {
                return;
            }
            RoomView.AddTurn();
            //RpcId(Server, "EndTurn");
        }
    }
}
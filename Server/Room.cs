using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Godot;


namespace CardGame.Server
{
    public class Room: Node
    {
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private readonly Dictionary<int, Card> _cards = new Dictionary<int, Card>();
        private int _nextCardId = 0;

        public Room()
        {
            // I believe an empty constructor is required in Godot Classes that have non-empty constructor(s)
            // ..for the sake of some Godot callbacks
        }
        public Room(Player player1, Player player2)
        {
            player1.Opponent = player2;
            player2.Opponent = player1;
            _players[player1.Id] = player1;
            _players[player2.Id] = player2;
        }

        [Master]
        public void OnClientReady()
        {
            _players[CustomMultiplayer.GetRpcSenderId()].Ready = true;
            if (_players.Values.Any(player => !player.Ready))
            {
                return;
            }
            Start();
        }

        private void Start()
        {
            SetDecks();
        }

        private void SetDecks()
        {
            // We Set Decks here so we cards can be registered
            foreach (Player player in _players.Values)
            {
                for (int i = 0; i < 40; i++)
                {
                    Card card = new Card(_nextCardId, player);
                    _nextCardId++;
                    player.Deck.Add(card);
                }
            }
        }
        
    }
}
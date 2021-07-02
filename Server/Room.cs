using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Server
{
    public delegate void Enqueue(int id, CommandId command, params object[] args);

    public class Room : Node
    {
        private readonly Cards _cards = new();
        private readonly Match _match;
        private readonly Dictionary<int, Player> _players = new();
        public Room() { /* Required By Godot Engine Callbacks */ }

        public Room(Player player1, Player player2)
        {
            _players[player1.Id] = player1;
            _players[player2.Id] = player2;
            _match = new Match(player1, player2, _cards, Update, Queue);
        }
        
        private void Update()
        {
            // Requires Some Work
            foreach (int id in _players.Keys)
            {
                foreach (Card card in _cards)
                {
                    if (card.Controller.Id != id) continue;
                   // Console.WriteLine();
                    RpcId(id, "UpdateCard", card.Id, card.CardState);
                }

                RpcId(id, "Update", _players[id].State);
            }
        }

        private void Queue(int player, CommandId commandId, params object[] args) { RpcId(player, "Queue", commandId, args); }
        
        [Master]
        public void OnClientReady()
        {
            _players[CustomMultiplayer.GetRpcSenderId()].Ready = true;
            if (_players.Values.Any(player => !player.Ready)) return;
            _match.Begin(_players.Values.ToList());
        }

        [Master] public void Deploy(int cardId) => _match.Deploy(_players[Multiplayer.GetRpcSenderId()], _cards[cardId]); 
        [Master] public void SetFaceDown(int cardId) => _match.SetFaceDown(_players[Multiplayer.GetRpcSenderId()], _cards[cardId]);
        [Master] public void EndTurn() => _match.EndTurn(_players[Multiplayer.GetRpcSenderId()]); 
    }
}
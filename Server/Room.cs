using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Server
{
    public delegate void Enqueue(int id, CommandId command, params object[] args);

    public class Room : Node
    {
        private Cards Cards { get; } = new();
        private Match Match { get; }
        private Dictionary<int, Player> Players { get; } = new();
        public Room() { /* Required By Godot Engine Callbacks */ }

        public Room(Player player1, Player player2)
        {
            Players[player1.Id] = player1;
            Players[player2.Id] = player2;
            Match = new Match(player1, player2, Cards, Update, Queue);
        }
        
        private void Update()
        {
            // Requires Some Work
            foreach (int id in Players.Keys)
            {
                Dictionary<int, CardStates> updateCards = new();
                foreach (Card card in Cards)
                {
                    if (card.Controller.Id != id) continue;
                    updateCards[card.Id] = card.CardStates;
                }

                // This doesn't work directly, maybe the args are stored in object[] when passed rather than unpacked?
                Queue(id, CommandId.UpdateCards, updateCards.AsEnumerable());
                Queue(id, CommandId.UpdatePlayer, Players[id].State);
                RpcId(id, "Update");
            }
        }

        private void Queue(int player, CommandId commandId, params object[] args) { RpcId(player, "Queue", commandId, args); }
        
        [Master]
        public void OnClientReady()
        {
            Players[CustomMultiplayer.GetRpcSenderId()].Ready = true;
            if (Players.Values.Any(player => !player.Ready)) return;
            Match.Begin(Players.Values.ToList());
        }

        [Master] public void Deploy(int cardId) => Match.Deploy(Players[Multiplayer.GetRpcSenderId()], Cards[cardId]);
        [Master] public void DeclareAttack(int attackerId, int defenderId) => Match.DeclareAttack(Players[Multiplayer.GetRpcSenderId()], Cards[attackerId], Cards[defenderId]);
        [Master] public void DeclareDirectAttack(int attackerId) => Match.DeclareDirectAttack(Players[Multiplayer.GetRpcSenderId()], Cards[attackerId]);
        [Master] public void SetFaceDown(int cardId) => Match.SetFaceDown(Players[Multiplayer.GetRpcSenderId()], Cards[cardId]);
        [Master] public void EndTurn() => Match.EndTurn(Players[Multiplayer.GetRpcSenderId()]); 
    }
}
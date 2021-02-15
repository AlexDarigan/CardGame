using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Godot;


namespace CardGame.Server
{
    public class Room: Node
    {
        private readonly Match _match;
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private readonly CardRegister _cards = new CardRegister();
     
        public Room()
        { 
            // I believe an empty constructor is required in Godot Classes that have non-empty constructor(s)
            // ..for the sake of some Godot callbacks
            
        }
        public Room(Player player1, Player player2)
        {
            _players[player1.Id] = player1;
            _players[player2.Id] = player2;
            _match = new Match(player1, player2, _cards, Update);
        }

        private void Update()
        {
            
        }
        

        [Master]
        public void OnClientReady()
        {
            _players[CustomMultiplayer.GetRpcSenderId()].Ready = true;
            if (_players.Values.Any(player => !player.Ready))
            {
                return;
            }
            
            // Since all of our work is done in the constructor we will be ready to push our events here
            // (With that in mind, should we queue everything serverside and only push it once when ready to update?..
            // ..arguably that could help with some peekAhead methods on client-side for better queueing).
            Update();
        }

        [Master]
        public void EndTurn(int playerId)
        {
            _match.EndTurn(_players[playerId]);
        }
    }
}
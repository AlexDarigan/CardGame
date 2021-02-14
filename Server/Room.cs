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
            GD.Print("Let The Game Begin!");
        }
        
    }
}
using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Server
{
    public class Connection : Node
    {
        private const int Port = 5000;
        private readonly NetworkedMultiplayerENet Server = new();
        private readonly Queue<Player> Queue = new();
        public bool IsLive => Server.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
        public bool IsServer => CustomMultiplayer.IsNetworkServer();
        public int PlayerCount => Queue.Count;

        public override void _Ready()
        {
            Error err = Server.CreateServer(5000);
            if (err != Error.Ok)
            {
                GD.PushError(err.ToString());
            }

            CustomMultiplayer = new MultiplayerAPI {RootNode = this, NetworkPeer = Server};
            CustomMultiplayer.Connect("network_peer_connected", this, nameof(OnNetworkPeerConnected));
        }

        private void OnNetworkPeerConnected(int id)
        {
            Queue.Enqueue(new Player(id));
        }

        public override void _Process(float delta)
        {
            if (CustomMultiplayer.HasNetworkPeer())
            {
                CustomMultiplayer.Poll();
            }
        }

        public override void _ExitTree()
        {
            Server?.CloseConnection();
        }
    }
}
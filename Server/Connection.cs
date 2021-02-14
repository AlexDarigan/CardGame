using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Server
{
    public class Connection : Node
    {
        private const int Port = 5000;
        private readonly NetworkedMultiplayerENet Server = new NetworkedMultiplayerENet();
        private readonly Queue<Player> Queue = new Queue<Player>();
        private int roomCount = 0;
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

            CustomMultiplayer = new MultiplayerAPI {NetworkPeer = Server};
            CustomMultiplayer.SetRootNode(this);
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

            if (PlayerCount > 1)
            {
                CreateRoom();
            }
        }

        private void CreateRoom()
        {
            // This will likely need review when we build our GUI since it depends on NodePaths in the tree
            Player player1 = Queue.Dequeue();
            Player player2 = Queue.Dequeue();
            roomCount++;
            Room room = new Room(player1, player2) {Name = roomCount.ToString(), CustomMultiplayer = CustomMultiplayer};
            RpcId(player1.Id, "CreateRoom", roomCount.ToString());
            RpcId(player2.Id, "CreateRoom", roomCount.ToString());
            AddChild(room);
        }

        public override void _ExitTree()
        {
            Server?.CloseConnection();
        }
    }
}
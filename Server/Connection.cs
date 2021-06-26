using System;
using System.Collections.Generic;
using System.Linq;
using Godot;


namespace CardGame.Server
{
    public class Connection : Node
    {
        private const int Port = 5000;
        private readonly NetworkedMultiplayerENet _server = new NetworkedMultiplayerENet();
        private readonly Queue<Player> _queue = new Queue<Player>();
        private int _roomCount = 0;
        public bool IsLive => _server.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
        public bool IsServer => CustomMultiplayer.IsNetworkServer();
        public int PlayerCount => _queue.Count;

        public override void _Ready()
        {
            Error err = _server.CreateServer(5000);
            if (err != Error.Ok) { GD.PushError(err.ToString()); }
            CustomMultiplayer = new MultiplayerAPI() {NetworkPeer = _server, RootNode = this};
        }

        [Master]
        public void OnNetworkPeerConnected(IEnumerable<SetCodes> deckList)
        {
            _queue.Enqueue(new Player(CustomMultiplayer.GetRpcSenderId(), deckList));
        }

        public override void _Process(float delta)
        {
            if (CustomMultiplayer.HasNetworkPeer()) {CustomMultiplayer.Poll(); }
            if (PlayerCount > 1) { CreateRoom(); }
        }

        private void CreateRoom()
        {
            // This will likely need review when we build our GUI since it depends on NodePaths in the tree
            Player player1 = _queue.Dequeue();
            Player player2 = _queue.Dequeue();
            _roomCount++;
            Room room = new Room(player1, player2) {Name = _roomCount.ToString(), CustomMultiplayer = CustomMultiplayer};
            RpcId(player1.Id, "CreateRoom", _roomCount.ToString());
            RpcId(player2.Id, "CreateRoom", _roomCount.ToString());
            AddChild(room);
        }

        public override void _ExitTree()
        {
            _server?.CloseConnection();
            QueueFree();
        }
    }
}
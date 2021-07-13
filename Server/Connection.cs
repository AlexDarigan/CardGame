using System.Collections.Generic;
using Godot;

namespace CardGame.Server
{
    public class Connection : Node
    {
        private const int Port = 5000;
        private Queue<Player> Players { get; } = new();
        private List<Room> Rooms { get; } = new();
        private NetworkedMultiplayerENet Server { get; } = new();
        public bool IsLive => Server.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
        public bool IsServer => CustomMultiplayer.IsNetworkServer();

        
        public override void _Ready()
        {
            Error err = Server.CreateServer(5000);
            if (err != Error.Ok) GD.PushError(err.ToString());
            CustomMultiplayer = new MultiplayerAPI {NetworkPeer = Server, RootNode = this};
        }

        [Master]
        public void OnNetworkPeerConnected(IEnumerable<SetCodes> deckList)
        {
            Players.Enqueue(new Player(CustomMultiplayer.GetRpcSenderId(), deckList));
        }

        public override void _Process(float delta)
        {
            if (CustomMultiplayer.HasNetworkPeer()) CustomMultiplayer.Poll();
            if (Players.Count > 1) { CreateRoom(); }
        }

        private void CreateRoom()
        {
            // This will likely need review when we build our GUI since it depends on NodePaths in the tree
            Player player1 = Players.Dequeue();
            Player player2 = Players.Dequeue();
            string count = Rooms.Count.ToString();
            Room room = new(player1, player2) {Name = count, CustomMultiplayer = CustomMultiplayer};
            RpcId(player1.Id, "CreateRoom", count);
            RpcId(player2.Id, "CreateRoom", count);
            Rooms.Add(room);
            AddChild(room);
        }

        public override void _ExitTree()
        {
            Server?.CloseConnection();
            QueueFree();
        }
    }
}
using Godot;
using System;

namespace CardGame.Client
{
    public class Connection : Node
    {
        private const string ServerAddress = "127.0.0.1";
        private const int ServerPort = 5000;
        private readonly NetworkedMultiplayerENet Client = new();
        public bool IsLive => Client.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
        public bool IsClient => !CustomMultiplayer.IsNetworkServer();

        public override void _Ready()
        {
            Error err = Client.CreateClient(ServerAddress, ServerPort);
            if (err != Error.Ok)
            {
                GD.PushError(err.ToString());
            }

            CustomMultiplayer = new MultiplayerAPI {RootNode = this, NetworkPeer = Client};
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
            Client?.CloseConnection();
        }
    }
}
using Godot;

namespace CardGame.Server
{
    public class Connection : Node
    {
        private const int Port = 5000;
        private readonly NetworkedMultiplayerENet Server = new();
        public bool IsLive => Server.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
        public bool IsServer => CustomMultiplayer.IsNetworkServer();

        public override void _Ready()
        {
            Error err = Server.CreateServer(5000);
            if (err != Error.Ok)
            {
                GD.PushError(err.ToString());
            }
            CustomMultiplayer = new MultiplayerAPI {RootNode = this, NetworkPeer = Server};
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
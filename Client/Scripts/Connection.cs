using Godot;
using Godot.Collections;

namespace CardGame.Client
{
	public class Connection : Node
	{
		private const string ServerAddress = "127.0.0.1";
		private const int ServerPort = 5000;
		private readonly NetworkedMultiplayerENet _client = new();
		public bool IsLive => _client.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
		public bool IsClient => !CustomMultiplayer.IsNetworkServer();
		public Array<SetCodes> DeckList;

		public override void _Ready()
		{
			Error err = _client.CreateClient(ServerAddress, ServerPort);
			if (err != Error.Ok) GD.PushError(err.ToString());
			CustomMultiplayer = new MultiplayerAPI {NetworkPeer = _client, RootNode = this};
			CustomMultiplayer.Connect("connected_to_server", this, nameof(OnConnectedToServer));
		}

		public void OnConnectedToServer() { RpcId(1, "OnNetworkPeerConnected", DeckList); }

		[Puppet]
		public void CreateRoom(string name) { AddChild(new Room(Scenes.Room(), name, CustomMultiplayer), true); }

		public override void _Process(float delta) { if (CustomMultiplayer.HasNetworkPeer()) CustomMultiplayer.Poll(); }

		public override void _ExitTree()
		{
			_client?.CloseConnection();
			QueueFree();
		}
	}
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGame.Client
{
	public class Connection : Node
	{
		private readonly PackedScene RoomScene = GD.Load<PackedScene>("res://Client/Room/Room.tscn");
		private const string ServerAddress = "127.0.0.1";
		private const int ServerPort = 5000;
		private readonly NetworkedMultiplayerENet Client = new NetworkedMultiplayerENet();
		public bool IsLive => Client.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected;
		public bool IsClient => !CustomMultiplayer.IsNetworkServer();

		public override void _Ready()
		{
			Error err = Client.CreateClient(ServerAddress, ServerPort);
			if (err != Error.Ok)
			{
				GD.PushError(err.ToString());
			}

			CustomMultiplayer = new MultiplayerAPI {NetworkPeer = Client};
			CustomMultiplayer.SetRootNode(this);
			CustomMultiplayer.Connect("connected_to_server", this, nameof(OnConnectedToServer));
		}

		public void OnConnectedToServer()
		{
			Array<SetCodes> deckList = new Array<SetCodes>();
			for (int i = 0; i < 40; i++)
			{
				deckList.Add(SetCodes.NullCard);
			}
			RpcId(1, "OnNetworkPeerConnected", deckList);
		}

		[Puppet]
		public void CreateRoom(string roomName)
		{
			Room room = (Room) RoomScene.Instance();
			room.Name = roomName;
			room.CustomMultiplayer = CustomMultiplayer;
			AddChild(room, true);
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
			QueueFree();
		}
	}
}

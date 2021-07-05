using System;
using System.Reflection;
using CardGame.Client;
using Godot;

namespace CardGame
{
	public class Players : EventArgs
	{
		public Room Room1 { get; }
		public Room Room2 { get; }
		public Participant Player1 { get; }
		public Participant Player2 { get; }

		public Players(Room room1, Room room2)
		{
			Room1 = room1;
			Room2 = room2;
			Player1 = (Participant) typeof(Room).GetProperty("Player", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(room1);
			Player2 = (Participant) typeof(Room).GetProperty("Player", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(room2);
		}
	}
	public class Main : Node
	{
		public event EventHandler<Players> GameBegun = (sender, args) => { };
		public event EventHandler RoomsUpdated = (sender, args) => { };
		private Room _room1; [Export] private bool _room1IsVisible;
		private Room _room2; [Export] private bool _room2IsVisible;
		private int _rooms;
		private int _roomUpdates;


		public override void _Ready()
		{
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
		}

		public void OnNodeAdded(Node node)
		{
			switch (node)
			{
				case Room room:
				{
					_rooms++;
					room.GameUpdated += OnRoomUpdated;
					// ReSharper disable once ConvertIfStatementToSwitchStatement
					if (_rooms == 1) _room1 = room;
					if (_rooms == 2) _room2 = room;
					bool visible = _rooms == 1 ? _room1IsVisible : _room2IsVisible;
			
					room.GetNode<Spatial>("Room/Table").Visible = visible;
					room.GetNode<Control>("Room/GUI").Visible = visible;
					room.GetNode<Spatial>("Cards").Visible = visible;

					if (_rooms != 2) return;
					GameBegun.Invoke(null, new Players(_room1, _room2));
					break;
				}
			}
		}

		public override void _Input(InputEvent gameEvent)
		{
			if (gameEvent is not InputEventKey {Pressed: true} key) return;
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch ((KeyList) key.Scancode)
			{
				case KeyList.S:
				{
					SetVisibility(_room1);
					SetVisibility(_room2);
					break;
				}
			}
		}

		private static void SetVisibility(Node room)
		{
			room.GetNode<Spatial>("Room/Table").Visible = !room.GetNode<Spatial>("Room/Table").Visible;
			room.GetNode<Control>("Room/GUI").Visible = !room.GetNode<Control>("Room/GUI").Visible;
			room.GetNode<Spatial>("Cards").Visible = !room.GetNode<Spatial>("Cards").Visible;
		}

		private void OnRoomUpdated(object sender, object args)
		{
			_roomUpdates++;
			if (_roomUpdates != 2) return;
			_roomUpdates = 0;
			RoomsUpdated.Invoke(null, null);
		}
	}
}

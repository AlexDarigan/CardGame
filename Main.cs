using System;
using CardGame.Client;
using Godot;
using JetBrains.Annotations;
using File = System.IO.File;

namespace CardGame
{
	public class Main : Node
	{
		[Signal] public delegate void GameBegun();
		[Signal] public delegate void RoomsUpdated();
		[Export()] private bool _room1IsVisible = false;
		[Export()] private bool _room2IsVisible = false;
		private RoomView _room1;
		private RoomView _room2;
		private int _rooms = 0;
		private int _roomUpdates = 0;

		
		public override void _Ready()
		{
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
		}

		public void OnNodeAdded(Node node)
		{
			switch (node)
			{
				case RoomView roomView:
				{
					_rooms++;
					// ReSharper disable once ConvertIfStatementToSwitchStatement
					if (_rooms == 1) _room1 = roomView;
					if (_rooms == 2) _room2 = roomView;
					bool visible = _rooms == 1 ? _room1IsVisible : _room2IsVisible;
					roomView.Visible = visible;
					if (_rooms != 2) return;
					EmitSignal(nameof(GameBegun));
					break;
				}
				case Room room:
					room.Connect(nameof(Room.Updated), this, nameof(OnRoomUpdated));
					break;
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

		private static void SetVisibility(Spatial room)
		{
			room.Visible = !room.Visible;
			room.GetNode<Control>("GUI").Visible = !room.GetNode<Control>("GUI").Visible;
		}

		public void OnRoomUpdated(States states)
		{
			_roomUpdates++;
			if (_roomUpdates != 2) return;
			_roomUpdates = 0;
			EmitSignal(nameof(RoomsUpdated));
		}
	}
}

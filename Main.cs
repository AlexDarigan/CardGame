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
		[Export()] private bool _room1IsVisible = false;
		[Export()] private bool _room2IsVisible = false;
		private RoomView _room1;
		private RoomView _room2;
		private int _rooms = 0;

		
		public override void _Ready()
		{
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
		}

		public void OnNodeAdded(Node node)
		{
			if (node is not RoomView room) return;
			_rooms++;
			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (_rooms == 1) _room1 = room;
			if (_rooms == 2) _room2 = room;

			bool visible = _rooms == 1 ? _room1IsVisible: _room2IsVisible;
			room.Visible = visible;
			room.GetNode<Control>("GUI").Visible = visible;
			
			if (_rooms != 2) return;
			
			GetTree().Disconnect("node_added", this, nameof(OnNodeAdded));
			EmitSignal(nameof(GameBegun));
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
				
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static void SetVisibility(Spatial room)
		{
			room.Visible = !room.Visible;
			room.GetNode<Control>("GUI").Visible = !room.GetNode<Control>("GUI").Visible;
		}
	}
}

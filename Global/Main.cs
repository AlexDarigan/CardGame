using System;
using System.Reflection;
using CardGame.Client;
using Godot;
using Godot.Collections;
using JetBrains.Annotations;
using Array = System.Array;

namespace CardGame
{
	public class Players : EventArgs
	{
		public Room Room1 { get; }
		public Room Room2 { get; }
		public Player Player1 { get; }
		public Player Player2 { get; }
		
		public Players(Room room1, Room room2)
		{
			Room1 = room1;
			Room2 = room2;
			Player1 = (Player) typeof(Room).GetProperty("Player", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(room1);
			Player2 = (Player) typeof(Room).GetProperty("Player", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(room2);
		}
	}
	public class Main : Node
	{
		// Events
		public event EventHandler<Players> GameBegun = (sender, args) => { };
		public static event EventHandler RoomsUpdated = (sender, args) => { };
		
		// Exports
		[UsedImplicitly][Export] private bool _room1IsVisible;
		[UsedImplicitly][Export] private bool _room2IsVisible;
		[Export] public Godot.Collections.Array<SetCodes> DeckList1 = DefaultDeck();
		[Export] public Godot.Collections.Array<SetCodes> DeckList2 = DefaultDeck();

		private Room _room1; 
		private Room _room2; 
		private int _rooms;
		private static int _roomUpdates;
		
		public override void _Ready()
		{
			GetNode<Connection>("Client1").DeckList = DeckList1;
			GetNode<Connection>("Client2").DeckList = DeckList2;
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
		}

		private static Array<SetCodes> DefaultDeck()
		{
			Array<SetCodes> deckList = new Array<SetCodes>();
			for (int i = 0; i < 20; i++)
			{
				deckList.Add(SetCodes.AlphaBioShocker);
				deckList.Add(SetCodes.AlphaQuestReward);
			}

			return deckList;
		}

		public void OnNodeAdded(Node node)
		{
			switch (node)
			{
				case Room room:
				{
					_rooms++;
					// ReSharper disable once ConvertIfStatementToSwitchStatement
					if (_rooms == 1) _room1 = room;
					if (_rooms == 2) _room2 = room;
					bool visible = _rooms == 1 ? _room1IsVisible : _room2IsVisible;
			
					room.GetNode<Spatial>("Room/Table").Visible = visible;
					room.GetNode<Control>("Room/GUI").Visible = visible;
					room.GetNode<Spatial>("Cards").Visible = visible;
					room.GetNode<Button>("Room/RivalHeart").MouseFilter =
						visible ? Control.MouseFilterEnum.Pass : Control.MouseFilterEnum.Ignore;

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
			room.GetNode<Button>("Room/RivalHeart").MouseFilter = room.GetNode<Button>("Room/RivalHeart").MouseFilter ==
				Control.MouseFilterEnum.Ignore ? Control.MouseFilterEnum.Pass : Control.MouseFilterEnum.Ignore;
		}

		public static void OnRoomUpdated()
		{
			_roomUpdates++;
			if (_roomUpdates != 2) return;
			_roomUpdates = 0;
			RoomsUpdated.Invoke(null, null);
		}
	}
}

using System;
using System.Collections.Generic;
using CardGame.Client;
using Godot;
using Godot.Collections;

namespace CardGame
{
	public class Main : Node
	{
		public event EventHandler GameBegun;
		public static event EventHandler RoomsUpdated = (sender, args) => { };
		private enum Visibility { IsVisible, IsNotVisible }
		[Export()] private Array<Visibility> RoomsVisible { get; set; } = new(Visibility.IsVisible, Visibility.IsNotVisible);
		public Array<SetCodes> DeckList1 { get; set; }= DefaultDeck();
		public Array<SetCodes> DeckList2 { get; set; }= DefaultDeck();
		public List<Room> Rooms { get; } = new();
		private static int _roomUpdates;

		public override void _Ready()
		{
			GetNode<Connection>("Client1").DeckList = DeckList1;
			GetNode<Connection>("Client2").DeckList = DeckList2;
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
		}

		private static Array<SetCodes> DefaultDeck()
		{
			Array<SetCodes> deckList = new();
			for (int i = 0; i < 20; i++)
			{
				deckList.Add(SetCodes.AlphaBioShocker);
				deckList.Add(SetCodes.AlphaQuestReward);
			}

			return deckList;
		}

		public async void OnNodeAdded(Node node)
		{
			if (node is not Room room) { return;}
			Rooms.Add(room);
			bool visible = RoomsVisible[Rooms.Count - 1] == Visibility.IsVisible;
			await ToSignal(room, "ready");
			room.Table.Visible = visible;
			room.Text.Visible = visible;
			room.Cards.Visible = visible;
			room.Link.Visible = visible;
			if (Rooms.Count == 2) { GameBegun?.Invoke(null, null); }
		}

		public override void _Input(InputEvent gameEvent)
		{
			if (gameEvent is not InputEventKey {Pressed: true, Scancode: (int) KeyList.S}) { return; }
			SetVisibility(Rooms[0]);
			SetVisibility(Rooms[1]);
		}

		private static void SetVisibility(Room room)
		{
			room.Table.Visible = !room.Table.Visible;
			room.Text.Visible = !room.Text.Visible;
			room.Cards.Visible = !room.Cards.Visible;
			room.Link.Visible = !room.Link.Visible;
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

using System;
using CardGame.Client;
using Godot;

namespace CardGame
{
	public class Main : Node
	{
		
		[Signal] public delegate void GameBegun();
		[Export()] private bool Room1IsVisible;
		[Export()] private bool Room2IsVisible;
		private Room Room1;
		private Room Room2;
		private int rooms = 0;

		
		public override void _Ready()
		{
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
		}

		public void OnNodeAdded(Node node)
		{
			if (node is not Room room) return;
			rooms++;
			switch (rooms)
			{
				case 1:
					Room1 = room;
					break;
				case 2:
					Room2 = room;
					break;
			}
			bool visible = rooms == 1 ? Room1IsVisible: Room2IsVisible;
			room.Visible = visible;
			room.GetNode<Control>("GUI").Visible = visible;
			Console.WriteLine($"{node.Name} added");
			
			if (rooms != 2) return;
			
			GetTree().Disconnect("node_added", this, nameof(OnNodeAdded));
			EmitSignal(nameof(GameBegun));
			Console.WriteLine("Emitting Signal");
		}
		
		public override void _Input(InputEvent gameEvent)
		{
			if (gameEvent is not InputEventKey key) return;
			if (!key.Pressed || key.Scancode is not (uint) KeyList.S) return;
			Console.WriteLine("Switching visbility");
			Room1.Visible = !Room1.Visible;
			Room1.GetNode<Control>("GUI").Visible = !Room1.GetNode<Control>("GUI").Visible;
			Room2.Visible = !Room2.Visible;
			Room2.GetNode<Control>("GUI").Visible = !Room2.GetNode<Control>("GUI").Visible;
			Console.WriteLine($"Room1.Visible? {Room1.Visible}");
			Console.WriteLine($"Room2.Visible? {Room2.Visible}");
		}
	}
}

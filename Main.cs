using System;
using CardGame.Client;
using Godot;

namespace CardGame
{
	public class Main : Node
	{
		public override void _Ready()
		{
			Console.WriteLine("Hello World");
			GD.Print("Hello World");
		}
	}
}

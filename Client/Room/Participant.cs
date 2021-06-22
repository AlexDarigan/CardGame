using System.IO.Ports;
using Godot;

namespace CardGame.Client
{
	public class Participant: Node
	{
		public Position3D Deck { get; private set; }
		public Position3D Discard { get; private set; }
		public Node Hand { get; private set; }
		public Node Units { get; private set; }
		public Node Support { get; private set; }

		public override void _Ready()
		{
			Deck = GetNode <Position3D> ("Deck");
			Deck = GetNode <Position3D> ("Discard");
			Hand = GetNode <Node>("Hand");
			Units = GetNode <Node>("Units");
			Support = GetNode <Node>("Support");

		}
	}
}

using System.IO.Ports;
using Godot;

namespace CardGame.Client
{
	public class Participant: Spatial
	{
		public Node Deck { get; private set; }
		public Spatial Discard { get; private set; }
		public Node Hand { get; private set; }
		public Node Units { get; private set; }
		public Node Support { get; private set; }

		public override void _Ready()
		{
			Deck = GetNode <Node> ("Deck");
			Discard = GetNode <Spatial> ("Discard");
			Hand = GetNode <Node>("Hand");
			Units = GetNode <Node>("Units");
			Support = GetNode <Node>("Support");
		}
	}
}

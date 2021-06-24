using System;
using System.IO.Ports;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client
{
	public class Participant: Spatial
	{
		public States State;
		public bool IsClient = false;
		public int Health = 8000;
		public Zone Deck { get; private set; }
		public Zone Discard { get; private set; }
		public Zone Hand { get; private set; }
		public Zone Units { get; private set; }
		public Zone Support { get; private set; }
		
		public override void _Ready()
		{
			Deck = GetNode <Zone> ("Deck");
			Discard = GetNode <Zone> ("Discard");
			Hand = GetNode <Zone>("Hand");
			Units = GetNode <Zone>("Units");
			Support = GetNode <Zone>("Support");
		}

	}
}

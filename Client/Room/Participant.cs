using System;
using System.Xml;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class Participant : Object
	{
		public int Health = 8000;
		public States State;
		public readonly bool IsClient;
		public readonly Zone Deck = null;
		public readonly Zone Discard = null;
		public readonly Zone Hand = null;
		public readonly Zone Units = null;
		public readonly Zone Support = null;
		public readonly Room.Declaration Declare;


		public Participant(Node view, Room.Declaration declare)
		{
			Declare = declare;
			IsClient = view.Name == "Player";
			foreach (Node zone in view.GetChildren())
			{
				Set(zone.Name, new Zone(zone));
			}
		}

		public void Update(States state) => State = state;

		public void OnCardPressed(Card pressed)
		{
			switch (pressed.CardState)
			{
				case CardState.Deploy:
					Declare("Deploy", pressed.Id);
					State = States.Passive;
					break;
				case CardState.AttackUnit:
					break;
				case CardState.AttackPlayer:
					break;
				case CardState.Set:
					Console.WriteLine("Sending State");
					Declare("SetFaceDown", pressed.Id);
					State = States.Passive;
					break;
				case CardState.Activate:
					break;
				case CardState.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}

using System;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class Participant
	{
		public int Health = 8000;
		public States State;
		public readonly bool IsClient;
		public readonly Zone Deck;
		public readonly Zone Discard;
		public readonly Zone Hand;
		public readonly Zone Units;
		public readonly Zone Support;
		public readonly Declaration Declare;


		public Participant(Node view, Declaration declare)
		{
			Declare = declare;
			IsClient = view.Name == "Player";
			Deck = new Zone(view.GetNode<Spatial>("Deck"));
			Discard = new Zone(view.GetNode<Spatial>("Discard"));
			Hand = new Zone(view.GetNode<Spatial>("Hand"));
			Units = new Zone(view.GetNode<Spatial>("Units"));
			Support = new Zone(view.GetNode<Spatial>("Support"));
		}

		public void Update(States state) => State = state;

		public void OnCardPressed(Card pressed)
		{
			switch (pressed.CardState)
			{
				case CardState.Deploy:
					Declare(CommandId.Deploy, pressed.Id);
					State = States.Passive;
					break;
				case CardState.AttackUnit:
					break;
				case CardState.AttackPlayer:
					break;
				case CardState.Set:
					Declare(CommandId.SetFaceDown, pressed.Id);
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

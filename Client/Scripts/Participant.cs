using System;
using Godot;

namespace CardGame.Client
{
    public class Participant
    {
        public readonly Zone Deck;
        public readonly Declaration Declare;
        public readonly Zone Discard;
        public readonly Zone Hand;
        public readonly bool IsClient;
        public readonly Zone Support;
        public readonly Zone Units;
        public int Health = 8000;
        public States State = States.Passive;


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

        public void Update(States state)
        {
            State = state;
        }

        public void OnCardPressed(Card pressed)
        {
            if (State == States.Passive) return;
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
using System;
using Godot;

namespace CardGame.Client
{
    public class Participant
    {
        public event Declaration Declare;
        public Zone Deck { get; }
        public Zone Discard { get; }
        public Zone Hand { get; }
        public Zone Support { get; }
        public Zone Units { get; }
        public object Declared { get; set; }

        public int Health = 8000;
        public States State = States.Passive;
        public readonly bool IsClient;

        public Participant(Node view)
        {
            IsClient = view.Name == "Player";
            Deck = new Zone(view.GetNode<Spatial>("Deck"));
            Discard = new Zone(view.GetNode<Spatial>("Discard"));
            Hand = new Zone(view.GetNode<Spatial>("Hand"));
            Units = new Zone(view.GetNode<Spatial>("Units"));
            Support = new Zone(view.GetNode<Spatial>("Support"));
        }

        public void Update(States state) { State = state; }

        public void OnCardPressed(Card pressed)
        {
            if (State == States.Passive) return;
            switch (pressed.CardState)
            {
                case CardState.Deploy:
                    Declare?.Invoke(CommandId.Deploy, pressed.Id);
                    State = States.Passive;
                    break;
                case CardState.AttackUnit:
                    break;
                case CardState.AttackPlayer:
                    break;
                case CardState.Set:
                    Declare?.Invoke(CommandId.SetFaceDown, pressed.Id);
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
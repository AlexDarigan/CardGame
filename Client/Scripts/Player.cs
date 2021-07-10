using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
    public class Player: Participant
    {
        public event Declaration Declare;
        public States State { get; private set; } = States.Passive;
        private Mouse Mouse { get; }
        private Card Attacker { get; set; }
        
        public Player(Mouse mouse)
        {
            Mouse = mouse;
            Deck = new Zone(new Vector3(10.5f, 0, 8.25f), new Vector3(0, .034f, 0), new Vector3(0, 0, 180));
            Discard = new Zone(new Vector3(10.5f, 0.5f, 4.5f), new Vector3(0, 0.04f, 0), new Vector3(0, 0, 0));
            Hand = new Zone(new Vector3(0, 4, 11), new Vector3(1.1f, 0, 0), new Vector3(33, 0, 0));
            Units = new Zone(new Vector3(0, .33f, 3), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 0));
            Supports = new Zone(new Vector3(0, .33f, 7), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 180));
        }

        public void OnGameUpdated(Room room, States states)
        {
            State = states;
        }
        
        
        public void OnCardPressed(Card pressed)
        {
            if (State == States.Passive) return;
            if (pressed == Attacker)
            {
                Attacker = null;
                Mouse.OnAttackCancelled();
                return;
            }
            if (Attacker is not null)
            {
                // Add a check here to make sure the defender is a valid attack target
                Console.WriteLine($"{Attacker} is attacking {pressed}");
                Mouse.OnAttackCancelled(); // Committed?
                Declare?.Invoke("DeclareAttack", Attacker.Id, pressed.Id);
                Attacker = null;
                return;
            }
            
            switch (pressed.CardState)
            {
                case CardState.Deploy:
                    Declare?.Invoke("Deploy", pressed.Id);
                    State = States.Passive;
                    break;
                case CardState.AttackUnit:
                    Console.WriteLine("Attacking");
                    Attacker = pressed;
                    Mouse.OnAttackDeclared();
                    break;
                case CardState.AttackPlayer:
                    break;
                case CardState.Set:
                    Declare?.Invoke("SetFaceDown", pressed.Id);
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

        
        public void PassPlay() { }

        public void EndTurn() { if (State == States.IdleTurnPlayer) { Declare("EndTurn"); } }
        
    }
}
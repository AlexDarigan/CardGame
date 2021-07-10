using System;
using System.Collections.ObjectModel;
using Godot;
using System.Collections.Generic;

namespace CardGame.Client
{
    public class Player: Participant
    {
        private delegate States Play(Card card);
        private ReadOnlyDictionary<CardState, Play> Plays { get; }
        public event Action OnAttackDeclared;
        public event Action OnAttackCancelled;
        public event Declaration Declare;
        public States State { get; set; } = States.Passive;
        private Card Attacker { get; set; }

      
        public Player()
        {
            Plays = new ReadOnlyDictionary<CardState, Play>(new Dictionary<CardState, Play> {
                {CardState.Deploy, Deploy}, {CardState.SetFaceDown, SetFaceDown}, {CardState.Activate, Activate}, 
                {CardState.AttackPlayer, AttackPlayer}, {CardState.AttackUnit, AttackUnit}, {CardState.None, None}});
            
            Deck = new Zone(new Vector3(10.5f, 0, 8.25f), new Vector3(0, .034f, 0), new Vector3(0, 0, 180));
            Discard = new Zone(new Vector3(10.5f, 0.5f, 4.5f), new Vector3(0, 0.04f, 0), new Vector3(0, 0, 0));
            Hand = new Zone(new Vector3(0, 4, 11), new Vector3(1.1f, 0, 0), new Vector3(33, 0, 0));
            Units = new Zone(new Vector3(0, .33f, 3), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 0));
            Supports = new Zone(new Vector3(0, .33f, 7), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 180));
        }
        
        public void OnCardPressed(Card pressed)
        {
            if (pressed == Attacker) { CancelAttack(); }
            else if (Attacker is not null) { CommitAttack(pressed); }
            else if(State != States.Passive) { Plays[pressed.CardState](pressed); }
        }
        
        private States Deploy(Card card)
        {
            Declare?.Invoke(CommandId.Deploy, card.Id);
            return States.Passive;
        }

        private States AttackUnit(Card card)
        {
            // Could we make this async?
            Attacker = card;
            OnAttackDeclared?.Invoke();
            return State; // This seems wrong? Targeting maybe?
        }

        private States AttackPlayer(Card card) { return State; }

        private States SetFaceDown(Card card)
        {
            Declare?.Invoke(CommandId.SetFaceDown, card.Id);
            return State;
        }

        private States Activate(Card card) { return State; }

        private void CommitAttack(Card card)
        {
            // Is Defender Valid
            OnAttackCancelled?.Invoke(); // Committed?
            Declare?.Invoke(CommandId.DeclareAttack, Attacker.Id, card.Id);
            Attacker = null;
        }

        private void CancelAttack()
        {
            Attacker = null;
            OnAttackCancelled?.Invoke();
        }

        private States None(Card card) { return State; }
        
        public void PassPlay() { }

        public void EndTurn() { if (State == States.IdleTurnPlayer) { Declare?.Invoke(CommandId.EndTurn); } }
        
    }
}
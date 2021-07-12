using System;
using System.Collections.ObjectModel;
using Godot;
using System.Collections.Generic;
using CardGame.Client.Views;

namespace CardGame.Client
{
    public class Player: Participant
    {
        private delegate States Play(Card card);
        private ReadOnlyDictionary<CardStates, Play> Plays { get; }
        public event Action OnAttackDeclared;
        public event Action OnAttackCancelled;
        public event Declaration Declare;
        public States State { get; set; } = States.Passive;
        private Card Attacker { get; set; }

      
        public Player()
        {
            Plays = new ReadOnlyDictionary<CardStates, Play>(new Dictionary<CardStates, Play> {
                {CardStates.Deploy, Deploy}, {CardStates.SetFaceDown, SetFaceDown}, {CardStates.Activate, Activate}, 
                {CardStates.AttackPlayer, AttackPlayer}, {CardStates.AttackUnit, AttackUnit}, {CardStates.None, None}});
        }
        
        public void OnRivalAvatarPressed()
        {
            // Note: When testing two scenes, have to make sure focus isn't being stolen by the other copy
            if (Attacker is not null && Attacker.CardState.Get() == CardStates.AttackPlayer) { CommitAttack(); }
        }
        
        public void OnCardPressed(Card pressed)
        {
            if (pressed == Attacker) { CancelAttack(); }
            else if (Attacker is not null && Attacker.CardState.Get() == CardStates.AttackUnit) { CommitAttack(pressed); }
            else if(State != States.Passive) { Plays[pressed.CardState.Get()](pressed); }
        }
        
        private States Deploy(Card card)
        {
            Declare?.Invoke(CommandId.Deploy, card.Id.Get());
            return States.Passive;
        }

        private States AttackUnit(Card card)
        {
            // Could we make this async?
            Attacker = card;
            OnAttackDeclared?.Invoke();
            return State; // This seems wrong? Targeting maybe?
        }

        private States AttackPlayer(Card card)
        {
            Attacker = card;
            OnAttackDeclared?.Invoke();
            return State;
        }

        private States SetFaceDown(Card card)
        {
            Declare?.Invoke(CommandId.SetFaceDown, card.Id.Get());
            return State;
        }

        private States Activate(Card card) { return State; }

        private void CommitAttack(Card card)
        {
            // Is Defender ValId.Get()
            OnAttackCancelled?.Invoke(); // Committed?
            Declare?.Invoke(CommandId.DeclareAttack, Attacker.Id.Get(), card.Id.Get());
            Attacker = null;
        }
        
        private void CommitAttack()
        {
            OnAttackCancelled?.Invoke(); // Committed?
            Declare?.Invoke(CommandId.DeclareDirectAttack, Attacker.Id.Get());
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
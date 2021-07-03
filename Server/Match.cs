using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CardGame.Server
{
    public class Match
    {
        /*
         * Match is a Rules-Enforced Script. This means any action called from here will have
         * to be a legal play otherwise the player invoking the illegal play will be disqualified.
         * When we want to test actions independently of a script like this, we will just invoke them
         * directly on their owners (either a player or a skill).
         */

        public bool GameOver { get; private set; }
        private readonly Cards _cards;
        private readonly List<SkillState> Link = new();
        private readonly Enqueue Queue;
        private readonly Action _update;

        public Match(Player player1, Player player2, Cards cards, Action update, Enqueue queue)
        {
            _update = update;
            Queue = queue;
            GameOver = false;
            _cards = cards;
            player1.Opponent = player2;
            player2.Opponent = player1;
        }

        public void Begin(List<Player> players)
        {
            // Could place this directly inside room (we'd remove the register dependency for a start)
            foreach (Player player in players)
            {
                player.LoadDeck(_cards).QueueOnClients(Queue);
                for (int i = 0; i < 7; i++) player.Draw().QueueOnClients(Queue);
            }

            players[0].State = States.IdleTurnPlayer;
            Update();
        }

        public void Draw(Player player)
        {
            if(Disqualified((player.State != States.IdleTurnPlayer), player, Illegal.Draw)) { return; }

            if (player.Deck.Count == 0)
            {
                OnGameOver(player.Opponent, player);
                Update();
                return;
            }

            player.Draw().QueueOnClients(Queue);
            Update();
        }

        public void Deploy(Player player, Card unit)
        {
            if(Disqualified(unit.CardState != CardState.Deploy, player, Illegal.Deploy)) { return; }
            player.Deploy(unit).QueueOnClients(Queue);
            Update();
        }

        public void DeclareAttack(Player player, Card attacker, Card defender)
        {
            if(Disqualified(attacker.CardState != CardState.AttackUnit, player, Illegal.AttackUnit)) { return; }

            static void DamageCalculation(Card winner, Card loser)
            {
                loser.Controller.Health -= winner.Power - loser.Power;
                loser.Controller.Units.Remove(loser);
                loser.Owner.Graveyard.Add(loser);
            }

            if (attacker.Power > defender.Power) { DamageCalculation(attacker, defender); }
            else if (defender.Power > attacker.Power) { DamageCalculation(defender, attacker); }
            attacker.IsReady = false;
            Update();
        }

        public void DeclareDirectAttack(Player player, Card attacker)
        {
            if(Disqualified(attacker.CardState != CardState.AttackPlayer, player, Illegal.AttackPlayer)) { return; }
            player.Opponent.Health -= attacker.Power;
            if (player.Opponent.Health <= 0)
            {
                OnGameOver(player, player.Opponent);
            }
            attacker.IsReady = false;
            Update();
        }

        public void SetFaceDown(Player player, Card support)
        {
            if(Disqualified(support.CardState != CardState.Set, player, Illegal.SetFaceDown)) { return; }
            player.SetFaceDown(support).QueueOnClients(Queue);
            Update();
        }

        public void Activate(Player player, Card support)
        {
            
            if(Disqualified(support.CardState != CardState.Activate, player, Illegal.Activation)) { return; }
            Link.Add(support.Activate());
            Resolve();
            Update();
        }

        private void Resolve()
        {
            while (Link.Count > 0)
            {
                SkillState current = Link[Link.Count - 1];
                current.Execute();
                if (!current.IsDone()) continue;
                Link.Remove(current);
                
                // OnResolve
                // AddToCard
                // SpawnEvents?
                current.Controller.Supports.Remove(current.OwningCard);
                current.Owner.Graveyard.Add(current.OwningCard);

            }
        }

        public void PassPlay(Player player)
        {
            if(Disqualified(player.State != States.Active, player, Illegal.PassPlay)) { return; }
            // More Here
            Update();
        }
        
        public void EndTurn(Player player)
        {
            if(Disqualified(player.State != States.IdleTurnPlayer, player, Illegal.EndTurn)) { return; }
            player.State = States.Passive;
            player.Opponent.State = States.IdleTurnPlayer;
            Draw(player.Opponent);
            foreach (Card card in player.Units) card.IsReady = true;
            foreach (Card card in player.Supports) card.IsReady = true;
            Update();
        }

        private bool Disqualified(bool condition, Player player, Illegal reason)
        {
            if (GameOver) return true;
            player.ReasonPlayerWasDisqualified = reason;
            return condition;
        }

        private void Update()
        {
            foreach (Card card in _cards) { card.Update(); }
            _update();
        }

        private void OnGameOver(Player winner, Player loser)
        {
            winner.State = States.Winner;
            loser.State = States.Loser;
            GameOver = true;
            Update();
        }
    }
}
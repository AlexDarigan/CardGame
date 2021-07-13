using System;
using System.Collections.Generic;
using CardGame.Server.Events;

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
        private Cards Cards { get; }
        private History History { get; } = new();
        private Link Link { get; } = new();
        private Enqueue Queue { get; }
        private Action UpdateClient { get; }

        public Match(Player player1, Player player2, Cards cards, Action updateClient, Enqueue queue)
        {
            UpdateClient = updateClient;
            Queue = queue;
            GameOver = false;
            Cards = cards;
            player1.Opponent = player2;
            player2.Opponent = player1;
        }

        public void Begin(List<Player> players, Enqueue loadDeck = null)
        {
            players[0].LoadDeck(Cards).QueueOnClients(Queue);
            players[1].LoadDeck(Cards).QueueOnClients(Queue);
            foreach (Player player in players) { for (int i = 0; i < 7; i++) { player.Draw().QueueOnClients(Queue); } }
            players[0].State = States.IdleTurnPlayer;
            Update();
        }
        
        public void Deploy(Player player, Card unit)
        {
            if(Disqualified(unit.CardStates != CardStates.Deploy, player, Illegal.Deploy)) { return; }
            Event gameEvent = player.Deploy(unit);
            History.Add(gameEvent);
            gameEvent.QueueOnClients(Queue);
            Update();
        }

        public void DeclareAttack(Player player, Card attacker, Card defender)
        {
            if(Disqualified(attacker.CardStates != CardStates.AttackUnit, player, Illegal.AttackUnit)) { return; }

            Event battle = new Battle(attacker, defender);
            History.Add(battle);
            battle.QueueOnClients(Queue);
            
            void DamageCalculation(Card winner, Card loser)
            {
                
                int difference = winner.Power - loser.Power;
                loser.Controller.Health -= difference;
                
                Event setHealth = new SetHealth(loser.Controller);
                History.Add(setHealth);
                setHealth.QueueOnClients(Queue);
                
                loser.Controller.Units.Remove(loser);
                loser.Owner.Graveyard.Add(loser);
                
                Event sentToGraveyard = new SentToGraveyard(loser);
                History.Add(sentToGraveyard);
                sentToGraveyard.QueueOnClients(Queue);

                if (loser.Controller.Health <= 0) { OnGameOver(winner.Controller, loser.Controller); }
            }


            if (attacker.Power > defender.Power) { DamageCalculation(attacker, defender); }
            else if (defender.Power > attacker.Power) { DamageCalculation(defender, attacker); }
            attacker.IsReady = false;
            Update();
        }

        public void DeclareDirectAttack(Player player, Card attacker)
        {
            
            if(Disqualified(attacker.CardStates != CardStates.AttackPlayer, player, Illegal.AttackPlayer)) { return; }
            player.Opponent.Health -= attacker.Power;

            Event directAttack = new DirectAttack(attacker);
            History.Add(directAttack);
            directAttack.QueueOnClients(Queue);
            
            Event setHealth = new SetHealth(player.Opponent);
            History.Add(setHealth);
            setHealth.QueueOnClients(Queue);
            
            if (player.Opponent.Health <= 0)
            {
                OnGameOver(player, player.Opponent);
            }
            
            attacker.IsReady = false;
            Update();
        }

        public void SetFaceDown(Player player, Card support)
        {
            if(Disqualified(support.CardStates != CardStates.SetFaceDown, player, Illegal.SetFaceDown)) { return; }

            Event setFaceDown = player.SetFaceDown(support);
            History.Add(setFaceDown);
            setFaceDown.QueueOnClients(Queue);
            
            Update();
        }

        public void Activate(Player player, Card support)
        {
            
            if(Disqualified(support.CardStates != CardStates.Activate, player, Illegal.Activation)) { return; }
            
            (Event activation, SkillState skillState) = support.Activate();
            History.Add(activation);
            activation.QueueOnClients(Queue);
            
            Link.Add(skillState);
            Link.Resolve();
            Update();
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
            player.EndTurn().QueueOnClients(Queue);
            if (player.Opponent.Deck.Count > 0) { player.Opponent.Draw().QueueOnClients(Queue); } else { OnGameOver(player, player.Opponent); }
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
            foreach (Card card in Cards) { card.Update(); }
            UpdateClient();
        }

        private void OnGameOver(Player winner, Player loser)
        {
            winner.State = States.Winner;
            loser.State = States.Loser;
            GameOver = true;
            
            Event gameOver = new GameOver(loser);
            History.Add(gameOver);
            gameOver.QueueOnClients(Queue);
            
            Update();
        }
    }
}
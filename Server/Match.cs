using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

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

        private readonly Action Update;
        private Player TurnPlayer;
        private bool _isGameOver;
        public Match(Player player1, Player player2, CardRegister cardRegister, Action update)
        {
            player1.Opponent = player2;
            player2.Opponent = player1;
            player1.LoadDeck(cardRegister);
            player2.LoadDeck(cardRegister);
            
            // This is not the most pleasing, maybe be better to do it as player.draw(count)?
            foreach (Player player in new List<Player>{player1, player2})
            {
                for (int i = 0; i < 7; i++)
                {
                    player.Draw();
                }
            }
            TurnPlayer = player1;
            TurnPlayer.State = Player.States.Idle;
            Update = update;
        }
        
        public void Draw(Player player)
        {
            if (player.State != Player.States.Idle || player != TurnPlayer)
            {
                Disqualify(player);
                return;
            }

            if (player.Deck.Count == 0)
            {
                GameOver(player.Opponent, player);
                return;
            }
            player.Draw();
            Update();
        }

        public void Deploy(Player player, Card unit)
        {
            if (player.State != Player.States.Idle || player != TurnPlayer || unit.CardType != CardType.Unit)
            {
                Disqualify(player);
                return;
            }

            player.Deploy(unit);
            Update();
        }

        public void DeclareAttack(Player player, Card attacker, Card defender)
        {
            if (player.State != Player.States.Idle || !player.Units.Contains(attacker) ||
                !player.Opponent.Units.Contains(defender) || !attacker.IsReady)
            {
                Disqualify(player);
                return;
            }

            static void DamageCalculation(Card winner, Card loser)
            {
                loser.Controller.Health -= winner.Power - loser.Power;
                loser.Controller.Units.Remove(loser);
                loser.Owner.Graveyard.Add(loser);
            }

            if (attacker.Power > defender.Power)
            {
                DamageCalculation(attacker, defender);
            }
            else if(defender.Power > attacker.Power)
            {
                DamageCalculation(defender, attacker);
            }

            Update();
        }
        
        public void DeclareDirectAttack(Player player, Card attacker)
        {
            if (player.State != Player.States.Idle || !player.Units.Contains(attacker) ||
                player.Opponent.Units.Count != 0 || !attacker.IsReady)
            {
                Disqualify(player);
                return;
            }

            player.Opponent.Health -= attacker.Power;
            Update();
        }

        public void SetFaceDown(Player player, Card support)
        {
            if (player.State != Player.States.Idle || player != TurnPlayer || support.CardType != CardType.Support)
            {
                Disqualify(player);
                return;
            }

            player.SetFaceDown(support);
            Update();
        }
        
        public void EndTurn(Player player)
        {
            if (player.State != Player.States.Idle || player != TurnPlayer)
            {
                Disqualify(player);
                return;
            }
            
            TurnPlayer = TurnPlayer.Opponent;
            TurnPlayer.State = Player.States.Idle;
            TurnPlayer.Opponent.State = Player.States.Passive;
            Draw(TurnPlayer);
            foreach (Card card in player.Units) { card.IsReady = true; }
            foreach (Card card in player.Supports) { card.IsReady = true; }
            Update();
        }

        private void Disqualify(Player player)
        {
            if (_isGameOver)
            {
                // The Player States are already invalid at this point so no reason to force the disqualification
                return;
            }
            player.Disqualified = true;
        }

        private void GameOver(Player winner, Player loser)
        {
            winner.State = Player.States.Winner;
            loser.State = Player.States.Loser;
            _isGameOver = true;
            Update();
        }
    }
}
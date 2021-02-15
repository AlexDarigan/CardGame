using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
            if (player.State != Player.States.Idle && player != TurnPlayer)
            {
                Disqualify(player);
                return;
            }
            
            player.Draw();
            Update();
        }

        public void EndTurn(Player player)
        {
            if (player.State != Player.States.Idle && player != TurnPlayer)
            {
                Disqualify(player);
                return;
            }

            TurnPlayer = TurnPlayer.Opponent;
            TurnPlayer.State = Player.States.Idle;
            TurnPlayer.Opponent.State = Player.States.Passive;
            TurnPlayer.Draw();
            Update();
        }

        public void Disqualify(Player player)
        {
            player.Disqualified = true;
        }
    }
}
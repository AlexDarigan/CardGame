using System;
using System.Collections.Generic;

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
        public Match(Action update)
        {
            Update = update;
        }

        public void Start(Player player1, Player player2)
        {
            foreach (Player player in new List<Player>{player1, player2})
            {
                for (int i = 0; i < 7; i++)
                {
                    Draw(player);
                }
            }
        }
        
        public void Draw(Player player)
        {
            player.Draw();
            Update();
        }
    }
}
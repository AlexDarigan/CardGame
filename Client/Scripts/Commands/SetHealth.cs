using System;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetHealth: Command
    {

        private bool IsPlayer { get; }
        private int NewHealth { get; }
       
        public SetHealth(bool isPlayer, int newHealth)
        {
            IsPlayer = isPlayer;
            NewHealth = newHealth;
        }
        
        protected override void Setup(CommandQueue gfx)
        {
            gfx.InterpolateCallback(this, 0.2f, nameof(SetPlayerHealth), gfx);
        }

        private void SetPlayerHealth(CommandQueue gfx)
        {
            Participant player = gfx.GetPlayer(IsPlayer);
            player.Health = NewHealth;
            Console.WriteLine($"Player's Health is {player.Health}");
        }
    }
}
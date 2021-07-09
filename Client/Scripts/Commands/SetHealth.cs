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
        
        protected override void Setup(Room room)
        {
            // We'd probably use the GUI HealthBar for this?
            room.Gfx.InterpolateCallback(this, 0.2f, nameof(SetPlayerHealth), room);
        }

        private void SetPlayerHealth(Room room)
        {
            Participant player = room.GetPlayer(IsPlayer);
            player.Health = NewHealth;
        }
    }
}
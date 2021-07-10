﻿namespace CardGame.Client.Commands
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
            Participant participant = room.GetPlayer(IsPlayer);
            participant.Health = NewHealth;
            Views.HealthBar healthBar = IsPlayer ? room.RoomView.PlayerHealth : room.RoomView.RivalHealth;
            healthBar.DisplayHealth(participant, room);
        }
    }
}

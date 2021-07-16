using System;

namespace CardGame.Client.Commands
{
    public class SetHealth: Command
    {
        private Who Who { get; }
        private int NewHealth { get; }
       
        public SetHealth(Who who, int newHealth)
        {
            Who = who;
            NewHealth = newHealth;
        }
        
        protected override void Setup(Room room)
        {
            Participant player = Who == Who.Player ? room.Player : room.Rival;
            player.SetHealth(NewHealth, room);
        }
    }
}

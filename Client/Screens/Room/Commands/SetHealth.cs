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
            room.GetPlayer(IsPlayer).SetHealth(NewHealth, room);
        }
    }
}

namespace CardGame.Client.Commands
{
    public class SetHealth: Command
    {
        private int NewHealth { get; }
       
        public SetHealth(Who who, int newHealth)
        {
            Who = who;
            NewHealth = newHealth;
        }
        
        protected override void Setup(Room room)
        {
            Player.SetHealth(NewHealth, room);
        }
    }
}

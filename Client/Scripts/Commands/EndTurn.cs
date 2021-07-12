namespace CardGame.Client.Commands
{
    public class EndTurn: Command
    {
        public EndTurn()
        {
            
        }

        protected override void Setup(Room room)
        {
            room.TurnCounter.NextTurn();
        }
    }
}
namespace CardGame.Client.Commands
{
    public class GameOver: Command
    {
        private bool IsClientLoser { get; }
        
        public GameOver(bool isClientLoser) { IsClientLoser = isClientLoser; }
        
        protected override void Setup(Room room)
        { 
            room.Text.DisplayGameOver(IsClientLoser);
        }
    }
}
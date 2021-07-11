namespace CardGame.Client.Commands
{
    public class GameOver: Command
    {
        private const string LoseText = "You Lose!";
        private const string WinText = "You Win!";
        private bool IsClientLoser { get; }
        
        public GameOver(bool isClientLoser) { IsClientLoser = isClientLoser; }
        
        protected override void Setup(Room room)
        {
            string text = IsClientLoser ? LoseText : WinText;
            room.RoomView.GameOver.Text = text;
            room.Gfx.InterpolateCallback(room.RoomView.GameOver, 0.1f, "set_visible", true);
        }
    }
}
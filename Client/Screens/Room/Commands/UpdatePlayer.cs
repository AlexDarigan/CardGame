using WAT;

namespace CardGame.Client.Commands
{
    public class UpdatePlayer: Command
    {
        private States States { get; }

        public UpdatePlayer(States states)
        {
            Who = Who.Player;
            States = states;
        }

        protected override void Setup(Room room)
        {
            room.InputController.State = States;
            room.Text.State = States;
            room.ChessClockButton.State = States;
            Main.OnRoomUpdated(); // Required For Testing
        }
    }
}
using WAT;

namespace CardGame.Client.Commands
{
    public class UpdatePlayer: Command
    {
        private const bool IsPlayer = true;
        private States States { get; }

        public UpdatePlayer(States states)
        {
            States = states;
        }

        protected override void Setup(Room room)
        {
            Player player = (Player) room.GetPlayer(IsPlayer);
            player.State = States;
            room.RoomView.OnGameUpdated(room, States);
        }
    }
}
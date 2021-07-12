using System;

namespace CardGame.Client.Commands
{
    public class EndTurn: Command
    {
        protected override void Setup(Room room)
        {
            room.Text.AddTurn();
        }
    }
}
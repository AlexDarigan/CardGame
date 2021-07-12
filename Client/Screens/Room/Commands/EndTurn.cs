using System;

namespace CardGame.Client.Commands
{
    public class EndTurn: Command
    {
        public EndTurn()
        {
            
        }

        protected override void Setup(Room room)
        {
            room.GUI.TurnCount.Text = (int.Parse(room.GUI.TurnCount.Text) + 1).ToString();
        }
    }
}
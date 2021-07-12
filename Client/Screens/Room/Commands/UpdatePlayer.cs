﻿using WAT;

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
            Participant player = room.GetPlayer(IsPlayer);
            room.InputController.State = States;
            room.GUI.State.Text = States.ToString();
            room.ChessClockButton.State = States;
            Main.OnRoomUpdated(); // Required For Testing
        }
    }
}
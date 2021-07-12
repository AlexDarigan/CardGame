﻿using WAT;

namespace CardGame.Client.Commands
{
    public class UpdatePlayer: Command
    {
        private States States { get; }

        public UpdatePlayer(States states)
        {
            IsPlayer = true;
            States = states;
        }

        protected override void Setup(Room room)
        {
            Participant player = room.GetPlayer(IsPlayer);
            room.InputController.State = States;
            room.Text.State = States;
            room.ChessClockButton.State = States;
            Main.OnRoomUpdated(); // Required For Testing
        }
    }
}
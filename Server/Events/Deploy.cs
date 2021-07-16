﻿namespace CardGame.Server.Events
{
    public class Deploy : Event
    {
        private Card Card { get; }
        private Player Controller { get; }

        public Deploy(Player controller, Card card)
        {
            Controller = controller;
            Card = card; 
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerDeploy, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalDeploy, Card.Id, Card.SetCodes);
        }
    }
}
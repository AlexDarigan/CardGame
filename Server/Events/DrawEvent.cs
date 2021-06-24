﻿using System;
using Godot;

namespace CardGame.Server
{
    public class DrawEvent: Event
    {
        private readonly Player Controller;
        private readonly Card Card;
        
        public DrawEvent(Card card)
        {
            Command = CommandId.Draw;
            Controller = card.Controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, isClient, Card.Id);
            queue(Controller.Opponent.Id, Command, !isClient, -1);
        }
    }
}
﻿using System;
using System.Threading.Tasks;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Commands
{
    public abstract class Command
    {
        // Store common operations down here so we can be more declarative in subclasses
        protected int CardId { get; set; } = -1;
        protected SetCodes SetCode { get; set; } = SetCodes.NullCard;
        protected Who Who { get; set; }
        protected Card Card { get; set; }
        protected Participant Player { get; private set; }

        public async Task Execute(Room room)
        {
            room.Effects.RemoveAll();

            Player = Who switch
            {
                Who.Player => room.Player,
                Who.Rival => room.Rival,
                _ => null
            };
            
            // Possible NullReference Exception?
            Card = room.Cards[CardId, SetCode];

            Setup(room);
            room.Effects.Start();
            await room.Effects.Executed();
        }

        protected abstract void Setup(Room room);
        
    }
}
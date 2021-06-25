﻿using System;
using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Deploy : Command
    {
        private readonly Participant _player;
        private readonly Card Card;

        public Deploy(Participant player, Card card)
        {
            _player = player;
            Card = card;
        }
		
        protected override void Setup(Tween gfx)
        {
            Card card = _player.IsClient ? Card : _player.Hand.Last();
             _player.Hand.Remove(Card);
            Location destination = _player.Units.Add(Card);
            const float duration = .35f;
            gfx.InterpolateProperty(card, nameof(Card.Translation), card.Translation, destination.Translation,  duration);
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees),  Card.RotationDegrees, destination.RotationDegrees, duration);
        }
    }
}
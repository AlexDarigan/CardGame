﻿using System;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetHealth: Command
    {

        private Participant Player { get; }
        private int NewHealth { get; }

        public SetHealth(Participant player, int newHealth)
        {
            Player = player;
            NewHealth = newHealth;
        }
        
        protected override void Setup(Tween gfx)
        {
            gfx.InterpolateCallback(this, 0.2f, nameof(SetPlayerHealth));
        }

        private void SetPlayerHealth()
        {
            Player.Health = NewHealth;
            Console.WriteLine($"Player's Health is {Player.Health}");
        }
    }
}
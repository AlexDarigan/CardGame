﻿using Godot;

namespace CardGame.Client.Commands
{
    public class Battle: Command
    {
        private int AttackerId { get; }
        private int DefenderId { get; }

        public Battle(int attackerId, int defenderId)
        {
            AttackerId = attackerId;
            DefenderId = defenderId;
        }

        protected override void Setup(Room room)
        {
            Card attacker = room.Cards[AttackerId];
            Card defender = room.Cards[DefenderId];
            attacker.LookAt(defender.Translation);
            Vector3 sourceRotation = attacker.RotationDegrees;
            attacker.RotationDegrees = new Vector3(0, 0, 0);
            
            room.Effects.InterpolateCallback(attacker, 0.2f, nameof(Card.LookAt), defender.Translation);
            
            room.Effects.InterpolateProperty(attacker, nameof(Card.Translation), attacker.Translation, defender.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            room.Effects.InterpolateProperty(attacker, nameof(Card.Translation), defender.Translation, attacker.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);

            room.Effects.InterpolateProperty(attacker, nameof(attacker.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
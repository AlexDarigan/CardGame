using Godot;

namespace CardGame.Client.Commands
{
    public class Battle: Command
    {
        private int AttackerId { get; }
        private int DefenderId { get; }

        public Battle(int attacker, int defender)
        {
            AttackerId = attacker;
            DefenderId = defender;
        }

        protected override void Setup(CommandQueue gfx)
        {
            Card attacker = gfx.GetCard(AttackerId);
            Card defender = gfx.GetCard(DefenderId);
            attacker.LookAt(defender.Translation);
            Vector3 sourcePosition = attacker.Translation;
            Vector3 sourceRotation = attacker.RotationDegrees;
            attacker.RotationDegrees = new Vector3(0, 0, 0);
            
            gfx.InterpolateCallback(attacker, 0.2f, nameof(Card.LookAt), defender.Translation);
            
            gfx.InterpolateProperty(attacker, nameof(Card.Translation), attacker.Translation, defender.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            gfx.InterpolateProperty(attacker, nameof(Card.Translation), attacker.Translation, sourcePosition,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);

            gfx.InterpolateProperty(attacker, nameof(attacker.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
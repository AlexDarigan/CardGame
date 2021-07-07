using Godot;

namespace CardGame.Client.Commands
{
    public class Battle: Command
    {
        private Card Attacker { get; }
        private Card Defender { get; }

        public Battle(Card attacker, Card defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        protected override void Setup(Tween gfx)
        {
            Attacker.LookAt(Defender.Translation);
            Vector3 sourcePosition = Attacker.Translation;
            Vector3 sourceRotation = Attacker.RotationDegrees;
            Attacker.RotationDegrees = new Vector3(0, 0, 0);
            
            gfx.InterpolateCallback(Attacker, 0.2f, nameof(Card.LookAt), Defender.Translation);
            
            gfx.InterpolateProperty(Attacker, nameof(Card.Translation), Attacker.Translation, Defender.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            gfx.InterpolateProperty(Attacker, nameof(Card.Translation), Attacker.Translation, sourcePosition,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);

            gfx.InterpolateProperty(Attacker, nameof(Attacker.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
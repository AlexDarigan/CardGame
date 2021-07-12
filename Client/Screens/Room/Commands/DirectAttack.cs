using CardGame.Client.Views;
using Godot;

namespace CardGame.Client.Commands
{
    public class DirectAttack: Command
    {
        private int AttackerId { get; }
        
        public DirectAttack(int attackerId) { AttackerId = attackerId; }

        
        protected override void Setup(Room room)
        {
            Card card = room.GetCard(AttackerId);
            Avatar avatar = card.Controller.IsClient ? room.Rival.Avatar : room.Player.Avatar;
            
            card.LookAt(avatar.Translation);
            Vector3 sourceRotation = card.RotationDegrees;
            card.RotationDegrees = new Vector3(0, 0, 0);
            
            room.Effects.InterpolateCallback(card, 0.2f, nameof(Card.LookAt), avatar.Translation);
            room.Effects.InterpolateProperty(card, nameof(Card.Translation), card.Translation, avatar.Translation, 0.2f, 
                Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            room.Effects.InterpolateProperty(card, nameof(Card.Translation), avatar.Translation, card.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);
            
            room.Effects.InterpolateProperty(card, nameof(card.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
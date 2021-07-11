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
            Heart heart = card.Controller is Player ? room.RoomView.RivalHeart : room.RoomView.PlayerHeart;
            
            card.LookAt(heart.Translation);
            Vector3 sourceRotation = card.RotationDegrees;
            card.RotationDegrees = new Vector3(0, 0, 0);
            
            room.Gfx.InterpolateCallback(card, 0.2f, nameof(Card.LookAt), heart.Translation);
            room.Gfx.InterpolateProperty(card, nameof(Card.Translation), card.Translation, heart.Translation, 0.2f, 
                Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            room.Gfx.InterpolateProperty(card, nameof(Card.Translation), heart.Translation, card.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);
            
            room.Gfx.InterpolateProperty(card, nameof(card.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
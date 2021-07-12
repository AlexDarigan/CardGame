using CardGame.Client.Views;
using Godot;

namespace CardGame.Client.Commands
{
    public class DirectAttack: Command
    {
        public DirectAttack(int cardId) { CardId = cardId; }

        
        protected override void Setup(Room room)
        {
            Avatar avatar = Card.Controller.IsClient ? room.Player.Avatar : room.Rival.Avatar;
            
            Card.LookAt(avatar.Translation);
            Vector3 sourceRotation = Card.RotationDegrees;
            Card.RotationDegrees = new Vector3(0, 0, 0);
            
            room.Effects.InterpolateCallback(Card, 0.2f, nameof(Card.LookAt), avatar.Translation);
            room.Effects.InterpolateProperty(Card, nameof(Card.Translation), Card.Translation, avatar.Translation, 0.2f, 
                Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            room.Effects.InterpolateProperty(Card, nameof(Card.Translation), avatar.Translation, Card.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);
            
            room.Effects.InterpolateProperty(Card, nameof(Card.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
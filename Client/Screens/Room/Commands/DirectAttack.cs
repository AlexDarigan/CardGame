using Godot;

namespace CardGame.Client.Commands
{
    public class DirectAttack: Command
    {
        private int CardId { get; }

        public DirectAttack(int cardId)
        {
            CardId = cardId;
        }

        
        protected override void Setup(Room room)
        {
            Card card = room.Cards[CardId];
            Avatar avatar = card.Controller.IsClient ? room.Rival.Avatar : room.Player.Avatar;
            
            card.LookAt(avatar.Translation);
            Vector3 sourceRotation = card.RotationDegrees;
            card.RotationDegrees = new Vector3(0, 0, 0);
            
            room.Effects.InterpolateCallback(card, 0.2f, nameof(card.LookAt), avatar.Translation);
            room.Effects.InterpolateProperty(card, nameof(card.Translation), card.Translation, avatar.Translation, 0.2f, 
                Tween.TransitionType.Linear, Tween.EaseType.InOut, 0.3f);
            
            room.Effects.InterpolateProperty(card, nameof(card.Translation), avatar.Translation, card.Translation,
                0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut, .5f);
            
            room.Effects.InterpolateProperty(card, nameof(card.RotationDegrees), sourceRotation, new Vector3(0, 0, 0),
                0.9f);
        }
    }
}
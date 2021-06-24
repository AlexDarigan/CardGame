using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Draw: Command
    {
        private readonly Player _player;
        private readonly Card Card;

        public Draw(Player player, Card card)
        {
            _player = player;
            Card = card;
        }
		
        public override void Execute(Tween gfx)
        {
            // Our rival doesn't have a real card, so we need to make a local check lest we end up moving the same card around 
            Card card = _player.isClient ? Card : _player.Deck.Last();
            Spatial source = _player.Zones.Deck.GetNode<Spatial>($"{_player.Deck.Count - 1}");
            Spatial destination = _player.Zones.Hand.GetNode<Spatial>($"{_player.Hand.Count}");
			
            _player.Deck.Remove(card);
            _player.Hand.Add(card);
            source.Visible = false; 
            
            // The tween can look weird if we don't prepare beforehand
            card.Translation = source.Translation;
            card.RotationDegrees = source.RotationDegrees;
            const float duration = .35f;
            gfx.InterpolateProperty(card, "translation", card.Translation, destination.Translation, duration);
            gfx.InterpolateProperty(card, "rotation_degrees", card.RotationDegrees, destination.RotationDegrees, duration);

        }
    }
}
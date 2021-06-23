using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Deploy : Command
    {
        private readonly Player _player;
        private readonly Card Card;

        public Deploy(Player player, Card card)
        {
            _player = player;
            Card = card;
        }
		
        public override SignalAwaiter Execute(Tween gfx)
        {
            Card card = _player.isClient ? Card : _player.Hand.Last();
            gfx.RemoveAll();

            Spatial destination = _player.Zones.Units.GetNode<Spatial>($"{_player.Units.Count}");

            _player.Hand.Remove(Card);
            _player.Units.Add(Card);
			
            const float duration = 0.25f;
            gfx.InterpolateProperty(card, "translation", card.Translation, destination.Translation,  duration);
            gfx.InterpolateProperty(card, "rotation_degrees", card.RotationDegrees, destination.RotationDegrees, duration);
				
            gfx.Start();
            return ToSignal(gfx, "tween_all_completed");
        }
    }
}
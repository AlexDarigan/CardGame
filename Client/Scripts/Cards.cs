using Godot;
using Godot.Collections;

namespace CardGame.Client
{
    public class Cards: Spatial
    {
        private readonly Dictionary<int, Card> _cards = new();
        public Player Player; // We'll do this for now
        public Cards() => Name = "Cards";
        public Card this[int index] => _cards[index];
        public Card GetCard(int id, SetCodes setCodes)
        {
            if (_cards.ContainsKey(id)) { return _cards[id]; }
            Spatial view = Views.Scenes.Card();
            Card card = new(Library.Cards[setCodes], view, id) {Translation = new Vector3(0, -3, 0)};
            card.CardPressed += Player.OnCardPressed;
            AddChild(view);
            _cards[id] = card;
            return card;
        }
    }
}
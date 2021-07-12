using Godot;
using Godot.Collections;

namespace CardGame.Client
{
    public class Cards: Spatial
    {
        private static readonly PackedScene CardScene = GD.Load<PackedScene>("res://Client/Card/Card.tscn");
       
        private readonly Dictionary<int, Card> _cards = new();
        public Player Player; // We'll do this for now
        public Cards() => Name = "Cards";
        public Card this[int index] => _cards[index];
        public Card GetCard(int id, SetCodes setCodes)
        {
            if (_cards.ContainsKey(id)) { return _cards[id]; }
            Card card = (Card) CardScene.Instance();
            card.Translation = new Vector3(0, -3, 0);
            card.CardPressed += Player.OnCardPressed;
            AddChild(card);
            Library.Cards[setCodes].WriteTo(card);
            _cards[id] = card;
            card.Id.Set(id);
            return card;
        }
    }
}
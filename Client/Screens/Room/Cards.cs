using Godot;
using Godot.Collections;

namespace CardGame.Client
{
    public class Cards: Spatial
    {
        private static readonly PackedScene CardScene = GD.Load<PackedScene>("res://Client/Card/Card.tscn");
       
        private readonly Dictionary<int, Card> _cards = new();
        public InputController InputController;
        public Cards() => Name = "Cards";
        public Card this[int id] => _cards[id];

        public Card this[int id, SetCodes setCode = SetCodes.NullCard]
        {
            get
            {
                if (!_cards.ContainsKey(id)) { Add(id, setCode); }
                return _cards[id];
            }
        }

        public void Add(int id, SetCodes setCodes)
        {
            Card card = (Card) CardScene.Instance();
            card.Translation = new Vector3(0, -3, 0);
            card.CardPressed += InputController.OnCardPressed;
            AddChild(card);
            Library.Cards[setCodes].WriteTo(card);
            _cards[id] = card;
            card.Id = id;
        }
        
    }
}
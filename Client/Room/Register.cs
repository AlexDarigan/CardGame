using Godot;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CardGame.Client
{
    
    [UsedImplicitly]
    public class Register : Spatial
    {
        private readonly PackedScene CardScene = (PackedScene) GD.Load("res://Client/Card/Card.tscn");
        private readonly Dictionary<int, Card> _register = new();
        public Card this[int id] => _register[id];
        
        public void Add(int id, SetCodes setCodes)
        {
            CardInfo info = Library.Cards[setCodes];
            Card card = (Card) CardScene.Instance();
            AddChild(card);
            card.Id = id;
            card.Title = info.Title;
            card.Power = info.Power;
            card.CardType = info.CardType;
            card.Text = info.Text;
            card.Art = (Texture) GD.Load($"res://Client/Assets/CardArt/{info.Art}.png");
            _register[id] = card;
            card.Translation = new Vector3(0, -3, 0);
        }
    }
}


﻿using Godot;
using Godot.Collections;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class Cards
    {
        public Participant Player; // We'll do this for now
        private static readonly PackedScene CardView = GD.Load<PackedScene>("res://Client/Card/CardView.tscn");
        private readonly Dictionary<int, Card> _cards = new();
        private readonly Spatial _view;

        public Cards(Spatial view) => _view = view;
        
        public Card GetCard(int id, SetCodes setCodes)
        {
            if (_cards.ContainsKey(id)) { return _cards[id]; }
            Spatial view = (Spatial) CardView.Instance();
            Card card = new Card(Library.Cards[setCodes], view, id) {Translation = new Vector3(0, -3, 0)};
            card.CardPressed += Player.OnCardPressed;
            _view.AddChild(view);
            _cards[id] = card;
            return card;
        }
        
        public Card this[int index] => _cards[index];
    }
}
using System;
using Godot;
using JetBrains.Annotations;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGame.Client
{
    
    [UsedImplicitly]
    public class Register : Spatial
    {
        private readonly PackedScene CardScene = (PackedScene) GD.Load("res://Client/Card/Card.tscn");
        private readonly System.Collections.Generic.Dictionary<int, Card> _register = new();
        public Card this[int id] => _register[id];
        
        public void Add(int id, SetCodes setCodes)
        {
            CardInfo info = Library.Cards[setCodes];
            Card card = (Card) CardScene.Instance();
            AddChild(card);
            card.Name = $"{id}_{info.Title}";
            card.Id = id;
            card.Title = info.Title;
            card.Power = info.Power;
            card.CardType = info.CardType;
            card.Text = info.Text;
            card.Art = (Texture) GD.Load($"res://Client/Assets/CardArt/{info.Art}.png");
            _register[id] = card;
            card.Translation = new Vector3(0, -3, 0);
            card.GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEnterCard), new Array{ card });
            card.GetNode<Area>("Area").Connect("mouse_exited", this, nameof(OnMouseExitCard), new Array{ card });
        }

        // Store here temp
        private Card CurrentCard;
        public void OnMouseEnterCard(Card card)
        {
            CurrentCard = card;
            Console.WriteLine($"Mouse entered {card.Id}: {card.Name}");
        }

        public void OnMouseExitCard(Card card)
        {
            CurrentCard = null;
            Console.WriteLine($"Mouse exited {card.Id}: {card.Name}");
        }

        public override void _Process(float delta)
        {
            if (Input.IsMouseButtonPressed((int)ButtonList.Left) && CurrentCard is not null)
            {
                GetParent<Room>().Deploy(CurrentCard);
            }
        }
    }
}


using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;

namespace CardGame.Client
{
    public static class Library
    {
        private static readonly PackedScene _cardScene = (PackedScene) GD.Load("res://Client/Card/Card.tscn");
        private const string JsonCardsFilePath = @"Client/Library/Library.json";
        public static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(System.IO.File.ReadAllText(JsonCardsFilePath));
        
        public static Card GetCard(Node cards, SetCodes setCode, int id)
        {
            Card card = (Card) _cardScene.Instance();
            CardInfo info = Cards[setCode];
            cards.AddChild(card);
            card.Name = $"{id}_{info.Title}";
            card.Id = id;
            card.Title = info.Title;
            card.Power = info.Power;
            card.CardType = info.CardType;
            card.Text = info.Text;
            card.Art = (Texture) GD.Load($"res://Client/Assets/CardArt/{info.Art}.png");
            card.Translation = new Vector3(0, -3, 0);
            return card;
        }

    }
    
    
    public readonly struct CardInfo
    {
        public readonly CardType CardType;
        public readonly string Title;
        public readonly string Art;
        public readonly string Text;
        public readonly int Power;

        [JsonConstructor]
        public CardInfo(CardType cardType, string title, string art, string text, int power)
        {
            CardType = cardType;
            Title = title;
            Art = art;
            Text = text;
            Power = power;
        }
    }
}
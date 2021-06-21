using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;
using File = System.IO.File;

namespace CardGame.Client
{
    public static class Library
    {
        private static readonly PackedScene CardScene = (PackedScene) GD.Load("res://Client/Cards/Card.tscn");
        private const string JsonCardsFilePath = @"Client/Library/Library.json";

        private static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(File.ReadAllText(JsonCardsFilePath));

        public static Card Create(int id, SetCodes setCode)
        {
            Card card = (Card) CardScene.Instance();
            CardInfo info = Cards[setCode];
            card.Id = id;
            card.Title = info.Title;
            card.Power = info.Power;
            card.CardType = info.CardType;
            card.Text = info.Text;
            card.Art = (Texture) GD.Load($"res://Client/Assets/CardArt/{info.Art}.png");
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
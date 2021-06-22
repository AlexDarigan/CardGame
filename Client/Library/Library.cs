using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;
using File = System.IO.File;

namespace CardGame.Client
{
    public static class Library
    {
        private const string JsonCardsFilePath = @"Client/Library/Library.json";

        public static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(File.ReadAllText(JsonCardsFilePath));
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
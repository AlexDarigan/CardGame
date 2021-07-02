using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;
using File = System.IO.File;

namespace CardGame.Client
{
    public static class Library
    {
        private const string JsonCardsFilePath = @"Client/Assets/Library.json";

        public static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(File.ReadAllText(JsonCardsFilePath));
    }

    public readonly struct CardInfo
    {
        private readonly CardType _cardType;
        private readonly string _title;
        private readonly Texture _art;
        private readonly string _text;
        private readonly int _power;

        [JsonConstructor]
        public CardInfo(CardType cardType, string title, string art, string text, int power)
        {
            _cardType = cardType;
            _title = title;
            _art = (Texture) GD.Load($"res://Client/Assets/CardArt/{art}.png");
            _text = text;
            _power = power;
        }

        public void Deconstruct(out CardType cardType, out string title, out Texture art, out string text,
            out int power)
        {
            cardType = _cardType;
            title = _title;
            art = _art;
            text = _text;
            power = _power;
        }
    }
}
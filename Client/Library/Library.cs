using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;

namespace CardGame.Client
{
    public static class Library
    {
        private static readonly PackedScene _cardScene = (PackedScene) GD.Load("res://Client/Card/CardView.tscn");
        private const string JsonCardsFilePath = @"Client/Library/Library.json";
        private static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(System.IO.File.ReadAllText(JsonCardsFilePath));
        
        public static Card GetCard(Node cards, SetCodes setCode, int id)
        {
            CardView cardView = (CardView) _cardScene.Instance(); //.GetCard();
            cards.AddChild(cardView);
            return new Card(Cards[setCode], cardView, id) {Translation = new Vector3(0, -3, 0)};
        }
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

        public (CardType, string, Texture, string, int) GetData() => (_cardType, _title, _art, _text, _power);

    }
}
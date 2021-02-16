using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace CardGame.Server
{
    public static class Library
    {
        private const string JsonCardsFilePath = @"Server/Library.json";
        private static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(File.ReadAllText(JsonCardsFilePath));

        public static Card Create(Player owner, CardRegister cardRegister, SetCodes setCode)
        {
            Card card = new Card(cardRegister.Count, owner);
            CardInfo cardInfo = Cards[setCode];
            card.Title = cardInfo.Title;
            card.SetCodes = cardInfo.SetCode;
            card.CardType = cardInfo.CardType;
            card.Faction = cardInfo.Faction;
            card.Power = cardInfo.Power;
            cardRegister.Add(card);
            return card;
        }
        
        private readonly struct CardInfo
        {
            public readonly SetCodes SetCode;
            public readonly CardType CardType;
            public readonly string Title;
            public readonly Faction Faction;
            public readonly int Power;

            [JsonConstructor]
            public CardInfo(SetCodes setCode, CardType cardType, Faction faction, string title, int power)
            {
                SetCode = setCode;
                CardType = cardType;
                Faction = faction;
                Title = title;
                Power = power;
            }
        }
    }
}
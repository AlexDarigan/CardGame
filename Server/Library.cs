using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;
using Newtonsoft.Json;
using File = System.IO.File;

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
            foreach (SkillInfo skillInfo in cardInfo.Skills)
            {
                card.Skills.Add(new Skill(card, skillInfo.Triggers, skillInfo.Instructions, skillInfo.Arguments, skillInfo.Description));
            }
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
            public readonly IEnumerable<SkillInfo> Skills;

            [JsonConstructor]
            public CardInfo(SetCodes setCode, CardType cardType, Faction faction, string title, int power, IEnumerable<SkillInfo> skills)
            {
                SetCode = setCode;
                CardType = cardType;
                Faction = faction;
                Title = title;
                Power = power;
                Skills = skills;
            }
        }

        private readonly struct SkillInfo
        {
            // The Description Attribute is more of the sake of debugging rather than any practical application in game
            public readonly IEnumerable<Triggers> Triggers;
            public readonly IEnumerable<Instructions> Instructions;
            public readonly Stack<object> Arguments;
            public readonly string Description;
            
            [JsonConstructor]
            public SkillInfo(IEnumerable<Triggers> triggers, IEnumerable<Instructions> instructions, Stack<object> arguments, string description)
            {
                Triggers = triggers;
                Instructions = instructions;
                Description = description;
                
                // Stack Reverses the Order, so we're calling a second constructor to reverse it back
                Arguments = new Stack<object>(arguments);
               
            }
            
        }
    }
}
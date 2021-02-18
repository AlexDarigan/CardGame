using System;
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
            
            // Staying with one skill per card for the time being
            SkillInfo skillInfo = cardInfo.Skill;
            card.Skill = new Skill(card, skillInfo.Triggers, skillInfo.Instructions, skillInfo.Description);
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
            public readonly SkillInfo Skill;

            [JsonConstructor]
            public CardInfo(SetCodes setCode, CardType cardType, Faction faction, string title, int power, SkillInfo skill)
            {
                SetCode = setCode;
                CardType = cardType;
                Faction = faction;
                Title = title;
                Power = power;
                Skill = skill;
            }
        }

        private readonly struct SkillInfo
        {
            // The Description Attribute is more of the sake of debugging rather than any practical application in game
            public readonly IEnumerable<Triggers> Triggers;
            public readonly IEnumerable<int> Instructions;
            public readonly string Description;
            
            [JsonConstructor]
            public SkillInfo(IEnumerable<Triggers> triggers, IEnumerable<string> instructions, string description)
            {
                Triggers = triggers;
                // We store all arguments as strings but they can exist in one of a number of enums or as a literal
                //Instructions = instructions;
                List<int> insts = new List<int>();
                
                // All Instructions are stored as Integers
                // We check the string against each enum we have..
                //      ..If we have a match, then we add the related int value..
                //      ....else we just add it as a literal int
                
                // This is probably fairly expensive but we do this once at the load-time of the server so unlikely
                // ..to be any real performance issue.
                foreach (string command in instructions)
                {
                    
                    if (Enum.TryParse(command, out Instructions instruction))
                    {
                        insts.Add((int) instruction);
                    }
                    else if (Enum.TryParse(command, out CardType cardType))
                    {
                        insts.Add((int) cardType);
                    }
                    else 
                    {
                        // Is Literal Integer
                        insts.Add(command.ToInt());
                    }
                }

                Instructions = insts;
                Description = description;
                               
            }
            
        }
    }
}
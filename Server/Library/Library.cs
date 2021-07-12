using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using File = System.IO.File;

namespace CardGame.Server
{
    public static class Library
    {
        private const string JsonCardsFilePath = @"Server/Library/Library.json";
        public static readonly ReadOnlyDictionary<SetCodes, CardInfo> Cards =
            JsonConvert.DeserializeObject<ReadOnlyDictionary<SetCodes,
                CardInfo>>(File.ReadAllText(JsonCardsFilePath));
    }
    
    public readonly struct CardInfo
    {
        public CardTypes CardTypes { get; }
        public string Title { get; }
        public Factions Factions { get; }
        public int Power { get; }
        public SkillInfo Skill { get; }

        [JsonConstructor]
        public CardInfo(CardTypes cardTypes, Factions factions, string title, int power, SkillInfo skill)
        {
            CardTypes = cardTypes;
            Factions = factions;
            Title = title;
            Power = power;
            Skill = skill;
        }
    }

    public readonly struct SkillInfo
    {
        public readonly IEnumerable<Triggers> Triggers;
        public readonly IEnumerable<int> OpCodes;
        public readonly string Description; // Debugging Purposes

        [JsonConstructor]
        public SkillInfo(IEnumerable<Triggers> triggers, IEnumerable<string> opCodes, string description)
        {
            Triggers = triggers;
            Description = description;
            OpCodes = GetOpCodes(opCodes);
            
            static IEnumerable<int> GetOpCodes(IEnumerable<string> ops)
            {
                foreach (string code in ops)
                {
                    if (Enum.IsDefined(typeof(OpCodes), code)) { yield return ParseEnum<OpCodes>(code); }
                    else if (Enum.IsDefined(typeof(Factions), code)) { yield return ParseEnum<Factions>(code); }
                    else if (Enum.IsDefined(typeof(CardTypes), code)) { yield return ParseEnum<CardTypes>(code); }
                    else { yield return Convert.ToInt32(code, CultureInfo.InvariantCulture); }
                }
            }
        }
        
        private static int ParseEnum<T>(string value)
        {
            return (int) Enum.Parse(typeof(T), value, true);
        }
    }
}
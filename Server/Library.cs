using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Godot;
using Newtonsoft.Json;
using File = System.IO.File;

namespace CardGame.Server
{
    public static class Library
    {
        private const string JsonCardsFilePath = @"Server/Library.json";
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
        // The Description Attribute is more of the sake of debugging rather than any practical application in game
        public readonly IEnumerable<Triggers> Triggers;
        public readonly IReadOnlyList<int> Instructions;
        public readonly string Description;

        [JsonConstructor]
        public SkillInfo(IEnumerable<Triggers> triggers, IEnumerable<string> instructions, string description)
        {
            Triggers = triggers;
            // We store all arguments as strings but they can exist in one of a number of enums or as a literal
            //Instructions = instructions;
            List<int> insts = new();

            // All Instructions are stored as Integers
            // We check the string against each enum we have..
            //      ..If we have a match, then we add the related int value..
            //      ....else we just add it as a literal int

            // This is probably fairly expensive but we do this once at the load-time of the server so unlikely
            // ..to be any real performance issue.
            
            // Figure out how to add CultureInfoInvariant
            foreach (string command in instructions)
                if (Enum.TryParse(command, out OpCodes instruction))
                    insts.Add((int) instruction);
                else if (Enum.TryParse(command, out CardTypes cardType))
                    insts.Add((int) cardType);
                else
                    // Is Literal Integer
                    insts.Add(command.ToInt());

            Instructions = insts;
            Description = description;
        }
    }
}
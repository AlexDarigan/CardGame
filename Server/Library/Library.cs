using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
			OpCodes = GetOpCodes();
			
			IEnumerable<int> GetOpCodes()
			{
				List<int> codes = new();
				foreach (string code in opCodes)
				{
					if (int.TryParse(code, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)) { codes.Add(value); }
					else if (Enum.IsDefined(typeof(OpCodes), code)) { codes.Add(ParseEnum<OpCodes>(code)); }
					else if (Enum.IsDefined(typeof(Factions), code)) { codes.Add(ParseEnum<Factions>(code)); }
					else if (Enum.IsDefined(typeof(CardTypes), code)) { codes.Add(ParseEnum<CardTypes>(code)); }
					else { throw new Exception("Could not locate a matching Op Code");}
				}
				return codes.AsEnumerable();
			}
		}
		
		private static int ParseEnum<T>(string value)
		{
			return (int) Enum.Parse(typeof(T), value, true);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace CardGame.Server
{
    public static class SkillOperations
    {
        private static readonly IReadOnlyDictionary<OpCodes, Action<SkillState>> Operations;
        private const int Player = 1;
        private const int Opponent = 0;
        
        static SkillOperations()
        {
            Dictionary<OpCodes, Action<SkillState>> operations = new();
            foreach (OpCodes instruction in Enum.GetValues(typeof(OpCodes)))
            {
                operations[instruction] = (Action<SkillState>) Delegate.CreateDelegate(typeof(Action<SkillState>), null,
                    typeof(SkillOperations).GetMethod(instruction.ToString(), BindingFlags.Static | BindingFlags.NonPublic)!);
            }
            Operations = operations;
        }

        public static Action<SkillState> GetOperation(OpCodes opCode) => Operations[opCode];
        
        // Internal Non-Operation Helper Methods
        private static Player GetPlayer(SkillState skill) { return skill.PopBack() == Player ? skill.Controller: skill.Opponent; }
        private static void GetCards(SkillState skill, Zone zone) { skill.Cards.AddRange(zone); }
        
        // Getters
        private static void Literal(SkillState skill) { skill.Push(skill.Next()); }
        private static void GetOwningCard(SkillState skill) { }
        private static void GetController(SkillState skill) { skill.Push(Player); }
        private static void GetOpponent(SkillState skill) { skill.Push(Opponent); }
        private static void GetDeck(SkillState skill) { GetCards(skill, GetPlayer(skill).Deck); }
        private static void GetGraveyard(SkillState skill) { GetCards(skill, GetPlayer(skill).Graveyard); }
        private static void GetHand(SkillState skill) { GetCards(skill, GetPlayer(skill).Hand); }
        private static void GetUnits(SkillState skill) { GetCards(skill, GetPlayer(skill).Units); }
        private static void GetSupport(SkillState skill) { GetCards(skill, GetPlayer(skill).Supports); }
        private static void Count(SkillState skill) { skill.Push(skill.Cards.Count); }
        
        // Control Flow
        private static void If(SkillState skill) { }
        private static void GoToEnd(SkillState skill) { }
        
        // Boolean
        private static void IsLessThan(SkillState skill) { }
        private static void IsGreaterThan(SkillState skill) { }
        private static void IsEqual(SkillState skill) { }
        private static void IsNotEqual(SkillState skill) { }
        private static void And(SkillState skill) { }
        private static void Or(SkillState skill) { }
        
        // Actions
        private static void SetFaction(SkillState skill) { }
        private static void SetPower(SkillState skill) { }
        private static void Destroy(SkillState skill) { }
        private static void DealDamage(SkillState skill) { }
        private static void Draw(SkillState skill)
        {
            Player player = GetPlayer(skill);
            for (int i = 0; i < skill.PopBack(); i++) { player.Draw(); }
        }
    }
}

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

        private static void CompareAndPushResult(SkillState skill, Func<int, int, bool> comparator)
        {
            int b = skill.PopBack();
            int a = skill.PopBack();
            skill.Push(Convert.ToInt32(comparator(a, b)));
        }
        
        private static void Calculate(SkillState skill, Func<int, int, int> calculator)
        {
            int b = skill.PopBack();
            int a = skill.PopBack();
            int result = calculator(a, b);
            skill.Push(result);
        }
        
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
        private static void CountCards(SkillState skill) { skill.Push(skill.Cards.Count); }
        
        // Control Flow
        private static void If(SkillState skill)
        {
            int jump = skill.PopBack();
            int condition = skill.PopBack();
            // We jump on the else, not the positive
            if (condition == 0) { skill.Jump(jump); }
        }
        
        // Boolean
        private static void IsLessThan(SkillState skill) { CompareAndPushResult(skill, (a, b) => a < b); }
        private static void IsGreaterThan(SkillState skill) { CompareAndPushResult(skill, (a, b) => a > b ); }
        private static void IsEqual(SkillState skill) { CompareAndPushResult(skill, (a, b) => a == b ); }
        private static void IsNotEqual(SkillState skill) { CompareAndPushResult(skill, (a, b) => a != b ); }
        private static void And(SkillState skill) { skill.Push(Convert.ToInt32(skill.PopBack() == 1 && skill.PopBack() == 1)); }
        private static void Or(SkillState skill) { skill.Push(Convert.ToInt32(skill.PopBack() == 1 || skill.PopBack() == 1)); }
        
        // Calculation
        private static void Add(SkillState skill) { Calculate(skill, (a, b) => a + b); }
        private static void Subtract(SkillState skill) { Calculate(skill, (a, b) => a - b); }
        private static void Multiply(SkillState skill) { Calculate(skill, (a, b) => a * b); }
        private static void Divide(SkillState skill) { Calculate(skill, (a, b) => a / b); }
        
        // Actions
        private static void SetHealth(SkillState skill)
        {
            Player player = GetPlayer(skill);
            int newHealth = skill.PopBack();
            player.Health = newHealth;
        }
        private static void SetFaction(SkillState skill) { }
        private static void SetPower(SkillState skill) { }
        private static void Destroy(SkillState skill) { }
        private static void DealDamage(SkillState skill) { }
        private static void Draw(SkillState skill)
        {
            // If we use skill.PopBack() inlined into the loop, it seems to only ever return the value "2"..
            // ..I imagine this is because we're dealing with it inside a static method
            Player player = GetPlayer(skill);
            int count = skill.PopBack();
            for (int i = 0; i < count; i++) { player.Draw(); }
        }
    }
}

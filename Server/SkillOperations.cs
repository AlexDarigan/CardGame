using System;
using System.Collections.Generic;
using System.Reflection;

namespace CardGame.Server
{
    public static class SkillOperations
    {
        private static readonly IReadOnlyDictionary<OpCodes, Action<SkillState>> Operations;
        
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

        // Getters
        private static void Literal(SkillState skill)
        {
            skill.Push(skill.Next());
        }

        private static void GetOwningCard(SkillState skill) { }
        private static void GetController(SkillState skill)
        {
            // 0 is opponent
            // 1 is controller
            skill.Push(1);
        }
        private static void GetOpponent(SkillState skill) { }
        private static void GetDeck(SkillState skill) { }
        private static void GetGraveyard(SkillState skill) { }
        private static void GetHand(SkillState skill) { }
        private static void GetUnits(SkillState skill) { }
        private static void GetSupport(SkillState skill) { }
        private static void Count(SkillState skill) { }
        
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
            Player player = skill.PopBack() == 1 ? skill.Controller : skill.Opponent;
            for (int i = 0; i < skill.PopBack(); i++)
            {
                Console.WriteLine("Drawing Card!");
                // Seek an avenue to see where this can give spawn-source?
                player.Draw();
            }
        }
    }
}

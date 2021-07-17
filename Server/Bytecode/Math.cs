using System;

namespace CardGame.Server.Bytecode
{
    public static partial class VirtualMachine
    {
        private static void Calculate(SkillState skill, Func<int, int, int> calculator)
        {
            int b = skill.PopBack();
            int a = skill.PopBack();
            int result = calculator(a, b);
            skill.Push(result);
        }
        
        private static void Add(SkillState skill) { Calculate(skill, (a, b) => a + b); }
        private static void Subtract(SkillState skill) { Calculate(skill, (a, b) => a - b); }
        private static void Multiply(SkillState skill) { Calculate(skill, (a, b) => a * b); }
        private static void Divide(SkillState skill) { Calculate(skill, (a, b) => a / b); }
        private static void CountCards(SkillState skill) { skill.Push(skill.Cards.Count); }
    }
}
using System;

namespace CardGame.Server.Bytecode
{
    public static partial class VirtualMachine
    {
        private static void If(SkillState skill)
        {
            int jump = skill.PopBack();
            int condition = skill.PopBack();
            // We jump on the else, not the positive
            if (condition == 0) { skill.Jump(jump); }
        }
        
        private static void CompareAndPushResult(SkillState skill, Func<int, int, bool> comparator)
        {
            int b = skill.PopBack();
            int a = skill.PopBack();
            skill.Push(Convert.ToInt32(comparator(a, b)));
        }
        
        private static void IsLessThan(SkillState skill) { CompareAndPushResult(skill, (a, b) => a < b); }
        private static void IsGreaterThan(SkillState skill) { CompareAndPushResult(skill, (a, b) => a > b ); }
        private static void IsEqual(SkillState skill) { CompareAndPushResult(skill, (a, b) => a == b ); }
        private static void IsNotEqual(SkillState skill) { CompareAndPushResult(skill, (a, b) => a != b ); }
        private static void And(SkillState skill) { skill.Push(Convert.ToInt32(skill.PopBack() == 1 && skill.PopBack() == 1)); }
        private static void Or(SkillState skill) { skill.Push(Convert.ToInt32(skill.PopBack() == 1 || skill.PopBack() == 1)); }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using CardGame.Server.Events;

namespace CardGame.Server.Bytecode
{
    public static partial class VirtualMachine
    {
        private static IReadOnlyDictionary<OpCodes, Action<SkillState>> Ops { get; }
        private const int Owner = 2;
        private const int Player = 1;
        private const int Opponent = 0;
        
        static VirtualMachine()
        {
            Dictionary<OpCodes, Action<SkillState>> operations = new();
            foreach (OpCodes instruction in Enum.GetValues(typeof(OpCodes)))
            {
                operations[instruction] = (Action<SkillState>) Delegate.CreateDelegate(typeof(Action<SkillState>), null,
                    typeof(VirtualMachine).GetMethod(instruction.ToString(), BindingFlags.Static | BindingFlags.NonPublic)!);
            }
            Ops = operations;
        }

        public static Action<SkillState> GetOperation(OpCodes opCode) => Ops[opCode];
        
     
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;

// ReSharper disable InconsistentNaming

namespace WAT
{
    public partial class Test : Node
    {
        public Array get_test_methods()
        {
            return new(GetType().GetMethods().Where(m => m.IsDefined(typeof(TestAttribute))).Select(m => m.Name)
                .ToList());
        }

        public Test setup(string directory, string filepath, IEnumerable<string> methods)
        {
            _methods = GenerateTestMethods(methods);
            _case = (Object) GD.Load<GDScript>("res://addons/WAT/test/case.gd").New(directory, filepath, Title(), this);
            return this;
        }

        private static bool _is_wat_test()
        {
            return true;
        }
    }
}
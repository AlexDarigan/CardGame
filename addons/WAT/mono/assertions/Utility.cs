using Godot.Collections;

namespace WAT
{
    public class Utility : Assertion
    {
        public static Dictionary Fail(string context)
        {
            return Result(false, "N/A", "N/A", context);
        }
    }
}
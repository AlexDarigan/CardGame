using System.Collections.Generic;

namespace CardGame.Server
{
    public class Stack
    {
        private readonly List<int> _list;
        public readonly int MaxSize;

        public Stack(List<int> list)
        {
            _list = list;
            MaxSize = _list.Count;
        }

        public int Count => _list.Count;

        public int this[int i] => _list[i];

        public int Pop()
        {
            int popped = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);
            return popped;
        }

        public void Push(int i)
        {
            _list.Add(i);
        }
    }
}
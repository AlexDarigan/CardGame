using System.Collections.Generic;

namespace CardGame.Server
{
    public class Stack
    {
        private readonly List<int> _list;
        public int Count => _list.Count;

        public Stack(List<int> list) => _list = list;

        public int Pop()
        {
            int popped = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);
            return popped;
        }

        public void Push(int i) => _list.Add(i);
        public int this[int i] => _list[i];
    }
    
}
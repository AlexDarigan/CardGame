﻿using System;
using System.Collections.Generic;

namespace CardGame.Server
{
    public class SkillState
    {
        public Card OwningCard { get; }
        public Player Owner => OwningCard.Owner;
        public Player Controller => OwningCard.Controller;
        public Player Opponent => OwningCard.Controller.Opponent;
        public List<Card> Cards = new();
        private List<int> _instructions;
        private int _cursor = 0;
        private readonly int _maxSize;

        public SkillState(Card owningCard, IEnumerable<int> instructions)
        {
            OwningCard = owningCard;
            _instructions = new List<int>(instructions);
            _maxSize = _instructions.Count;
        }

        public void Execute()
        {
            Action<SkillState> operation = Operations.GetOperation((OpCodes) _instructions[_cursor]);
            operation(this);
            _cursor++;
        }

        public void Jump(int i) { _cursor = _cursor + i - 1;}
        public void Push(int i) { _instructions.Add(i); }
        public int Next() => _instructions[++_cursor];
        public int PopBack() => Pop(_instructions.Count - 1);
        private int Pop(int index)
        {
            int popped = _instructions[index];
            _instructions.RemoveAt(index);
            return popped;
        }
        
        public bool IsDone() => _cursor == _maxSize;
    }
}

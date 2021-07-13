﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
    public class SkillState
    {
        public Card OwningCard { get; }
        public Player Owner => OwningCard.Owner;
        public Player Controller => OwningCard.Controller;
        public Player Opponent => OwningCard.Controller.Opponent;
        public List<Card> Cards { get; } = new();
        private List<int> OpCodes { get; }
        private readonly int _maxSize;
        private int _cursor = 0;

        public SkillState(Card owningCard, IEnumerable<int> opCodes)
        {
            OwningCard = owningCard;
            OpCodes = opCodes.ToList();
            _maxSize = OpCodes.Count;
        }

        public void Execute()
        {
            Action<SkillState> operation = Operations.GetOperation((OpCodes) OpCodes[_cursor]);
            operation(this);
            _cursor++;
        }

        public void Jump(int i) { _cursor = _cursor + i - 1;}
        public void Push(int i) { OpCodes.Add(i); }
        public int Next() => OpCodes[++_cursor];
        public int PopBack() => Pop(OpCodes.Count - 1);
        private int Pop(int index)
        {
            int popped = OpCodes[index];
            OpCodes.RemoveAt(index);
            return popped;
        }
        
        public bool IsDone() => _cursor == _maxSize;
    }
}

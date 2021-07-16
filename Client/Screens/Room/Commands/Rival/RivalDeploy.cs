﻿using System.Linq;

namespace CardGame.Client.Commands
{
    public class RivalDeploy: Command
    {
        private int CardId { get; }
        private SetCodes SetCode { get; }
        
        public RivalDeploy(int cardId, SetCodes setCode)
        {
            CardId = cardId;
            SetCode = setCode;
        }
        
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            Card card = room.Cards[CardId, SetCode];
            
            // Replace a hidden card with our concrete one
            // For now we'll remove from the end of the hand (we'll worry about index later)
            Card fake = rival.Hand.Last();
            rival.Hand.Remove(fake);
            fake.Free();
            rival.Hand.Add(card);
            
            Move(room, card, rival.Units);
        }
    }
}
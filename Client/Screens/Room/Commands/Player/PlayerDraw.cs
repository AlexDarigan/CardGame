﻿namespace CardGame.Client.Commands
{
    public class PlayerDraw: Command
    {
        private int CardId { get; }
        
        public PlayerDraw(int cardId)
        {
            CardId = cardId;
        }
        
        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];
            
            // Remove + Add so our card's location is at the top
            player.Deck.Remove(card);
            player.Deck.Add(card);
            
            Move(card, player.Hand);
        }
    }
}
﻿using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Commands
{
    // Commands are required to be Godot Objects otherwise we can't use .Call()
    public abstract class Command : Object
    {
        
        public Command()
        {
            AddUserSignal("NullCommand");
        }

        public SignalAwaiter Execute(Tween gfx)
        {
            gfx.RemoveAll();
            Setup(gfx);
            gfx.Start();
            return ToSignal(gfx, "tween_all_completed");
        }

        // We don't really need to store the tween info here do we?
        // We could just assign it to base values and remove it afterwards?
        protected abstract void Setup(Tween gfx);

        // Helper
        protected static void SortHand(Tween gfx, Participant player)
        {
            for (int i = 0; i < player.Hand.Count; i++)
            {
                Card currentCard = player.Hand[i];
                Location location = player.Hand.Locations[i];
                if (currentCard.Translation != location.Translation)
                    gfx.InterpolateProperty(currentCard, nameof(Card.Translation), currentCard.Translation,
                        location.Translation, .1f);
            }
        }
    }
}
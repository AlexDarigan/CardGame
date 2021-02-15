using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Server
{
    public class CardData: Resource
    {
        [Export()] public string Title = "NullCard";
        [Export()] public SetCodes SetCodes = SetCodes.NullCard;
        [Export()] public CardType CardType = CardType.Null;
        [Export()] public Faction Faction = Faction.Null;
        [Export()] public int Power = 0;

        
    }
}
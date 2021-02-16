namespace CardGame
{
    public enum CardType { Null, Unit, Support }
    public enum Faction { Null, Warrior }
    public enum SetCodes
    {
        NullCard = 0,
        Alpha001,
        Alpha002
    }

    public enum Triggers
    {
        Any
    }

    public enum Instructions
    {
        // Getters
        GetOwningCard,
        GetController,
        GetOpponent,
        GetDeck,
        GetGraveyard,
        GetHand,
        GetUnits,
        GetSupport,
        
        // Setters
        SetTitle,
        SetFaction,
        SetPower,
        
        // Actions
        Draw,
        Destroy, // Whether it is one or many cards, we will destroy them in a list
    }

}
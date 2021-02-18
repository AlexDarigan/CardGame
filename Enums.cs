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
        Literal = 0,
        GetOwningCard,
        GetController,
        GetOpponent,
        GetDeck,
        GetGraveyard,
        GetHand,
        GetUnits,
        GetSupport,
        
        // Control Flow
        GoToEnd,

        // Match
        Count,
        
        // Conditionals
        IfLessThan,
        IfGreaterThan,
        
        // Setters
        SetFaction,
        SetPower,
        
        // Actions
        Draw,
        Destroy, // Whether it is one or many cards, we will destroy them in a list
        DealDamage,
        
       
    }

}
namespace CardGame
{
    public enum CardType { Null, Unit, Support }
    public enum Faction { Null, Warrior }
    public enum SetCodes
    {
        NullCard = 0,
        AlphaBioShocker = 1,
        AlphaDrawTest = 2
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

        // Math
        Count, // Cards, Not Numbers

        // Comparison
        IsLessThan,
        IsGreaterThan,
        IsEqual,
        IsNotEqual,
        
        // Boolean
        If,
        And,
        Or,
        
        // Setters
        SetFaction,
        SetPower,
        
        // Actions
        // These should double up as Commands Types
        Draw,
        Destroy, // Whether it is one or many cards, we will destroy them in a list
        DealDamage,
    }

    public enum CommandId
    {
        LoadDeck,
        Draw,
    }
    
}
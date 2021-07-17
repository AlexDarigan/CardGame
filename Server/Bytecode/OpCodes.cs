namespace CardGame.Server.Bytecode
{
    public enum OpCodes: int
    {
        // Getters
        Literal = 0,
        GetOwningCard,
        GetOwner,
        GetController,
        GetOpponent,
        
        // Global Getters
        GetDecks,
        GetGraveyards,
        GetHands,
        GetUnits,
        GetSupports,
        
        // Player Getters
        GetControllerHand,
        GetControllerDeck,
        GetControllerGraveyard,
        GetControllerUnits,
        GetControllerSupport,
        
        // Opponent Getters
        GetOpponentHand,
        GetOpponentDeck,
        GetOpponentGraveyard,
        GetOpponentUnits,
        GetOpponentSupport,

        // Math
        CountCards, // Cards, Not Numbers
        Add,
        Subtract,
        Multiply,
        Divide,

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
        SetHealth,
        SetFaction,
        SetPower,

        // Actions
        // These should double up as Commands Types
        Draw,
        Destroy, // Whether it is one or many cards, we will destroy them in a list
        DealDamage
    }
}
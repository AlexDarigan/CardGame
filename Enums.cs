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
        // Literals
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Hundred = 100,
        Thousand = 1000,
        
        // Getters
        GetController,
        GetOpponent,
        GetDeck,
        GetGraveyard,
        GetHand,
        GetUnits,
        GetSupport,
        

        // Actions
        Draw,
        Destroy, // Whether it is one or many cards, we will destroy them in a list
    }

}
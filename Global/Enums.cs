using CardGame.Client;

namespace CardGame
{
    public enum CardTypes
    {
        Null,
        Unit,
        Support
    }

    public enum Factions
    {
        Null,
        Warrior
    }

    public enum SetCodes
    {
        [CardResource("NullCard")] NullCard = 0,
        [CardResource("BioShocker")] AlphaBioShocker = 1,
        [CardResource("QuestReward")] AlphaQuestReward = 2,
        [CardResource("WeakShocker")] WeakShocker = 3
    }

    public enum Who
    {
        NoOne = 0,
        Player = 1,
        Rival = 2,
    }

    public enum Triggers
    {
        Any
    }
    

    public enum OpCodes: int
    {
        // Getters
        Literal = 0,
        GetOwningCard,
        GetOwner,
        GetController,
        GetOpponent,
        GetDeck,
        GetGraveyard,
        GetHand,
        GetUnits,
        GetSupport,

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
    

    public enum CommandId {
        UpdatePlayer,
        UpdateCards,
        //SetFaceDown, /// ??????? 
        EndTurn,
        SetHealth,
        GameOver,
        MoveCard,
        Resolve,
        
        // Common
        AttackUnit,
        AttackParticipant,
        
        // Player Commands
        PlayerLoadDeck,
        PlayerDraw,
        PlayerDeploy,
        PlayerSetFaceDown,
        PlayerActivate,
        
        // Rival Commands
        RivalLoadDeck,
        RivalDraw,
        RivalDeploy,
        RivalSetFaceDown,
        RivalActivate,
    }
    public enum States { IdleTurnPlayer, Active, Passive, Loser, Winner,
        Passing,
        Acting
    }
    
    public enum Declaration { Deploy, SetFaceDown, Activate, AttackUnit, AttackPlayer, PassPlay, EndTurn}
    public enum Zones { Deck, Discard, Hand, Units, Supports }
    public enum CardStates { None, Deploy, AttackUnit, AttackPlayer, SetFaceDown, Activate }
    public enum Illegal { NotDisqualified, Draw, Deploy, AttackUnit, AttackPlayer, SetFaceDown, PassPlay, EndTurn, Activation }

}
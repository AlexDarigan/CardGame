using System;
using CardGame.Client;
using JetBrains.Annotations;

namespace CardGame
{
    public enum CardType
    {
        Null,
        Unit,
        Support
    }

    public enum Faction
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
        LoadDeck, 
        Draw, 
        Deploy, 
        SetFaceDown, 
        EndTurn,
        DeclareAttack,
        DeclareDirectAttack,
        SetHealth,
        SentToGraveyard,
        Battle,
    }
    public enum States { IdleTurnPlayer, Active, Passive, Loser, Winner }
    public enum CardState { None, Deploy, AttackUnit, AttackPlayer, SetFaceDown, Activate }
    public enum Illegal { NotDisqualified, Draw, Deploy, AttackUnit, AttackPlayer, SetFaceDown, PassPlay, EndTurn, Activation }

}
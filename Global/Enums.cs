using System;

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
        NullCard = 0,
        AlphaBioShocker = 1,
        AlphaQuestReward = 2
    }

    public enum Triggers
    {
        Any
    }

    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class Effect : Attribute
    {
        private Action Actionx;

        public Effect(string name)
        {
            Action method = () => GetType().GetMethod(name);
        }
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

    public enum CommandId { LoadDeck, Draw, Deploy, SetFaceDown, EndTurn,
        DeclareAttack
    }
    public enum States { IdleTurnPlayer, Active, Passive, Loser, Winner }
    public enum CardState { None, Deploy, AttackUnit, AttackPlayer, Set, Activate }
    public enum Illegal { NotDisqualified, Draw, Deploy, AttackUnit, AttackPlayer, SetFaceDown, PassPlay, EndTurn, Activation }

}
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
        DealDamage
    }

    public enum CommandId { LoadDeck, Draw, Deploy, SetFaceDown }
    public enum States { IdleTurnPlayer, Active, Passive, Loser, Winner }
    public enum CardState { None, Deploy, AttackUnit, AttackPlayer, Set, Activate }
    public enum Illegal { NotDisqualified, Draw, Deploy, AttackUnit, AttackPlayer, SetFaceDown, PassPlay, EndTurn, Activation }

}
using System;

namespace CardGame.Server.Bytecode
{
    public static partial class VirtualMachine
    {
        
        private static Player GetPlayer(SkillState skill)
        {
            int id = skill.PopBack();
            return id switch
            {
                Player => skill.Controller,
                Opponent => skill.Opponent,
                Owner => skill.Owner,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // Basic Getters
        private static void Literal(SkillState skill) { skill.Push(skill.Next()); }
        private static void GetOwningCard(SkillState skill) { }
        private static void GetOwner(SkillState skill) { skill.Push(Owner); }
        private static void GetController(SkillState skill) { skill.Push(Player); }
        private static void GetOpponent(SkillState skill) { skill.Push(Opponent); }
        
        // Get Global Zones
        private static void GetDecks(SkillState skill)
        {
            skill.Cards.AddRange(skill.Controller.Deck); 
            skill.Cards.AddRange(skill.Opponent.Deck);
        }
        
        private static void GetHands(SkillState skill)
        {
            skill.Cards.AddRange(skill.Controller.Hand); 
            skill.Cards.AddRange(skill.Opponent.Hand);
        }
        
        private static void GetGraveyards(SkillState skill)
        {
            skill.Cards.AddRange(skill.Controller.Graveyard); 
            skill.Cards.AddRange(skill.Opponent.Graveyard);
        }
        
        private static void GetUnits(SkillState skill)
        {
            skill.Cards.AddRange(skill.Controller.Units); 
            skill.Cards.AddRange(skill.Opponent.Units);
        }
        
        private static void GetSupports(SkillState skill)
        {
            skill.Cards.AddRange(skill.Controller.Supports); 
            skill.Cards.AddRange(skill.Opponent.Supports);
        }

        
        // Get Controller Zones
        private static void GetControllerDeck(SkillState skill) { skill.Cards.AddRange(skill.Controller.Deck); }
        private static void GetControllerGraveyard(SkillState skill) { skill.Cards.AddRange(skill.Controller.Graveyard); }
        private static void GetControllerHand(SkillState skill) { skill.Cards.AddRange(skill.Controller.Hand); }
        private static void GetControllerUnits(SkillState skill) { skill.Cards.AddRange(skill.Controller.Units); }
        private static void GetControllerSupport(SkillState skill) { skill.Cards.AddRange(skill.Controller.Supports); }
        
        // Get Opponent Zones
        private static void GetOpponentDeck(SkillState skill) { skill.Cards.AddRange(skill.Opponent.Deck); }
        private static void GetOpponentGraveyard(SkillState skill) { skill.Cards.AddRange(skill.Opponent.Graveyard); }
        private static void GetOpponentHand(SkillState skill) { skill.Cards.AddRange(skill.Opponent.Hand); }
        private static void GetOpponentUnits(SkillState skill) { skill.Cards.AddRange(skill.Opponent.Units); }
        private static void GetOpponentSupport(SkillState skill) { skill.Cards.AddRange(skill.Opponent.Supports); }
    }
}
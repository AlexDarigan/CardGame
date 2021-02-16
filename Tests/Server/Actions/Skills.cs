﻿using CardGame.Server;

namespace CardGame.Tests.Server.Actions
{
    public class Skills : BaseTest
    {
        [Test]
        public void Controller_Draws_2_Cards()
        {
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;

            // Create Skill
            SkillBuilder skillBuilder = new SkillBuilder {Description = "Draw 2 Cards"};
            skillBuilder.Triggers.Add(Triggers.Any);
            skillBuilder.Instructions.Add(Instructions.GetController);
            skillBuilder.Instructions.Add(Instructions.Draw);
            skillBuilder.Arguments.Push(2);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skills.Add(draw2Cards);
            
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            int handCountBeforeDraw = Player1.Hand.Count;
            Match.Activate(Player1, support);
            Assert.IsEqual(Player1.Hand.Count, handCountBeforeDraw + 2);
        }
        
        [Test]
        public void Opponent_Draws_Five_Cards()
        {
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;

            // Create Skill
            SkillBuilder skillBuilder = new SkillBuilder {Description = "Draw 5 Cards"};
            skillBuilder.Triggers.Add(Triggers.Any);
            skillBuilder.Instructions.Add(Instructions.GetOpponent);
            skillBuilder.Instructions.Add(Instructions.Draw);
            skillBuilder.Arguments.Push(5);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skills.Add(draw2Cards);
            
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            int handCountBeforeDraw = Player2.Hand.Count;
            Match.Activate(Player1, support);
            Assert.IsEqual(Player2.Hand.Count, handCountBeforeDraw + 5);
        }

        [Test]
        public void Destroy_All_Opponent_Units()
        {
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;

            // Create Skill
            SkillBuilder skillBuilder = new SkillBuilder {Description = "Draw 5 Cards"};
            skillBuilder.Triggers.Add(Triggers.Any);
            skillBuilder.Instructions.Add(Instructions.GetOpponent);
            skillBuilder.Instructions.Add(Instructions.GetUnits);
            skillBuilder.Instructions.Add(Instructions.Destroy);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skills.Add(draw2Cards);
            
            // Play Cards
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);

            // Deploy Units
            Card unitA = Player2.Hand[0];
            Card unitB = Player2.Hand[1];
            unitA.CardType = CardType.Unit;
            unitB.CardType = CardType.Unit;
            Match.Deploy(Player2, unitA);
            Assert.Contains(unitA, Player2.Units, "UnitA Was Deployed");
            Match.Deploy(Player2, unitB);
            Assert.Contains(unitB, Player2.Units, "UnitB Was Deployed");
            Match.EndTurn(Player2);
            
            // Activate
            Match.Activate(Player1, support);
            Assert.Contains(unitA, Player2.Graveyard, "Unit A is in P2 GY");
            Assert.Contains(unitB, Player2.Graveyard, "Unit B is in P2 GY");
            Assert.DoesNotContain(unitA, Player2.Units, "Unit A is not in P2 Units");
            Assert.DoesNotContain(unitB, Player2.Units, "Unit B is not in P2 Units");
            
        }
        
    }
}
using System;
using CardGame.Server;
using System.Collections.Generic;
using Godot;

//using Godot.Collections;

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
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(2);
            skillBuilder.Add(Instructions.GetController);
            skillBuilder.Add(Instructions.Draw);
            // skillBuilder.Arguments.Push(2);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skill = draw2Cards;

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
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(5);
            skillBuilder.Add(Instructions.GetOpponent);
            skillBuilder.Add(Instructions.Draw);
            //skillBuilder.Arguments.Push(5);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skill = draw2Cards;

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
            skillBuilder.Add(Instructions.GetOpponent);
            skillBuilder.Add(Instructions.GetUnits);
            skillBuilder.Add(Instructions.Destroy);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skill = draw2Cards;

            // Play Cards
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);

            // Deploy Units
            Card unitA = Player2.Hand[0];
            Card unitB = Player2.Hand[1];
            unitA.CardType = CardType.Unit;
            unitB.CardType = CardType.Unit;
            Match.Deploy(Player2, unitA);
            Match.Deploy(Player2, unitB);
            Match.EndTurn(Player2);

            // Activate
            Match.Activate(Player1, support);
            Assert.Contains(unitA, Player2.Graveyard);
            Assert.Contains(unitB, Player2.Graveyard);
            Assert.DoesNotContain(unitA, Player2.Units);
            Assert.DoesNotContain(unitB, Player2.Units);
        }

     
        
        [Test]
        public void Changes_Card_Faction()
        {
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;
            SkillBuilder skillBuilder = new SkillBuilder {Description = "Change this card's title to 'ChangedTitle'"};
            skillBuilder.Triggers.Add(Triggers.Any);
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(Faction.Warrior);
            skillBuilder.Add(Instructions.GetOwningCard);
            skillBuilder.Add(Instructions.SetFaction);
            Skill changeCardFaction = skillBuilder.CreateSkill(support);
            support.Skill = changeCardFaction;

            Faction previousFaction = support.Faction;
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            Match.Activate(Player1, support);
            
            Assert.IsEqual(support.Faction.ToString(), Faction.Warrior.ToString());
            Assert.IsNotEqual(support.Faction.ToString(), previousFaction);
        }
        
        [Test]
        public void Changes_Card_Power()
        {
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;
            SkillBuilder skillBuilder = new SkillBuilder {Description = "Change this card's title to 'ChangedTitle'"};
            skillBuilder.Triggers.Add(Triggers.Any);
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(1000);
            skillBuilder.Add(Instructions.GetOwningCard);
            skillBuilder.Add(Instructions.SetPower);
            Skill changeCardPower = skillBuilder.CreateSkill(support);
            support.Skill = changeCardPower;

            int previousPower = support.Power;
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            Match.Activate(Player1, support);
            Assert.IsEqual(1000, support.Power);
            Assert.IsNotEqual(previousPower, support);
        }
        
        // Draw A Card When you have six or less cards in your hand
        // Take Damage, When you have seven or more cards
        // covers if/else
        
        public enum Path { Happy, Sad }
        [RunWith(Path.Happy)]
        [RunWith(Path.Sad)]
        [Test]
        public void If_Hand_Count_Is_Less_Than_Seven_Draw_5_Cards_Else_Take_1000_Damage(Path path)
        {
            Describe(path == Path.Happy
                ? "Draw 5 cards if you have less than 7 cards in your hand else take 1000 damage (Happy Path)"
                : "Draw 5 cards if you have less than 7 cards in your hand else take 1000 damage (Sad Path)");

            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;
            SkillBuilder skillBuilder = new SkillBuilder {Description = "If you have less than seven cards " +
                                                                        "in your hand draw 2 cards else take 1000 damage"};
            skillBuilder.Triggers.Add(Triggers.Any);
            
            // Jump Inst
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(5);
            
            // Comparing Against
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(7);

            // Hand Count
            skillBuilder.Add(Instructions.GetController);
            skillBuilder.Add(Instructions.GetHand);
            skillBuilder.Add(Instructions.Count);
            skillBuilder.Add(Instructions.IfLessThan);
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(5);
            skillBuilder.Add(Instructions.GetController);
            skillBuilder.Add(Instructions.Draw);
            skillBuilder.Add(Instructions.GoToEnd);
            
            // Else Branch
            
            // DiscardArgument is to clear arguments from the other branch we don't care about
           // skillBuilder.Add(Instructions.DiscardArgument);
            skillBuilder.Add(Instructions.Literal);
            skillBuilder.Add(1000);
            skillBuilder.Add(Instructions.GetController);
            skillBuilder.Add(Instructions.DealDamage);
            
            Skill changeCardPower = skillBuilder.CreateSkill(support);
            support.Skill = changeCardPower;

            Match.SetFaceDown(Player1, support);

            if (path == Path.Happy)
            {
                Card support2 = Player1.Hand[1];
                support2.CardType = CardType.Support;
                Match.SetFaceDown(Player1, support2);
            }

            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            
            int previousLife = Player1.Health;
            int previousHandCount = Player1.Hand.Count;
            Match.Activate(Player1, support);
            
            if (path == Path.Happy)
            {
                Assert.IsEqual(Player1.Hand.Count, previousHandCount + 5);
            }
            else
            {
                Assert.IsEqual(Player1.Health, previousLife - 1000);
            }
        }
    }
}
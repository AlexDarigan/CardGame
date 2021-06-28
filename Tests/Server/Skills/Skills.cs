﻿using System;
using CardGame.Server;
using System.Collections.Generic;
using Godot;

//using Godot.Collections;

namespace CardGame.Tests.Server.Actions
{
    public class Skills : BaseServerTest
    {
        [Test]
        public void Controller_Draws_2_Cards()
        {
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;

            SkillBuilder skill = new SkillBuilder {Description = "Draw 2 Cards"};
            skill.Triggers.Add(Triggers.Any);
            skill.Add(Instructions.Literal);
            skill.Add(2);
            skill.Add(Instructions.GetController);
            skill.Add(Instructions.Draw);
            support.Skill = skill.Build(support);

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
            SkillBuilder skill = new SkillBuilder {Description = "Draw 5 Cards"};
            skill.Triggers.Add(Triggers.Any);
            skill.Add(Instructions.Literal);
            skill.Add(5);
            skill.Add(Instructions.GetOpponent);
            skill.Add(Instructions.Draw);
            support.Skill = skill.Build(support);

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
            SkillBuilder skill = new SkillBuilder {Description = "Draw 5 Cards"};
            skill.Triggers.Add(Triggers.Any);
            skill.Add(Instructions.GetOpponent);
            skill.Add(Instructions.GetUnits);
            skill.Add(Instructions.Destroy);
            support.Skill = skill.Build(support);

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
            SkillBuilder skill = new SkillBuilder {Description = "Change this card's title to 'ChangedTitle'"};
            skill.Triggers.Add(Triggers.Any);
            skill.Add(Instructions.Literal);
            skill.Add(Faction.Warrior);
            skill.Add(Instructions.GetOwningCard);
            skill.Add(Instructions.SetFaction);
            support.Skill = skill.Build(support);

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
            SkillBuilder skill = new SkillBuilder {Description = "Change this card's title to 'ChangedTitle'"};
            skill.Triggers.Add(Triggers.Any);
            skill.Add(Instructions.Literal);
            skill.Add(1000);
            skill.Add(Instructions.GetOwningCard);
            skill.Add(Instructions.SetPower);
            support.Skill = skill.Build(support);

            int previousPower = support.Power;
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            Match.Activate(Player1, support);
            Assert.IsEqual(1000, support.Power);
            Assert.IsNotEqual(previousPower, support);
        }
    }
}
using CardGame.Server;

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
            skillBuilder.Instructions.Add(Instructions.Two);
            skillBuilder.Instructions.Add(Instructions.Draw);
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
            skillBuilder.Instructions.Add(Instructions.Five);
            skillBuilder.Instructions.Add(Instructions.Draw);
            Skill draw2Cards = skillBuilder.CreateSkill(support);
            support.Skills.Add(draw2Cards);
            
            Match.SetFaceDown(Player1, support);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            int handCountBeforeDraw = Player2.Hand.Count;
            Match.Activate(Player1, support);
            Assert.IsEqual(Player2.Hand.Count, handCountBeforeDraw + 5);
        }
    }
}
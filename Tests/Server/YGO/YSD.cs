namespace CardGame.Server.Tests.YGO
{
    public class YSD: Fixture
    {
        // A number of tests created implementing cards from Yu-Gi-Oh implementing their skills
        // Some of these may require client-side tests (targeting-skills, option-skills, multi-targeting skills)

        // [Test]
        // public void DarkHole()
        // {
        //     Card support = CommonPlay(OpCodes.GetController, OpCodes.GetUnits, OpCodes.GetOpponent, OpCodes.GetUnits, OpCodes.Destroy);
        //     // Card support = CommonPlay(OpCodes.Literal, a, OpCodes.Literal, b, math, 
        //     //     OpCodes.GetController, OpCodes.SetHealth);
        //     // int health = P1.Health;
        //     // Match.Activate(P1, support);
        //     // Match.PassPlay(P2);
        //     // Match.PassPlay(P1);
        //     // Assert.IsEqual(P1.Health, result, context);
        // }
        
        private Card CommonPlay(params object[] ops)
        {
            StartGame(BuildDeck(SetCodes.BasicSupport));
            Card support = P1.Hand[0];
            support.Skill = BuildSkill(support, ops);
            Match.SetFaceDown(P1, support);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            return support;
        }
    }
}
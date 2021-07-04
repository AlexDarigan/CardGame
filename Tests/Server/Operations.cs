using System;

namespace CardGame.Server.Tests
{
    [Title("Operations")]
    public class Operations: Fixture
    {
        [Test(true, OpCodes.GetOwner, "The skills owner drew 3 cards")]
        [Test(true, OpCodes.GetController, "The skills controller drew 3 cards")]
        [Test(false, OpCodes.GetOpponent, "The skills opponent drew 3 cards")]
        public void PlayerGetter(bool isPlayer1, OpCodes getPlayer, string context)
        {
            Card support = CommonPlay(OpCodes.Literal, 3, getPlayer, OpCodes.Draw);
            Player player = isPlayer1 ? P1 : P2;
            int count = player.Hand.Count;
            Match.Activate(P1, support);
            Assert.IsEqual(player.Hand.Count, count + 3, context);
        }
        
        [Test(OpCodes.GetDeck, "Deck", 0, 39, "Player drew a card for each card in their deck")]
        [Test(OpCodes.GetHand, "Hand", 0, 14, "Player drew a card for each card in their hand")]
        [Test(OpCodes.GetUnits, "Units", 2, 9, "Player drew a card for each Unit on their field")]
        [Test(OpCodes.GetSupport, "Supports", 0, 8, "Player drew a card for each Support on their field (including the activated card)")]
        [Test(OpCodes.GetGraveyard, "Graveyard", 4, 11, "Player drew a card for each card in their graveyard")]
        public void CardGetter(OpCodes getZone, string zoneToAddTo, int cardsToAdd, int expected, string context)
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card support = CommonPlay(OpCodes.GetController, getZone, OpCodes.CountCards, OpCodes.GetController, OpCodes.Draw);
            Zone zone = (Zone) typeof(Player).GetProperty(zoneToAddTo)!.GetValue(P1);
            for(int i = 0; i < cardsToAdd; i++) { zone!.Add(new Card(0, P1)); }
            Match.Activate(P1, support);
            Assert.IsEqual(P1.Hand.Count, expected, context);
        }

        [Test(3, 3, OpCodes.IsEqual, "Opponent drew 5 cards because 3 is equal to 3")]
        [Test(5, 1, OpCodes.IsNotEqual, "Opponent drew 5 cards because 5 is not equal to 1")]
        [Test(9, 2, OpCodes.IsGreaterThan, "Opponent drew 5 cards because 9 is greater than 2")]
        [Test(2, 3, OpCodes.IsLessThan, "Opponent drew 5 cards because 2 is less than 3")]
        [Test(1, 1, OpCodes.And, "Opponent drew 5 cards because 1 AND 1 is true")]
        [Test(1, 0, OpCodes.Or, "Opponent drew 5 cards because 1 OR 0 is true")]
        public void Comparison(int a, int b, OpCodes comparison, string context)
        {
            const int jump = 4;
            Card support = CommonPlay(OpCodes.Literal, a, OpCodes.Literal, b, comparison, 
                OpCodes.Literal, jump, OpCodes.If, OpCodes.Literal, 5, OpCodes.GetOpponent, OpCodes.Draw);
            int count = P2.Hand.Count;
            Match.Activate(P1, support);
            Assert.IsEqual(P2.Hand.Count, count + 5, context);
        }

        [Test(OpCodes.Add, 500, 500, 1000, "Controllers health is set to the result of 500 + 500")]
        [Test(OpCodes.Subtract, 1000, 500, 500, "Controllers health is set to the result of 1000 - 500")]
        [Test(OpCodes.Multiply, 2, 1000, 2000, "Controllers health is set to the result of 2 x 1000")]
        [Test(OpCodes.Divide, 1000, 2, 500, "Controllers health is set to the result of 1000 / 2")]
        public void Calculation(OpCodes math, int a, int b, int result, string context)
        {
            Card support = CommonPlay(OpCodes.Literal, a, OpCodes.Literal, b, math, 
                OpCodes.GetController, OpCodes.SetHealth);
            int health = P1.Health;
            Match.Activate(P1, support);
            Assert.IsEqual(P1.Health, result, context);
        }
        
        private Card CommonPlay(params object[] ops)
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card support = P1.Hand[0];
            support.Skill = BuildSkill(support, ops);
            Match.SetFaceDown(P1, support);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            return support;
        }
    }
}
namespace CardGame.Server.Tests
{
    [Title("Player Disqualifications")]
    public class Disqualifications: Fixture
    {
        
        [Test("Illegal Draw", new object[]{nameof(Match.Draw), States.IdleTurnPlayer, Illegal.Draw})]
        [Test("Illegal Pass Play", new object[]{nameof(Match.PassPlay), States.Active, Illegal.PassPlay})]
        [Test("Illegal End Turn", new object[]{nameof(Match.EndTurn), States.IdleTurnPlayer, Illegal.EndTurn})]
        public void IllegalPlayerActions(string method, States states, Illegal reason)
        {
            StartGame();
            Assert.IsNotEqual(P2.State, states);
            typeof(Match).GetMethod(method)!.Invoke(Match, new object[]{P2});
            Assert.IsEqual(P2.ReasonPlayerWasDisqualified, reason);
        }

        // All of our disqualification information is wrapped up into the card state so we (probably) only
        // ..need to check against the card state
        [Test("Illegal Deploy", new object[]{nameof(Match.Deploy), CardState.Deploy, Illegal.Deploy})]
        [Test("Illegal Set FaceDown", new object[]{nameof(Match.SetFaceDown), CardState.Set, Illegal.SetFaceDown})]
        [Test("Illegal Activation", new object[]{nameof(Match.Activate), CardState.Activate, Illegal.Activation})]
        [Test("Illegal Attack Player", new object[]{nameof(Match.DeclareDirectAttack), CardState.AttackPlayer, Illegal.AttackPlayer})]
        public void IllegalCardActions(string method, CardState state, Illegal reason)
        {
            StartGame();
            Card card = P2.Hand[0];
            Assert.IsNotEqual(card.CardState, state);
            typeof(Match).GetMethod(method)!.Invoke(Match, new object[]{P2, card});  
            Assert.IsEqual(P2.ReasonPlayerWasDisqualified, reason);
        }

        [Test("Illegal Attack Unit")]
        public void IllegalAttackUnit()
        {
            StartGame();
            Card card = P2.Hand[0];
            Match.DeclareAttack(P2, card, P1.Hand[0]);
            Assert.IsNotEqual(card, CardState.AttackUnit);
            Assert.IsEqual(P2.ReasonPlayerWasDisqualified, Illegal.AttackUnit);
        }
        
    }
}
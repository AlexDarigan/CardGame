﻿namespace CardGame.Server.Tests
{
    [Title("Player Disqualifications")]
    public class Disqualifications: Fixture
    {
        
        [Test("Illegal Pass Play", nameof(Match.PassPlay), States.Active, Illegal.PassPlay)]
        [Test("Illegal End Turn", nameof(Match.EndTurn), States.IdleTurnPlayer, Illegal.EndTurn)]
        public void IllegalPlayerActions(string description, string method, States states, Illegal reason)
        {   
            Describe(description);
            StartGame();
            Assert.IsNotEqual(P2.State, states);
            typeof(Match).GetMethod(method)!.Invoke(Match, new object[]{P2});
            Assert.IsEqual(P2.ReasonPlayerWasDisqualified, reason);
        }

        // All of our disqualification information is wrapped up into the card state so we (probably) only
        // ..need to check against the card state
        [Test("Illegal Deploy", nameof(Match.Deploy), CardGame.CardStates.Deploy, Illegal.Deploy)]
        [Test("Illegal Set FaceDown", nameof(Match.SetFaceDown), CardGame.CardStates.SetFaceDown, Illegal.SetFaceDown)]
        [Test("Illegal Activation", nameof(Match.Activate), CardGame.CardStates.Activate, Illegal.Activation)]
        [Test("Illegal Attack Player", nameof(Match.DeclareDirectAttack), CardGame.CardStates.AttackPlayer, Illegal.AttackPlayer)]
        public void IllegalCardActions(string description, string method, CardGame.CardStates states, Illegal reason)
        {
            Describe(description);
            StartGame();
            Card card = P2.Hand[0];
            Assert.IsNotEqual(card.CardStates, states);
            typeof(Match).GetMethod(method)!.Invoke(Match, new object[]{P2, card});  
            Assert.IsEqual(P2.ReasonPlayerWasDisqualified, reason);
        }

        [Test()]
        public void IllegalAttackUnit()
        {
            StartGame();
            Card card = P2.Hand[0];
            Match.DeclareAttack(P2, card, P1.Hand[0]);
            Assert.IsNotEqual(card, CardGame.CardStates.AttackUnit);
            Assert.IsEqual(P2.ReasonPlayerWasDisqualified, Illegal.AttackUnit);
        }
        
    }
}
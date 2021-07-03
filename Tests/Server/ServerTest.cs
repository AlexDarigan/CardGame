using System.Collections.Generic;
using System.Threading;
using CardGame.Server;
using Godot;
using WAT;

namespace CardGame.Tests.Server
{
    public class DeckBuilder
    {
        private readonly List<SetCodes> _deckList = new();
        public void Add(SetCodes setCodes, int count) { for(int i = 0; i < count; i++) { _deckList.Add(setCodes); } }

        public IEnumerable<SetCodes> Default(SetCodes setCodes = SetCodes.NullCard)
        {
            for(int i = 0; i < 40; i++){ _deckList.Add(setCodes);}
            return _deckList;
        }
        
    }
    
    [Title("Given A Card")]
    public class ServerTest: WAT.Test
    {
        // CREATE A DECKBUILDER HELPER
        private Player P1;
        private Player P2;
        private Match _match;

        public void StartGame(IEnumerable<SetCodes> deckList1 = null, IEnumerable<SetCodes> deckList2 = null)
        {
            P1 = new Player(1, deckList1 ?? new DeckBuilder().Default());
            P2 = new Player(2,deckList2 ?? new DeckBuilder().Default());
            _match = new Match(P1, P2, new Cards(), () => {}, (id, command, args) => {});
            _match.Begin(new List<Player> {P1, P2});
        }
        
        
        [Test]
        public void DeployState()
        {
            StartGame(new DeckBuilder().Default(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Hand, "And it is in their controller's hand");
            Assert.IsLessThan(card.Controller.Units.Count, 5, "And its Controller's Unit Zones is not full");
            Assert.IsEqual(card.CardState, CardState.Deploy, "Then they can be deployed");
        }

        [Test]
        public void SetFaceDownState()
        {
            StartGame(new DeckBuilder().Default(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardType.Support, "When it is a Support Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Hand, "And it is in its controller's hand");
            Assert.IsLessThan(card.Controller.Supports.Count, 5, "And its Controller's Support Zones is not full");
            Assert.IsEqual(card.CardState, CardState.Set, "Then it can be set face down");
        }

        [Test]
        public void AttackUnitState()
        {
            StartGame(new DeckBuilder().Default(SetCodes.AlphaBioShocker), new DeckBuilder().Default(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];
            _match.Deploy(P1, card);
            _match.EndTurn(P1);
            Card defender = P2.Hand[1];
            _match.Deploy(P2, defender);
            _match.EndTurn(P2);
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, P1.Units, "And it is in its controller's units");
            Assert.IsTrue(card.IsReady, "And it is ready");
            Assert.IsGreaterThan(P2.Units.Count, 0, "And its controller's opponent's Unit zone is not empty");
            Assert.IsEqual(card.CardState, CardState.AttackUnit, "Then it can attack target unit");
        }

        [Test]
        public void AttackPlayerState()
        {
            StartGame(new DeckBuilder().Default(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];
            _match.Deploy(P1, card);
            _match.EndTurn(P1);
            _match.EndTurn(P2);
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, P1.Units, "And it is in its controller's units");
            Assert.IsTrue(card.IsReady, "And it is ready");
            Assert.IsEqual(P2.Units.Count, 0, "And its controller's opponent's Unit zone is empty");
            Assert.IsEqual(card.CardState, CardState.AttackPlayer, "Then it can attack directly");
        }
        
        // ACTION TESTS
        [Test]
        public void DrawAction()
        {
            // DECK IS 1 LESS
            // HAND IS 1 MORE
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void DeployAction()
        {
            // HAND IS 1 LESS
            // UNITS IS 1 MORE
            Assert.Fail("Not Implemented");

        }
        
        [Test]
        public void SetFaceDownAction()
        {
            // HAND IS 1 LESS
            // SUPPORTS IS 1 MORE
            Assert.Fail("Not Implemented");

        }
        
        [Test]
        public void ActivationAction()
        {
            // SUPPORT IS 1 LESS
            // DISCARD IS 1 MORE
            Assert.Fail("Not Implemented");

        }
        
        [Test]
        public void AttackPlayerAction()
        {
            // Player Life Is Less
            // Card is Exhausted
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalDraw()
        {
            // DRAW
            // When they are not the turn player
            // AssetDisqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalDeploy()
        {
            // DEPLOY
            // When they are not turn player
            // When their units is packed
            // When their unit is not in their hand
            // When their card is not a Unit
            // Assert Disqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalSetFaceDown()
        {
            // SET FACEDOWN
            // When they are not the turn player
            // When their supports is packed
            // When their support is not in their hand
            // When their card is not a Support
            // Assert Disqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalActivation()
        {
            // ACTIVATE CARD
            // ???????
            // Not idle/active player
            // Card is not support
            // Card is not ready
            // Card is not in Support
            // Assert Disqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalAttackUnit()
        {
            // ATTACK UNIT
            // When they are not the turn player
            // When the attack target is not on the field
            // When the attacker is not on the field
            // When the attacker is not ready
            // Assert disqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalAttackPlayer()
        {
            // ATTACK PLAYER
            // When they are not the turn player
            // When the attacker is not on the field
            // When the attacker is not ready
            // When the player has units
            // Assert Disqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalPassPlay()
        {
            // When they are not idle or active player
            // assert disqualified
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void IllegalEndTurn()
        {
            // When they are not the turn player
            Assert.Fail("Not Implemented");

        }
        
        // PlayerState - Idle -> Passive OnDeploy
        // PlayerState - Idle -> Passive OnAttack
        // PlayerState - Idle -> Passive OnActivation
        // PlayerState - Idle -> Idle OnSetFacedown
        // PlayerState - Idle -> Passive OnPassPlay
        // PlayerState - Active -> Passive OnPassPlay
        // PlayerState - Idle -> Passive OnEndTurn
        // PlayerState - Passive -> Active OnOpponentDeploy
        // PlayerState - Passive -> Active OnOpponentAttack
        // PlayerState - Passive -> Active OnOpponentActivation
        // PlayerState - Passive -> Active OnOpponentPassPlay
        // PlayerState - Passive -> TurnPlayer on OpponentEndTurn
        
        // GAME OVER ON NO CARDS LEFT TO
        // GAME OVER ON LIFE REACHING WINS 0
        // GAME OVER, WINNER, LOSER
        
        // TO BE CONTINUED:
            // BATTLE
            // SKILLS
            // CREATION
            // CONNECTIONS
   
        


    }
}
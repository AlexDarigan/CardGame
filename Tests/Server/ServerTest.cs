using System.Collections.Generic;
using System.Threading;
using CardGame.Server;
using Godot;
using WAT;

namespace CardGame.Tests.Server
{
    public class DeckBuilder
    {
        public List<SetCodes> DeckList = new();
        public void Add(SetCodes setCodes, int count) { for(int i = 0; i < count; i++) { DeckList.Add(setCodes); } }

        public List<SetCodes> Default()
        {
            for(int i = 0; i < 40; i++){ DeckList.Add(SetCodes.AlphaBioShocker);}
            return DeckList;
        }
        
    }
    
    public class ServerTest: WAT.Test
    {
        // CREATE A DECKBUILDER HELPER
        private Player P1;
        private Player P2;
        private Match _match;

        public void StartGame()
        {
            P1 = new Player(1, new DeckBuilder().Default());
            P2 = new Player(2, new DeckBuilder().Default());
            _match = new Match(P1, P2, new Cards(), () => {}, (id, command, args) => {});
            _match.Begin(new List<Player> {P1, P2});
        }
        
        
        [Test]
        [Description("A Card may be deployed if")]
        public void DeployState()
        {
            StartGame();
            Card card = P1.Hand[0];
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "Their controller is Idle Turn Player");
            Assert.IsEqual(card.CardType, CardType.Unit, "And their card type is Unit");
            Assert.Contains(card, card.Controller.Hand, "And they are in their controllers hand");
            Assert.IsLessThan(card.Controller.Units.Count, 5, "And there is room in untis");
            Assert.IsEqual(card.CardState, CardState.Deploy, "Then they can be deployed");
        }

        [Test]
        public void SetFaceDownState()
        {
            // CAN BE SET IF
            // Support < 5
            // Card is in hand
            // Card is Unit
            // Player is Idle Turn Player
            // AssertCardState
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void AttackUnitState()
        {
            // CAN ATTACK UNIT IF X, Y, Z
            // If Unit is Ready
            // If Player is Idle Turn Player
            // If Unit is in Units
            // If Target is in Units // May Be out of place?
            // If Opponent Field Not Empty
            // Asset-CheckCardState
            Assert.Fail("Not Implemented");

        }

        [Test]
        public void AttackPlayerState()
        {
            // CAN ATTACK DIRECTLY IF X, Y, Z
            // If Unit is Ready
            // If Player is Idle Turn Player
            // If Unit is in Units
            // If Opponent Field Empty
            // Asset-CheckCardState
            // CAN BE ACTIVATED IF X, Y, Z
            Assert.Fail("Not Implemented");

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
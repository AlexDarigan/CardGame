//
// using WAT;
//
// namespace CardGame.Server.Tests
// {
//     public class ServerTest
//     {
//         // ACTION TESTS
//         [Test]
//         public void DrawAction()
//         {
//             // DECK IS 1 LESS
//             // HAND IS 1 MORE
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void DeployAction()
//         {
//             // HAND IS 1 LESS
//             // UNITS IS 1 MORE
//             Assert.Fail("Not Implemented");
//
//         }
//         
//         [Test]
//         public void SetFaceDownAction()
//         {
//             // HAND IS 1 LESS
//             // SUPPORTS IS 1 MORE
//             Assert.Fail("Not Implemented");
//
//         }
//         
//         [Test]
//         public void ActivationAction()
//         {
//             // SUPPORT IS 1 LESS
//             // DISCARD IS 1 MORE
//             Assert.Fail("Not Implemented");
//
//         }
//         
//         [Test]
//         public void AttackPlayerAction()
//         {
//             // Player Life Is Less
//             // Card is Exhausted
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalDraw()
//         {
//             // DRAW
//             // When they are not the turn player
//             // AssetDisqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalDeploy()
//         {
//             // DEPLOY
//             // When they are not turn player
//             // When their units is packed
//             // When their unit is not in their hand
//             // When their card is not a Unit
//             // Assert Disqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalSetFaceDown()
//         {
//             // SET FACEDOWN
//             // When they are not the turn player
//             // When their supports is packed
//             // When their support is not in their hand
//             // When their card is not a Support
//             // Assert Disqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalActivation()
//         {
//             // ACTIVATE CARD
//             // ???????
//             // Not idle/active player
//             // Card is not support
//             // Card is not ready
//             // Card is not in Support
//             // Assert Disqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalAttackUnit()
//         {
//             // ATTACK UNIT
//             // When they are not the turn player
//             // When the attack target is not on the field
//             // When the attacker is not on the field
//             // When the attacker is not ready
//             // Assert disqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalAttackPlayer()
//         {
//             // ATTACK PLAYER
//             // When they are not the turn player
//             // When the attacker is not on the field
//             // When the attacker is not ready
//             // When the player has units
//             // Assert Disqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalPassPlay()
//         {
//             // When they are not idle or active player
//             // assert disqualified
//             Assert.Fail("Not Implemented");
//
//         }
//
//         [Test]
//         public void IllegalEndTurn()
//         {
//             // When they are not the turn player
//             Assert.Fail("Not Implemented");
//
//         }
//         
//         // PlayerState - Idle -> Passive OnDeploy
//         // PlayerState - Idle -> Passive OnAttack
//         // PlayerState - Idle -> Passive OnActivation
//         // PlayerState - Idle -> Idle OnSetFacedown
//         // PlayerState - Idle -> Passive OnPassPlay
//         // PlayerState - Active -> Passive OnPassPlay
//         // PlayerState - Idle -> Passive OnEndTurn
//         // PlayerState - Passive -> Active OnOpponentDeploy
//         // PlayerState - Passive -> Active OnOpponentAttack
//         // PlayerState - Passive -> Active OnOpponentActivation
//         // PlayerState - Passive -> Active OnOpponentPassPlay
//         // PlayerState - Passive -> TurnPlayer on OpponentEndTurn
//         
//         // GAME OVER ON NO CARDS LEFT TO
//         // GAME OVER ON LIFE REACHING WINS 0
//         // GAME OVER, WINNER, LOSER
//         
//         // TO BE CONTINUED:
//             // BATTLE
//             // SKILLS
//             // CREATION
//             // CONNECTIONS
//    
//         
//
//
//     }
// }
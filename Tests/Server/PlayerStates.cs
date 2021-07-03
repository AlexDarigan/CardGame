namespace CardGame.Server.Tests
{
    [Title("Player States")]
    public class PlayerStates: Fixture
    {
        // Most of these tests won't really work without a skill-based system

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
    }
}
using CardGame.Server;

namespace CardGame.Tests.Server.Rules
{
    public class MatchRules: BaseTest
    {
        /*
         * We care about the rules in this test. We want to see if our rule checks are working correctly and
         * effectively. While we do care about rules, we can modify some public state on an object to see if a
         * particular check is working. We should only invoke actions that are under test via our game script.
         */
        [Test]
        public void They_Draw_During_Their_Opponents_Turn()
        {
            Match.Draw(Player2);
            Assert.IsTrue(Player2.Disqualified);
        }
        
        [Test]
        public void They_Draw_In_A_NonIdle_State()
        {
            Player1.State = States.Passive;
            Match.Draw(Player1);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Deploy_During_Their_Opponents_Turn()
        {
            
            Match.Deploy(Player2, Player2.Hand[0]);
            Assert.IsTrue(Player2.Disqualified);
        }
        
        [Test]
        public void They_Deploy_During_Their_Turn_In_A_NonIdle_State()
        {
            Player1.State = States.Passive;
            Match.Deploy(Player1, Player1.Hand[0]);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Deploy_A_NonUnit_Card()
        {
            Card card = Player1.Hand[0];
            card.CardType = CardType.Support;
            Match.Deploy(Player1, card);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Set_A_Support_FaceDown_During_Their_Opponents_Turn()
        {
            Card card = Player2.Hand[0];
            card.CardType = CardType.Support;
            Match.SetFaceDown(Player2, card);
            Assert.IsTrue(Player2.Disqualified);
        }
        
        [Test]
        public void They_Set_A_Support_FaceDown_During_Their_Turn_In_A_NonIdle_State()
        {
            Card card = Player1.Hand[0];
            card.CardType = CardType.Support;
            Player1.State = States.Passive;
            Match.SetFaceDown(Player1, card);
            Assert.IsTrue(Player1.Disqualified);
        }

        
        [Test]
        public void They_Set_A_NonSupport_Card_FaceDown()
        {
            Card card = Player1.Hand[0];
            card.CardType = CardType.Unit;
            Match.SetFaceDown(Player1, card);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Activate_A_Card_That_Is_Not_On_Their_Field()
        {
            Card card = Player1.Hand[0];
            card.CardType = CardType.Support;
            Match.Activate(Player1, card);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Activate_A_Card_That_In_A_State_That_Is_Not_Idle_Or_Active()
        {
            Card card = Player1.Hand[0];
            card.CardType = CardType.Support;
            Match.SetFaceDown(Player1, card);
            Match.EndTurn(Player1);
            Match.Activate(Player1, card);
            Assert.IsTrue(Player1.Disqualified);
        }

        [Test]
        public void They_Declare_An_Attack_When_They_Are_Not_The_Turn_Player()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player1);
            Match.DeclareAttack(Player1, attacker, defender);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_An_Attack_When_They_Are_In_A_NonIdle_State()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            Player1.State = States.Passive;
            Match.DeclareAttack(Player1, attacker, defender);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_An_Attack_With_An_Unready_Unit()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            Match.Deploy(Player1, attacker);
            Match.DeclareAttack(Player1, attacker, defender);
            Assert.IsTrue(Player1.Disqualified);
        }

        [Test]
        public void They_Declare_An_Attack_With_A_Card_That_Is_Not_On_The_Field()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            Match.DeclareAttack(Player1, attacker, defender);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_A_Direct_Attack_When_They_Are_Not_The_Turn_Player()
        {
            Card attacker = Player1.Hand[0];
            attacker.CardType = CardType.Unit;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.EndTurn(Player1);
            Match.DeclareDirectAttack(Player1, attacker);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_A_Direct_Attack_When_They_Are_In_A_NonIdle_State()
        {
            Card attacker = Player1.Hand[0];
            attacker.CardType = CardType.Unit;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            Player1.State = States.Passive;
            Match.DeclareDirectAttack(Player1, attacker);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_A_Direct_Attack_With_An_Unready_Unit()
        {
            Card attacker = Player1.Hand[0];
            attacker.CardType = CardType.Unit;
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            Match.Deploy(Player1, attacker);
            Match.DeclareDirectAttack(Player1, attacker);
            Assert.IsTrue(Player1.Disqualified);
        }

        [Test]
        public void They_Declare_A_Direct_Attack_With_A_Card_That_Is_Not_On_The_Field()
        {
            Card attacker = Player1.Hand[0];
            attacker.CardType = CardType.Unit;
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            Match.DeclareDirectAttack(Player1, attacker);
            Assert.IsTrue(Player1.Disqualified);
        }

        [Test]
        public void They_Declare_A_Direct_Attack_Against_A_NonEmpty_Field()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            Match.DeclareDirectAttack(Player1, attacker);
            Assert.IsTrue(Player1.Disqualified);
        }
        
        [Test]
        public void They_End_Their_Turn_During_Their_Opponents_Turn()
        {
            Match.EndTurn(Player2);
            Assert.IsTrue(Player2.Disqualified);
        }
        
        [Test]
        public void They_End_Their_Turn_In_A_NonIdle_State()
        {
            Player1.State = States.Passive;
            Match.EndTurn(Player1);
            Assert.IsTrue(Player1.Disqualified);
        }
    }
}
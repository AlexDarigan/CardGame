using System;

namespace CardGame.Server
{
    public class Card
    {
        public readonly int Id;
        public readonly Player Owner;
        public CardState CardState = CardState.None;
        public CardType CardType;
        public Player Controller;
        public Faction Faction;
        public bool IsReady = false;
        public int Power;
        public SetCodes SetCodes;
        public Skill Skill;
        public string Title;
        public Zone Zone;

        public Card(int id, Player owner)
        {
            Id = id;
            Owner = owner;
            Controller = owner;
        }

        public void Update()
        {
            if (Controller.State != States.IdleTurnPlayer) { CardState = CardState.None; }
            else if (CanBeDeployed()) { CardState = CardState.Deploy; }
            else if (CanBeSetFaceDown()) { CardState = CardState.Set;}
            else if (CanAttackUnit()) { CardState = CardState.AttackUnit;}
            else if (CanAttackPlayer()) { CardState = CardState.AttackPlayer; }
            else { CardState = CardState.None; }
        }

        private bool CanBeDeployed() => Controller.Hand.Contains(this) && CardType is CardType.Unit;
        private bool CanBeSetFaceDown() => Controller.Hand.Contains(this) && CardType is CardType.Support;
        private bool CanAttackUnit() => Controller.Units.Contains(this) && IsReady && Controller.Opponent.Units.Count > 0;
        private bool CanAttackPlayer() => Controller.Units.Contains(this) && IsReady && Controller.Opponent.Units.Count == 0;
    }
}
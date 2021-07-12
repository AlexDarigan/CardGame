using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
    public class Card
    {
        public int Id { get; }
        public Player Owner { get; }
        public CardStates CardStates = CardStates.None;
        public CardTypes CardTypes;
        public Player Controller;
        public Factions Factions;
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
            if (Controller.State != States.IdleTurnPlayer) { CardStates = CardStates.None; }
            else if (CanBeDeployed()) { CardStates = CardStates.Deploy; }
            else if (CanBeSetFaceDown()) { CardStates = CardStates.SetFaceDown;}
            else if (CanAttackUnit()) { CardStates = CardStates.AttackUnit;}
            else if (CanAttackPlayer()) { CardStates = CardStates.AttackPlayer; }
            else if (CanBeActivated()) { CardStates = CardStates.Activate; }
            else { CardStates = CardStates.None; }
        }

        private bool CanBeDeployed() => Controller.Hand.Contains(this) && CardTypes is CardTypes.Unit;
        private bool CanBeSetFaceDown() => Controller.Hand.Contains(this) && CardTypes is CardTypes.Support;
        private bool CanAttackUnit() => Controller.Units.Contains(this) && IsReady && Controller.Opponent.Units.Count > 0;
        private bool CanAttackPlayer() => Controller.Units.Contains(this) && IsReady && Controller.Opponent.Units.Count == 0;
        private bool CanBeActivated() => Controller.Supports.Contains(this) && IsReady;
        public SkillState Activate() => new SkillState(this, Skill.OpCodes);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Events;

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
            CardStates = Controller.State switch
            {
                States.IdleTurnPlayer when CanBeDeployed() => CardStates.Deploy,
                States.IdleTurnPlayer when CanBeSetFaceDown() => CardStates.SetFaceDown,
                States.IdleTurnPlayer when CanAttackUnit() => CardStates.AttackUnit,
                States.IdleTurnPlayer when CanAttackPlayer() => CardStates.AttackPlayer,
                States.IdleTurnPlayer when CanBeActivated() => CardStates.Activate,
                States.Active when CanBeActivated() => CardStates.Activate,
                _ => CardStates.None
            };
        }

        private bool CanBeDeployed() => Controller.Hand.Contains(this) && CardTypes is CardTypes.Unit;
        private bool CanBeSetFaceDown() => Controller.Hand.Contains(this) && CardTypes is CardTypes.Support;
        private bool CanAttackUnit() => Controller.Units.Contains(this) && IsReady && Controller.Opponent.Units.Count > 0;
        private bool CanAttackPlayer() => Controller.Units.Contains(this) && IsReady && Controller.Opponent.Units.Count == 0;
        private bool CanBeActivated() => Controller.Supports.Contains(this) && IsReady;
        public (Activation, SkillState) Activate() => (new Activation(this), new SkillState(this, Skill.OpCodes));
    }
}
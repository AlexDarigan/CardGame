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
            CardState = CardState.None;
            if (Controller.Hand.Contains(this) && CardType is CardType.Unit &&
                Controller.State == States.IdleTurnPlayer) CardState = CardState.Deploy;
            if (Controller.Hand.Contains(this) && CardType is CardType.Support &&
                Controller.State == States.IdleTurnPlayer) CardState = CardState.Set;
            if (Controller.Units.Contains(this) && IsReady && Controller.State == States.IdleTurnPlayer)
                CardState = CardState.AttackUnit;
            if (Controller.Units.Contains(this) && IsReady
                                                && Controller.State == States.IdleTurnPlayer
                                                && Controller.Opponent.Units.Count == 0)
                CardState = CardState.AttackUnit;
        }
    }
}
namespace CardGame.Client.Views
{
    public class Faction: CardProperty<Factions>
    {
        public Faction(Card card) { Card = card;}
        public override void Set(Factions value) { Value = value; }
        public override Factions Get() { return Value; }
    }
}
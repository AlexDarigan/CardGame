namespace CardGame.Client.Views
{
    public class Title: CardProperty<string>
    {
        public Title(Card card) { Card = card;}
        public override void Set(string value) { Value = value; }
        public override string Get() { return Value; }
    }
}
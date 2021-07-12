namespace CardGame.Client.Views
{
    public class Text: CardProperty<string>
    {
        public Text(Card card) { Card = card;}
        public override void Set(string value) { Value = value; }
        public override string Get() { return Value; }
    }
}
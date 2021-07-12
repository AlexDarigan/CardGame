namespace CardGame.Client.Views
{
    public class Id: CardProperty<int>
    {
        private bool HasBeenSet { get; set; }
        public Id(Card card) { Card = card;}

        public override void Set(int value)
        {
            if (HasBeenSet) return;
            HasBeenSet = true;
            Value = value;
        }
        public override int Get() { return Value; }
    }
}
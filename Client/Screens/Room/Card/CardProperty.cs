namespace CardGame.Client
{
    public abstract class CardProperty<T>
    {
        public abstract T Value { get; set; }
        protected abstract T Property { get; set; }
        protected CardProperty(){}
    }
}
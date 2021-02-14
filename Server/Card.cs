namespace CardGame.Server
{
    public class Card
    {
        public readonly int Id;
        private readonly Player Owner;

        public Card(int id, Player owner)
        {
            Id = id;
            Owner = owner;
        }
    }
}
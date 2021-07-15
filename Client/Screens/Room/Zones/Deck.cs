namespace CardGame.Client
{
    public class Deck: Zone
    {
        
        public override void Add(Card card)
        {
            AddChild(card);
            Cards.Add(card);
            card.Translate(-OffSet * Cards.Count);
            card.CurrentZone = this;
            // Translate(OffSet);
        }

        public override void Insert(int index, Card card)
        {
            Cards.Insert(index, card);
            card.CurrentZone = this;
        }

        public override void Remove(Card card)
        {
            RemoveChild(card);
            Cards.Remove(card);
            card.CurrentZone = null;
            // Translate(OffSet);
        }
    }
}
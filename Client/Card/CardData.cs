using Godot;

namespace CardGame.Client
{
    public class CardData: Resource
    {
        // SetCode is initialized on load
        public SetCodes SetCode;
        [Export()] public string Name = "Card";
        [Export()] public Texture Art = (Texture) GD.Load($"Client/Assets/CardArt/NullCard.png");
        [Export(PropertyHint.Enum)] public CardTypes CardTypes = CardTypes.Null;
        [Export(PropertyHint.Enum)] public Factions Faction;
        [Export(PropertyHint.MultilineText)] public string Text = "";
        [Export()] public int Power;

        public void WriteTo(Card card)
        {
            card.Title = Name;
            card.CardType = CardTypes;
            card.Art = Art;
            card.Text = Text;
            card.Power = Power;
            card.Faction = Faction;
        }
    }
}
